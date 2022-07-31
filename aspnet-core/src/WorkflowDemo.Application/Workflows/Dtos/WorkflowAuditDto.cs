using System;
using System.Collections.Generic;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowAuditDto
    {
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public bool NeedAudit { get; set; }

        /// <summary>
        /// 审核记录
        /// </summary>
        public Dictionary<string, IEnumerable<WorkflowAuditRecord>> AuditRecords { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowAuditRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserIdentityName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserHeadPhoto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowAuditStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
    }
}