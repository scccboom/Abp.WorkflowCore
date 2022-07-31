using Abp.BaseDto;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowListInput : PagedInputDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
    }
}