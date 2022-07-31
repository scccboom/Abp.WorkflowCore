using System;
using System.Collections.Generic;

using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using WorkflowCore.Models;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMapFrom(typeof(PersistedWorkflow))]
    public class WorkflowDto : EntityDto<string>
    {
        /// <summary>
        /// 流程名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreationTime { get; set; }

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
        public string WorkflowDefinitionId { get; set; }

        /// <summary>
        /// 流程定义输入的数据
        /// </summary>
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }

        /// <summary>
        /// 流程输入数据
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<WorkflowExecutionRecordDto> ExecutionRecords { get; set; }
    }
}