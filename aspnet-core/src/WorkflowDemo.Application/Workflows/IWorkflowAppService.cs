using Abp.Application.Services;
using Abp.Application.Services.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

using WorkflowDemo.Application.Workflows.Dtos;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkflowAppService : IAsyncCrudAppService<WorkflowDto, string, WorkflowListInput>
    {
        /// <summary>
        /// 发起工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task StartAsync(StartWorkflowInput input);

        /// <summary>
        /// 我发起的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MyWorkflowListOutput>> GetMyWorkflowAsync(WorkflowListInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<AbpWorkflowStepDefinition> GetAllStepBodys();

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PublicEvent(PublishEventInput input);
    }
}
