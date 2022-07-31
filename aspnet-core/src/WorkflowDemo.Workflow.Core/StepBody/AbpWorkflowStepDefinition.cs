using System;

namespace WorkflowDemo.Workflows
{
    public class AbpWorkflowStepDefinition
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public WorkflowParamDictionary Inputs { get; set; } = new WorkflowParamDictionary();

        public Type StepBodyType { get; set; }
    }
}