using System;
using System.Collections.Generic;
using System.Text;

using Abp;

using AutoMapper;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowDefinitionMapProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkflowDefinitionMapProfile()
        {
            CreateMap<CreateWorkflowDefinitionDto, PersistedWorkflowDefinition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
