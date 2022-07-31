using System;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedWorkflowAuditor : CreationAuditedEntity<string>, IMayHaveTenant
    {
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersistedExecutionPointer ExecutionPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WorkflowAuditStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UserHeadPhoto { get; set; }

        /// <summary>
        /// 审核人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string UserIdentityName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersistedWorkflow Workflow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkflowId { get; set; }
    }
}
