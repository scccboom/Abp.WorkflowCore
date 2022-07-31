using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using Abp.Application.Services.Dto;

using WorkflowCore.Models;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class MyWorkflowListOutput : EntityDto<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
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
        public string CurrentStepName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentStepTitle
        {
            get
            {
                return Nodes.FirstOrDefault(i => i.Key == CurrentStepName)?.Title;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public IEnumerable<WorkflowNode> Nodes { get; set; }
    }
}