using System;

using Abp.AutoMapper;

using AutoMapper.Configuration.Annotations;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowExecutionRecordDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StepTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}