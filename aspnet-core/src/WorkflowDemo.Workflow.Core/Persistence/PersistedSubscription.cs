using System;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedSubscription : Entity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

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
        public DateTime SubscribeAsOf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SubscriptionData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExternalToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(64)]
        public string ExternalWorkerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExternalTokenExpiry { get; set; }
    }
}
