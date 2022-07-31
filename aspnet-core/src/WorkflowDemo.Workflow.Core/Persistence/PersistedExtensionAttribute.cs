using Abp.Domain.Entities;

using System.ComponentModel.DataAnnotations;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedExtensionAttribute : Entity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersistedExecutionPointer ExecutionPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AttributeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AttributeValue { get; set; }
    }
}
