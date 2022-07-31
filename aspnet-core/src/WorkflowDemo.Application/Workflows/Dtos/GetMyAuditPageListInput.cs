using System;

using Abp.Application.Services.Dto;
using Abp.BaseDto;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class GetMyAuditPageListInput : PagedInputDto
    {
        /// <summary>
        ///
        /// </summary>
        public bool? AuditedMark { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetMyAuditPageListOutput : EntityDto<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowDefinitionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 流程名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowAuditStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? AuditTime { get; set; }
    }
}