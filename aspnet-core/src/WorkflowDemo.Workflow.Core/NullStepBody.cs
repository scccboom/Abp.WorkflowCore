using Abp.Dependency;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowDemo.Workflow
{
    public class NullStepBody : StepBody, ITransientDependency
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
}