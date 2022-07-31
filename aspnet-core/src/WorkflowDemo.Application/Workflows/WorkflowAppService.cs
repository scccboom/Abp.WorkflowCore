using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;

using WorkflowDemo.Application.Workflows.Dtos;
using WorkflowDemo.Extensions;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Application.Workflows
{
    /// <summary>
    /// 工作流
    /// </summary>
    [AbpAuthorize]
    public class WorkflowAppService
        : AsyncCrudAppService<PersistedWorkflow, WorkflowDto, string, WorkflowListInput>,
        IWorkflowAppService
    {
        private readonly AbpWorkflowManager _abpWorkflowManager;

        private readonly IAbpWorkflowRepository _workflowRepository;

        private readonly IPrincipalAccessor _principal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="workflowRepository"></param>
        /// <param name="abpWorkflowManager"></param>
        /// <param name="principal"></param>
        public WorkflowAppService(
            IRepository<PersistedWorkflow, string> repository,
            IAbpWorkflowRepository workflowRepository,
            AbpWorkflowManager abpWorkflowManager,
            IPrincipalAccessor principal
        )
            : base(repository)
        {
            _abpWorkflowManager = abpWorkflowManager;
            _workflowRepository = workflowRepository;
            _principal = principal;
        }

        /// <summary>
        /// 我发起的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MyWorkflowListOutput>> GetMyWorkflowAsync(WorkflowListInput input)
        {
            var query = _workflowRepository.GetAll()
                .Where(u => u.CreatorUserId == AbpSession.UserId)
                .WhereIf(input.Title.IsNotEmpty(), u => u.WorkflowDefinition.Title.Contains(input.Title));

            return await query
                .OrderByDescending(u => u.CreationTime)
                .ToPagedResultAsync(input, u => new MyWorkflowListOutput()
                {
                    Title = u.WorkflowDefinition.Title,
                    Version = u.Version,
                    Id = u.Id,
                    Status = u.Status,
                    CompleteTime = u.CompleteTime,
                    CreationTime = u.CreationTime,
                    CurrentStepName = u.ExecutionPointers.OrderBy(i => i.StepId).Last().StepName,
                    Nodes = u.WorkflowDefinition.Nodes
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AbpWorkflowStepDefinition> GetAllStepBodys()
        {
            return _abpWorkflowManager.GetAllStepBodys();
        }

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task StartAsync(StartWorkflowInput input)
        {
            var userName = _principal.GetUserName();
            input.Inputs.Add("UserName", userName);
            input.Inputs.Add("UserId", AbpSession.UserId);
            await _abpWorkflowManager.StartWorlflow(input.Id, input.Version, input.Inputs);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input">待审核ID</param>
        /// <returns></returns>
        public override async Task<WorkflowDto> GetAsync(EntityDto<string> input)
        {
            var result = await _workflowRepository.GetByIdWithDetailsAsync(input.Id);
            
            var dto = MapToEntityDto(result);
            dto.ExecutionRecords = dto.ExecutionRecords.OrderByDescending(x => x.StepId).ToList();
            return dto;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="input"></param>
        public async Task PublicEvent(PublishEventInput input)
        {
            await _abpWorkflowManager.PublishEventAsync(input.EventName, input.EventKey, input.EventData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public override Task<WorkflowDto> UpdateAsync(WorkflowDto input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public override Task<WorkflowDto> CreateAsync(WorkflowDto input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public override Task DeleteAsync(EntityDto<string> input)
        {
            throw new NotImplementedException();
        }
    }
}