using System;
using System.Linq;
using System.Threading.Tasks;

using Abp;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.UI.Inputs;

using Microsoft.Extensions.Logging;

using WorkflowCore.Interface;
using WorkflowCore.Models;

using WorkflowDemo.Authorization.Users;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 通用审批
    /// </summary>
    public class GeneralAuditingStepBody : StepBodyAsync, ITransientDependency
    {
        private const string ActionName = "AuditEvent";

        private readonly ILogger _logger;

        private readonly INotificationPublisher _notificationPublisher;

        private readonly IAbpPersistenceProvider _abpPersistenceProvider;

        private readonly UserManager _userManager;

        private readonly IRepository<PersistedWorkflowAuditor, string> _auditorRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="notificationPublisher"></param>
        /// <param name="userManager"></param>
        /// <param name="abpPersistenceProvider"></param>
        /// <param name="auditorRepository"></param>
        public GeneralAuditingStepBody(
            ILogger<GeneralAuditingStepBody> logger,
            INotificationPublisher notificationPublisher,
            UserManager userManager, IAbpPersistenceProvider abpPersistenceProvider,
            IRepository<PersistedWorkflowAuditor, string> auditorRepository)
        {
            _logger = logger;
            _notificationPublisher = notificationPublisher;
            _abpPersistenceProvider = abpPersistenceProvider;
            _userManager = userManager;
            _auditorRepository = auditorRepository;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [UnitOfWork]
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            _logger.LogDebug($"==Run wf:{context.Workflow.Id} GeneralAuditingStepBody==");

            DateTime effectiveDate = DateTime.MinValue;

            if (!context.ExecutionPointer.EventPublished)
            {
                _logger.LogDebug($"Wait wf:{context.Workflow.Id} event");

                var workflow = await _abpPersistenceProvider.GetPersistedWorkflow(context.Workflow.Id);

                var workflowDefinition = await _abpPersistenceProvider.GetPersistedWorkflowDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version);

                var userIdentityName = _userManager.Users.Where(u => u.Id == workflow.CreatorUserId).Select(u => u.FullName).FirstOrDefault();

                //通知审批人
                var notificationData = new NotificationData();
                notificationData.Properties["content"] = $"【{userIdentityName}】提交的{workflowDefinition.Title}需要您审批！";
                _notificationPublisher.Publish(
                    notificationName: "Task", 
                    data: notificationData,
                    userIds: new UserIdentifier[] { new UserIdentifier(workflow.TenantId, UserId) },
                    entityIdentifier: new EntityIdentifier(workflow.GetType(), workflow.Id)
                );

                //添加审核人记录
                var auditUserInfo = _userManager.GetUserById(UserId);
                _auditorRepository.Insert(new PersistedWorkflowAuditor()
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkflowId = workflow.Id,
                    ExecutionPointerId = context.ExecutionPointer.Id,
                    Status = WorkflowAuditStatus.UnAudited,
                    UserId = UserId,
                    TenantId = workflow.TenantId,
                    UserIdentityName = auditUserInfo.FullName
                });

                return ExecutionResult.WaitForEvent(ActionName, Guid.NewGuid().ToString(), effectiveDate);
            }

            var auditors = _auditorRepository.GetAll()
                .Where(u => u.ExecutionPointerId == context.ExecutionPointer.Id && u.UserId == UserId)
                .ToList();

            if (!auditors.Any())
            {
                _logger.LogDebug($"Unknow auditor wf:{context.Workflow.Id}");

                return ExecutionResult.WaitForEvent(ActionName, Guid.NewGuid().ToString(), effectiveDate);
            }

            var pass = auditors.Any(x => x.Status == WorkflowAuditStatus.Pass);

            if (pass)
            {
                _logger.LogDebug($"wf:{context.Workflow.Id} pass");

                return ExecutionResult.Next();
            }

            _logger.LogDebug($"wf:{context.Workflow.Id} unapprove");

            context.Workflow.Status = WorkflowStatus.Complete;
            return ExecutionResult.Next();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AbpWorkflowStepDefinition Create()
        {
            var step = new AbpWorkflowStepDefinition
            {
                Name = "FixedUserAudit",
                DisplayName = "指定用户审核",
                StepBodyType = typeof(GeneralAuditingStepBody),
                Inputs = new WorkflowParamDictionary()
                {
                    {
                        "UserId", 
                        new WorkflowParam()
                        {
                            InputType = new SelectUserInputType(),
                            Name = "UserId",
                            DisplayName = "审核人"
                        } 
                    }
                }
            };
            return step;
        }
    }
}