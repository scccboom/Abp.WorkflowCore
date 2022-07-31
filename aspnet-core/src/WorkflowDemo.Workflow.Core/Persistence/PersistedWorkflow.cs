using System;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using WorkflowCore.Models;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedWorkflow : FullAuditedEntity<string>, IMayHaveTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public PersistedWorkflowDefinition WorkflowDefinition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual PersistedExecutionPointerCollection ExecutionPointers { get; set; } = new PersistedExecutionPointerCollection();

        /// <summary>
        /// 
        /// </summary>
        public long? NextExecution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserIdentityName { get; set; }
    }
}
