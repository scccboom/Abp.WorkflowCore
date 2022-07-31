using System.Collections.Generic;

using Abp;
using Abp.Collections.Extensions;

namespace WorkflowDemo.Workflows
{
    public abstract class AbpStepBodyDefinitionContextBase : IAbpStepBodyDefinitionContext
    {
        protected readonly Dictionary<string, AbpWorkflowStepDefinition> AbpStepBodys;

        public AbpStepBodyDefinitionContextBase()
        {
            AbpStepBodys = new Dictionary<string, AbpWorkflowStepDefinition>();
        }

        public void Create(AbpWorkflowStepDefinition entity)
        {
            if (AbpStepBodys.ContainsKey(entity.Name))
            {
                throw new AbpException("There is already a AbpStepBody with name: " + entity.Name);
            }
            AbpStepBodys[entity.Name] = entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<AbpWorkflowStepDefinition> GetAllStepBodys()
        {
            return AbpStepBodys.Values;
        }

        public AbpWorkflowStepDefinition GetStepBodyOrNull(string name)
        {
            return AbpStepBodys.GetOrDefault(name);
        }

        public void RemoveStepBody(string name)
        {
            AbpStepBodys.Remove(name);
        }
    }
}