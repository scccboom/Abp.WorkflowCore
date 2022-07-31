using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;

using WorkflowDemo.Workflows;
using WorkflowDemo.Application.Workflows.Dtos;

namespace WorkflowDemo.Application.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowDefinitionAppService
        : AsyncCrudAppService<PersistedWorkflowDefinition, WorkflowDefinitionDto, string, WorkflowListInput, CreateWorkflowDefinitionDto, WorkflowDefinitionDto>
    {
        private readonly AbpWorkflowManager _abpWorkflowManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="abpWorkflowManager"></param>
        public WorkflowDefinitionAppService(IRepository<PersistedWorkflowDefinition, string> repository,
            AbpWorkflowManager abpWorkflowManager) 
            : base(repository)
        {
            _abpWorkflowManager = abpWorkflowManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<WorkflowDefinitionDto> CreateAsync(CreateWorkflowDefinitionDto input)
        {
            var entity = await _abpWorkflowManager.CreateAsync(MapToEntity(input));
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<string> input)
        {
            await _abpWorkflowManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<WorkflowDefinitionDto> UpdateAsync(WorkflowDefinitionDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            MapToEntity(input, entity);
            await _abpWorkflowManager.UpdateAsync(entity);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<PersistedWorkflowDefinition> CreateFilteredQuery(WorkflowListInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.Title.IsNotEmpty(), u => u.Title.Contains(input.Title));
        }

        /// <summary>
        /// 获取所有分组的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, IEnumerable<WorkflowDefinitionDto>>> GetAllWithGroupAsync(WorkflowListInput input)
        {
            var query = this.CreateFilteredQuery(input).Select(u => u);

            var list = await AsyncQueryableExecuter.ToListAsync(query);

            return list.GroupBy(u => u.Group)
                .OrderBy(i => i.Key)
                .ToDictionary(u => u.Key, u => u.Select(i => MapToEntityDto(i)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllGroupAsync()
        {
            var query = _abpWorkflowManager.WorkflowDefinitions.GroupBy(u => u.Group).Select(u => u.Key);

            var data = await AsyncQueryableExecuter.ToListAsync(query);

            return data.Where(u => u != null || u != "");
        }
    }
}
