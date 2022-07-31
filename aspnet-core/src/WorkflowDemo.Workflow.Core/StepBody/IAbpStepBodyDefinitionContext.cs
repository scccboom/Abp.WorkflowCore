namespace WorkflowDemo.Workflows
{
    public interface IAbpStepBodyDefinitionContext
    {
        void Create(AbpWorkflowStepDefinition entity);
    }
}