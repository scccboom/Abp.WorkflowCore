using Abp.Dependency;

using Microsoft.Extensions.Logging;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class NullStepBody : StepBody, ITransientDependency
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public NullStepBody(ILogger<NullStepBody> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogDebug($"Start workflow: ${context.Workflow.Id}");
            return ExecutionResult.Next();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AbpWorkflowStepDefinition Create()
        {
            return new AbpWorkflowStepDefinition()
            {
                Name = "null",
                StepBodyType = typeof(NullStepBody)
            };
        }
    }
}