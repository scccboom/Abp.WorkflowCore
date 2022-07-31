using Abp.Dependency;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAbpStepBodyProvider : IAbpStepBodyProvider, ISingletonDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Build(IAbpStepBodyDefinitionContext context)
        {
            context.Create(GeneralAuditingStepBody.Create());

            context.Create(RoleAuditingStepBody.Create());

            context.Create(NullStepBody.Create());

            context.Create(EndStepBody.Create());
        }
    }
}