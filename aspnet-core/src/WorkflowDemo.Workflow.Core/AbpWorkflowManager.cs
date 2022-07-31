using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Json;
using Abp.UI;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.DefinitionStorage.v1;
using WorkflowCore.Services.DefinitionStorage;

namespace WorkflowDemo.Workflows
{
    public class AbpWorkflowManager : DomainService
    {
        protected readonly AbpStepBodyDefinitionContext _workflowDefinitionManager;

        protected IWorkflowHost _workflowHost;

        protected readonly IWorkflowController _workflowService;

        protected readonly IWorkflowRegistry _registry;

        public IAbpPersistenceProvider PersistenceProvider { get; }

        protected readonly ISearchIndex _searchService;

        protected readonly IDefinitionLoader _definitionLoader;

        protected readonly IRepository<PersistedWorkflowDefinition, string> _workflowDefinitionRepository;

        protected IReadOnlyCollection<AbpWorkflowStepDefinition> _stepBodys;

        public IQueryable<PersistedWorkflowDefinition> WorkflowDefinitions => _workflowDefinitionRepository.GetAll();

        public AbpWorkflowManager(
            IWorkflowHost workflowHost, 
            IWorkflowController workflowService, 
            IWorkflowRegistry registry, 
            ISearchIndex searchService, 
            IDefinitionLoader definitionLoader, 
            IAbpPersistenceProvider workflowStore, 
            AbpStepBodyDefinitionContext workflowDefinitionManager, 
            IRepository<PersistedWorkflowDefinition, string> workflowDefinitionRepository
            )
        {
            _workflowDefinitionManager = workflowDefinitionManager;
            _workflowHost = workflowHost;
            _workflowService = workflowService;
            _registry = registry;
            PersistenceProvider = workflowStore;
            _searchService = searchService;
            _definitionLoader = definitionLoader;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _stepBodys = _workflowDefinitionManager.GetAllStepBodys();
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public virtual async Task<bool> TerminateWorkflow(string workflowId)
        {
            return await _workflowService.TerminateWorkflow(workflowId);
        }

        public virtual WorkflowDefinition GetDefinition(string workflowId, int? version = null)
        {
            return _registry.GetDefinition(workflowId, version);
        }

        /// <summary>
        ///  初始化注册流程
        /// </summary>
        public void Initialize()
        {
            using var uow = UnitOfWorkManager.Begin();

            var workflows = WorkflowDefinitions.ToList();

            foreach (var workflow in workflows)
            {
                LoadDefinition(workflow);
            }

            uow.Complete();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<AbpWorkflowStepDefinition> GetAllStepBodys()
        {
            return _stepBodys;
        }

        public virtual async Task PublishEventAsync(string eventName, string eventKey, object eventData)
        {
            await _workflowHost.PublishEvent(eventName, eventKey, eventData);
        }

        /// <summary>
        /// 创建流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PersistedWorkflowDefinition> CreateAsync(PersistedWorkflowDefinition entity)
        {
            LoadDefinition(entity);
            await _workflowDefinitionRepository.InsertAsync(entity);
            return entity;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string id)
        {
            var entity = await _workflowDefinitionRepository.GetAsync(id);

            var all = await PersistenceProvider.GetAllRunnablePersistedWorkflow(entity.Id, entity.Version);
            if (all.Count() > 0)
            {
                throw new UserFriendlyException("删不了！！还有没有执行完的流程！");
            }

            if (_registry.IsRegistered(entity.Id.ToString(), entity.Version))
            {
                _registry.DeregisterWorkflow(entity.Id.ToString(), entity.Version);
            }

            await _workflowDefinitionRepository.DeleteAsync(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(PersistedWorkflowDefinition entity)
        {
            if (_registry.IsRegistered(entity.Id.ToString(), entity.Version))
            {
                _registry.DeregisterWorkflow(entity.Id.ToString(), entity.Version);
            }

            LoadDefinition(entity);
            await _workflowDefinitionRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task StartWorlflow(string id, int version, Dictionary<string, object> inputs)
        {
            if (!_registry.IsRegistered(id, version))
            {
                throw new UserFriendlyException("the workflow  has not been defined!");
            }
            await _workflowHost.StartWorkflow(id, version, inputs);
        }

        /// <summary>
        /// 注册工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal WorkflowDefinition LoadDefinition(PersistedWorkflowDefinition input)
        {
            if (_registry.IsRegistered(input.Id.ToString(), input.Version))
            {
                throw new AbpException($"the workflow {input.Id} has ben definded!");
            }
            var source = new DefinitionSourceV1
            {
                Id = input.Id.ToString(),
                Version = input.Version,
                Description = input.Title,
                DataType = $"{typeof(Dictionary<string, object>).FullName}, {typeof(Dictionary<string, object>).Assembly.FullName}"
            };

            BuildWorkflow(input.Nodes, source, _stepBodys, input.Nodes.First(u => u.Key.ToLower().StartsWith("start")));

            var json = source.ToJsonString();
            Logger.DebugFormat("Workflow Json:{0}", json);
            var def = _definitionLoader.LoadDefinition(json, Deserializers.Json);
            return def;
        }

        protected virtual void BuildWorkflow(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, IEnumerable<AbpWorkflowStepDefinition> stepNodes, WorkflowNode node)
        {
            if (source.Steps.Any(u => u.Id == node.Key))
            {
                return;
            }

            var stepSource = new StepSourceV1
            {
                Id = node.Key,
                Name = node.Key
            };
            AbpWorkflowStepDefinition stepNode = stepNodes.FirstOrDefault(u => u.Name == node.StepBody.Name);
            if (stepNode == null)
            {
                var sn = "null";

                if (node.Key.ToLower().StartsWith("end"))
                {
                    sn = "end";
                }

                stepNode = stepNodes.FirstOrDefault(x => x.Name == sn);
            }
            stepSource.StepType = $"{stepNode.StepBodyType.FullName}, {stepNode.StepBodyType.Assembly.FullName}";

            foreach (var input in stepNode.Inputs)
            {
                var value = node.StepBody.Inputs[input.Key].Value;
                if (!(value is IDictionary<string, object> || value is IDictionary<object, object>))
                {
                    value = $"\"{value}\"";
                }
                stepSource.Inputs.TryAdd(input.Key, value);
            }
            source.Steps.Add(stepSource);
            BuildBranching(allNodes, source, stepSource, stepNodes, node.NextNodes);
        }

        protected virtual void BuildBranching(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, StepSourceV1 stepSource, IEnumerable<AbpWorkflowStepDefinition> stepNodes, IEnumerable<WorkflowConditionNode> condNodes)
        {
            foreach (var nextNode in condNodes)
            {
                var node = allNodes.First(u => u.Key == nextNode.NodeId);
                stepSource.SelectNextStep[nextNode.NodeId] = "1==1";
                if (nextNode.Conditions.Count() > 0)
                {
                    List<string> exps = new List<string>();
                    foreach (var cond in nextNode.Conditions)
                    {
                        if (cond.Value is string && (!decimal.TryParse(cond.Value.ToString(), out decimal tempValue)))
                        {
                            if (cond.Operator != "==" && cond.Operator != "!=")
                            {
                                throw new AbpException($" if {cond.Field} is type of 'String', the Operator must be \"==\" or \"!=\"");
                            }
                            exps.Add($"data[\"{cond.Field}\"].ToString() {cond.Operator} \"{cond.Value}\"");
                            continue;
                        }
                        exps.Add($"decimal.Parse(data[\"{cond.Field}\"].ToString()) {cond.Operator} {cond.Value}");
                    }
                    stepSource.SelectNextStep[nextNode.NodeId] = string.Join(" && ", exps);
                }

                BuildWorkflow(allNodes, source, stepNodes, node);
            }
        }
    }
}