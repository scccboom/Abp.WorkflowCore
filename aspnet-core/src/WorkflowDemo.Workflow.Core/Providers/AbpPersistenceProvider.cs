using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq;

using WorkflowCore.Models;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class AbpPersistenceProvider : DomainService, IAbpPersistenceProvider
    {
        protected readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        protected readonly IGuidGenerator _guidGenerator;

        protected readonly IAbpWorkflowRepository _workflowRepository;

        protected readonly IRepository<PersistedWorkflowDefinition, string> _workflowDefinitionRepository;

        protected readonly IRepository<PersistedEvent, string> _eventRepository;

        protected readonly IRepository<PersistedSubscription, string> _eventSubscriptionRepository;

        protected readonly IRepository<PersistedExecutionError, string> _executionErrorRepository;

        protected readonly IRepository<PersistedExecutionPointer, string> _executionPointerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidGenerator"></param>
        /// <param name="asyncQueryableExecuter"></param>
        /// <param name="eventRepository"></param>
        /// <param name="executionPointerRepository"></param>
        /// <param name="workflowRepository"></param>
        /// <param name="eventSubscriptionRepository"></param>
        /// <param name="executionErrorRepository"></param>
        /// <param name="workflowDefinitionRepository"></param>
        public AbpPersistenceProvider(
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IGuidGenerator guidGenerator,
            IAbpWorkflowRepository workflowRepository,
            IRepository<PersistedWorkflowDefinition, string> workflowDefinitionRepository,
            IRepository<PersistedEvent, string> eventRepository,
            IRepository<PersistedSubscription, string> eventSubscriptionRepository,
            IRepository<PersistedExecutionError, string> executionErrorRepository,
            IRepository<PersistedExecutionPointer, string> executionPointerRepository)
        {
            _asyncQueryableExecuter = asyncQueryableExecuter;
            _guidGenerator = guidGenerator;
            _eventRepository = eventRepository;
            _workflowRepository = workflowRepository;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _eventSubscriptionRepository = eventSubscriptionRepository;
            _executionErrorRepository = executionErrorRepository;
            _executionPointerRepository = executionPointerRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscriptionId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [UnitOfWork]
        public virtual async Task ClearSubscriptionToken(string eventSubscriptionId, string token)
        {
            var existingEntity = await _eventSubscriptionRepository
                .FirstOrDefaultAsync(x => x.Id == eventSubscriptionId);

            if (existingEntity.ExternalToken != token)
                throw new InvalidOperationException();

            existingEntity.ExternalToken = null;
            existingEntity.ExternalWorkerId = null;
            existingEntity.ExternalTokenExpiry = null;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<string> CreateEvent(Event newEvent)
        {
            newEvent.Id = _guidGenerator.Create().ToString();
            var persistable = newEvent.ToPersistable();
            _ = _eventRepository.InsertAsync(persistable);
            await CurrentUnitOfWork.SaveChangesAsync();
            return newEvent.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<string> CreateEventSubscription(EventSubscription subscription)
        {
            subscription.Id = _guidGenerator.Create().ToString();
            var persistable = subscription.ToPersistable();
            await _eventSubscriptionRepository.InsertAsync(persistable);
            await CurrentUnitOfWork.SaveChangesAsync();
            return subscription.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<string> CreateNewWorkflow(WorkflowInstance workflow)
        {
            workflow.Id = _guidGenerator.Create().ToString();
            var persistable = workflow.ToPersistable();
            var inputs = workflow.Data as Dictionary<string, object>;
            if (inputs != null)
            {
                persistable.CreateUserIdentityName = Convert.ToString(inputs["UserName"]);
            }
            await _workflowRepository.InsertAsync(persistable);

            await CurrentUnitOfWork.SaveChangesAsync();

            return workflow.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        [UnitOfWork]
        public virtual void EnsureStoreExists()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="definitionId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedWorkflow>> GetAllRunnablePersistedWorkflow(string definitionId, int version)
        {
            var query = _workflowRepository.GetAll()
                .Where(u => u.WorkflowDefinitionId == definitionId && u.Version == version);
            return await _asyncQueryableExecuter.ToListAsync(query);
        }

        [UnitOfWork]
        public virtual async Task<Event> GetEvent(string id)
        {
            var raw = await _eventRepository
                .FirstOrDefaultAsync(x => x.Id == id);

            if (raw == null)
                return null;

            return raw.ToEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventKey"></param>
        /// <param name="asOf"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetEvents(string eventName, string eventKey, DateTime asOf)
        {
            var query = _eventRepository.GetAll()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey)
                .Where(x => x.EventTime >= asOf)
                .Select(x => x.Id);

            var raw = await _asyncQueryableExecuter.ToListAsync(query);

            var result = new List<string>();

            foreach (var s in raw)
            {
                result.Add(s.ToString());
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventKey"></param>
        /// <param name="asOf"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey, DateTime asOf)
        {
            var raw = await _eventSubscriptionRepository
                .FirstOrDefaultAsync(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf && x.ExternalToken == null);

            return raw?.ToEventSubscription();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id)
        {
            return _executionPointerRepository.GetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PersistedWorkflow> GetPersistedWorkflow(string id)
        {
            return _workflowRepository.GetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version)
        {
            return _workflowDefinitionRepository
                .FirstOrDefaultAsync(u => u.Id == id && u.Version == version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asAt"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetRunnableEvents(DateTime asAt)
        {
            var now = asAt.ToUniversalTime();

            var query = _eventRepository.GetAll()
                .Where(x => !x.IsProcessed)
                .Where(x => x.EventTime <= now)
                .Select(x => x.Id);

            var raw = await _asyncQueryableExecuter.ToListAsync(query);

            return raw.Select(s => s.ToString()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asAt"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt)
        {
            var now = asAt.ToUniversalTime().Ticks;

            var query = _workflowRepository.GetAll()
                .Where(x => x.NextExecution.HasValue && (x.NextExecution <= now) && (x.Status == WorkflowStatus.Runnable))
                .Select(x => x.Id);
            var raw = await _asyncQueryableExecuter.ToListAsync(query);

            return raw.Select(s => s.ToString()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscriptionId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<EventSubscription> GetSubscription(string eventSubscriptionId)
        {
            var raw = await _eventSubscriptionRepository.FirstOrDefaultAsync(x => x.Id == eventSubscriptionId);

            return raw?.ToEventSubscription();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventKey"></param>
        /// <param name="asOf"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey, DateTime asOf)
        {
            asOf = asOf.ToUniversalTime();
            var query = _eventSubscriptionRepository.GetAll()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf);
            var raw = await _asyncQueryableExecuter.ToListAsync(query);

            return raw.Select(item => item.ToEventSubscription()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<WorkflowInstance> GetWorkflowInstance(string id)
        {
            var query = _workflowRepository.GetAllThenDetail()
                .Where(x => x.Id == id);

            var raw = await _asyncQueryableExecuter.FirstOrDefaultAsync(query);

            if (raw == null)
                return null;

            var ordered = raw.ExecutionPointers.OrderByDescending(x => x.StepId).ToList();
            raw.ExecutionPointers.Clear();

            raw.ExecutionPointers = new PersistedExecutionPointerCollection(ordered.Count + 8);

            foreach (var item in ordered)
            {
                raw.ExecutionPointers.Add(item);
            }

            return raw.ToWorkflowInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take)
        {
            IQueryable<PersistedWorkflow> query = _workflowRepository.GetAllThenDetail();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!String.IsNullOrEmpty(type))
                query = query.Where(x => x.WorkflowDefinitionId == type);

            if (createdFrom.HasValue)
                query = query.Where(x => x.CreateTime >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(x => x.CreateTime <= createdTo.Value);

            query = query.Skip(skip).Take(take);

            var rawResult = await _asyncQueryableExecuter.ToListAsync(query);
            List<WorkflowInstance> result = new List<WorkflowInstance>();

            foreach (var item in rawResult)
                result.Add(item.ToWorkflowInstance());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids)
        {
            if (ids == null)
            {
                return new List<WorkflowInstance>();
            }

            var raw = _workflowRepository.GetAllThenDetail()
                .Where(x => ids.Contains(x.Id));

            return (await _asyncQueryableExecuter.ToListAsync(raw)).Select(i => i.ToWorkflowInstance());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task MarkEventProcessed(string id)
        {
            var existingEntity = await _eventRepository.FirstOrDefaultAsync(x => x.Id == id);

            existingEntity.IsProcessed = true;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task MarkEventUnprocessed(string id)
        {
            var existingEntity = await _eventRepository.FirstOrDefaultAsync(x => x.Id == id);

            existingEntity.IsProcessed = false;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task PersistErrors(IEnumerable<ExecutionError> errors)
        {
            var executionErrors = errors as ExecutionError[] ?? errors.ToArray();
            if (executionErrors.Any())
            {
                foreach (var error in executionErrors)
                {
                    await _executionErrorRepository.InsertAsync(error.ToPersistable());
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task PersistWorkflow(WorkflowInstance workflow)
        {
            var query = _workflowRepository.GetAllThenDetail(true)
                .Where(x => x.Id == workflow.Id);
            var existingEntity = await _asyncQueryableExecuter.FirstOrDefaultAsync(query);
            var persistable = workflow.ToPersistable(existingEntity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscriptionId"></param>
        /// <param name="token"></param>
        /// <param name="workerId"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> SetSubscriptionToken(string eventSubscriptionId, string token, string workerId, DateTime expiry)
        {
            var existingEntity = await _eventSubscriptionRepository
                .FirstOrDefaultAsync(x => x.Id == eventSubscriptionId);

            existingEntity.ExternalToken = token;
            existingEntity.ExternalWorkerId = workerId;
            existingEntity.ExternalTokenExpiry = expiry;

            await CurrentUnitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscriptionId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task TerminateSubscription(string eventSubscriptionId)
        {
            var existing = await _eventSubscriptionRepository
                .FirstOrDefaultAsync(x => x.Id == eventSubscriptionId);
            _eventSubscriptionRepository.Delete(existing);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}