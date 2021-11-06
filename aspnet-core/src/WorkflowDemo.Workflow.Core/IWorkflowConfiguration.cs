using Abp.Collections;
using Abp.Dependency;

namespace WorkflowDemo.Workflow
{
    public interface IWorkflowConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        ITypeList<AbpWorkflowProvider> Providers { get; }
    }

    internal class WorkflowConfiguration : IWorkflowConfiguration, ISingletonDependency
    {
        public ITypeList<AbpWorkflowProvider> Providers { get; }

        public WorkflowConfiguration()
        {
            Providers = new TypeList<AbpWorkflowProvider>();
        }
    }
}