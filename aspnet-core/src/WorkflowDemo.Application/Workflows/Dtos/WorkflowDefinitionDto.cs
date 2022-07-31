using System.Collections.Generic;

using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(PersistedWorkflowDefinition))]
    public class WorkflowDefinitionDto : EntityDto<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

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
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<WorkflowNode> Nodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowDefinitionDto()
        {
            Version = 1;
        }
    }
}