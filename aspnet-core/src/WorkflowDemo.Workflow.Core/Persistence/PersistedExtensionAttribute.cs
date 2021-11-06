using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Abp.Domain.Entities;

namespace WorkflowDemo.Workflow
{
    [Table("WorkflowExtensionAttributes")]
    public class PersistedExtensionAttribute : Entity<long>
    {
        public string ExecutionPointerId { get; set; }

        [ForeignKey("ExecutionPointerId")]
        public PersistedExecutionPointer ExecutionPointer { get; set; }

        [MaxLength(100)]
        public string AttributeKey { get; set; }

        public string AttributeValue { get; set; }
    }
}
