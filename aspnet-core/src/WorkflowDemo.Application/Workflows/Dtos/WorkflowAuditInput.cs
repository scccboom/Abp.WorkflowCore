using System.ComponentModel.DataAnnotations;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowAuditInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }
    }
}