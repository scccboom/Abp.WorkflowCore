using System;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedExecutionError : Entity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ErrorTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
