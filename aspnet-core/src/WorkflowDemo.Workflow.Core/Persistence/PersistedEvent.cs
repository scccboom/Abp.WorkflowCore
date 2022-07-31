using System;

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedEvent : CreationAuditedEntity<string>, IMayHaveTenant
    {
        /// <summary>
        /// 
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TenantId { get; set; }
    }
}
