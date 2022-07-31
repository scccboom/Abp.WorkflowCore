using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Abp.Json;

using AutoMapper;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowDtoProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkflowDtoProfile()
        {
            CreateMap<PersistedExecutionPointer, WorkflowExecutionRecordDto>()
                .ForMember(x => x.ExecutionPointerId, opt => opt.MapFrom(x => x.Id));

            CreateMap<PersistedWorkflow, WorkflowDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(s => s.WorkflowDefinition.Title))
                .ForMember(x => x.Data, opt => opt.MapFrom((s, d) => s.Data?.FromJsonString<Dictionary<string, object>>()))
                .ForMember(x => x.ExecutionRecords, opt => opt.MapFrom(s => s.ExecutionPointers))
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.CreateUserIdentityName))
                .ForMember(x => x.Inputs, opt =>
                {
                    opt.PreCondition(s => s.WorkflowDefinition != null);
                    opt.MapFrom(s => s.WorkflowDefinition.Inputs);
                })
                .AfterMap((s, d) =>
                {
                    foreach (var item in d.ExecutionRecords)
                    {
                        item.StepTitle = s.WorkflowDefinition?.Nodes?.FirstOrDefault(i => i.Key == item.StepName)?.Title;
                    }
                });
        }
    }
}
