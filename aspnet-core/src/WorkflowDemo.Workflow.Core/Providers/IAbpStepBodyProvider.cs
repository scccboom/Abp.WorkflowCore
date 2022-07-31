using Abp.Dependency;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    ///
    /// </summary>
    public interface IAbpStepBodyProvider
    {
        /// <summary>
        /// 设置码表类型
        /// </summary>
        /// <param name="context"></param>
        void Build(IAbpStepBodyDefinitionContext context);
    }
}