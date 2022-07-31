using Abp.Dependency;

namespace WorkflowDemo.Workflows
{
    public class AbpStepBodyDefinitionContext : AbpStepBodyDefinitionContextBase, ISingletonDependency
    {
        private readonly IIocManager _iocManager;

        public AbpStepBodyDefinitionContext(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            using var scope = _iocManager.CreateScope();

            var stepBodyProviders = scope.ResolveAll<IAbpStepBodyProvider>();

            foreach (var provider in stepBodyProviders)
            {
                provider.Build(this);
            }
        }
    }
}