using System;
using System.Linq;

using Abp;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.UI.Inputs;

using WorkflowCore.Interface;
using WorkflowCore.Models;

using WorkflowDemo.Authorization.Users;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 指定角色审批
    /// </summary>
    public class RoleAuditingStepBody : StepBody, ITransientDependency
    {
        private const string ActionName = "AuditEvent";

        private readonly INotificationPublisher _notificationPublisher;

        private readonly IAbpPersistenceProvider _abpPersistenceProvider;

        private readonly UserManager _userManager;

        private readonly IRepository<PersistedWorkflowAuditor, string> _auditorRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="notificationPublisher"></param>
        /// <param name="abpPersistenceProvider"></param>
        /// <param name="auditorRepository"></param>
        public RoleAuditingStepBody(
            UserManager userManager, 
            INotificationPublisher notificationPublisher, 
            IAbpPersistenceProvider abpPersistenceProvider, 
            IRepository<PersistedWorkflowAuditor, string> auditorRepository)
        {
            _notificationPublisher = notificationPublisher;
            _abpPersistenceProvider = abpPersistenceProvider;
            _userManager = userManager;
            _auditorRepository = auditorRepository;
        }

        /// <summary>
        /// 审核角色
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [UnitOfWork]
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var userIdentitys = _userManager.GetUsersInRoleAsync(RoleName).Result.Select(u => new { u.TenantId, u.Id, u.FullName });
            if (userIdentitys.Count() == 0)
            {
                context.Workflow.Status = WorkflowStatus.Complete;
                return ExecutionResult.Next();
            }

            if (!context.ExecutionPointer.EventPublished)
            {
                var workflow = _abpPersistenceProvider.GetPersistedWorkflow(context.Workflow.Id).Result;
                var workflowDefinition = _abpPersistenceProvider.GetPersistedWorkflowDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version).Result;

                var userIdentityName = _userManager.Users.Where(u => u.Id == workflow.CreatorUserId).Select(u => u.FullName).FirstOrDefault();
                //通知审批人
                var notifyData = new NotificationData();
                notifyData.Properties["content"] = $"【{userIdentityName}】提交的{workflowDefinition.Title}需要您审批！";
                _notificationPublisher.PublishAsync("Task", notifyData,
                    userIds: userIdentitys.Select(u => new UserIdentifier(u.TenantId, u.Id)).ToArray(),
                     entityIdentifier: new EntityIdentifier(workflow.GetType(), workflow.Id)
                    ).Wait();
                //添加审批人
                foreach (var item in userIdentitys)
                {
                    _auditorRepository.Insert(new PersistedWorkflowAuditor() { WorkflowId = workflow.Id, ExecutionPointerId = context.ExecutionPointer.Id, Status = WorkflowAuditStatus.UnAudited, UserIdentityName = item.FullName, UserId = item.Id, TenantId = item.TenantId });
                }
                DateTime effectiveDate = DateTime.MinValue;
                return ExecutionResult.WaitForEvent(ActionName, Guid.NewGuid().ToString(), effectiveDate);
            }

            var pass = _auditorRepository.GetAll().Count(u => u.ExecutionPointerId == context.ExecutionPointer.Id && u.Status == WorkflowAuditStatus.Pass);

            //需全部审核通过
            if (pass < userIdentitys.Count())
            {
                context.Workflow.Status = WorkflowStatus.Complete;
                return ExecutionResult.Next();
            }
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
                Name = "FixedRoleAudit",
                DisplayName = "指定角色审核",
                StepBodyType = typeof(RoleAuditingStepBody),
                Inputs = new WorkflowParamDictionary
                {
                    {
                        "RoleName",
                        new WorkflowParam()
                        {
                            InputType = new SelectRoleInputType(),
                            Name = "RoleName",
                            DisplayName = "审核角色名"
                        }
                    }
                }
            };
            return step;
        }
    }
}