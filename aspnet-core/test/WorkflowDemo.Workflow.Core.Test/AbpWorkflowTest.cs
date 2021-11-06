using Abp.TestBase;

using Shouldly;

using WorkflowCore.Interface;

using Xunit;

namespace WorkflowDemo.Workflow.Test
{
    public class AbpWorkflowTest : AbpIntegratedTestBase<WorkflowDemoWorkflowCoreTestModule>
    {
        protected readonly IWorkflowRegistry _registry;

        public AbpWorkflowTest(IWorkflowRegistry registry)
        {
            _registry = registry;
        }

        [Fact]
        public void TestAbpWorkflowIsRegistiedAuto()
        {
            var ret = _registry.IsRegistered("HelloAbpWorkflow", 1);
            ret.ShouldBeTrue();
        }
    }
}