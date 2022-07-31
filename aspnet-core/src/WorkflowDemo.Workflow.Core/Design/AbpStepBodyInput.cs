using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    public class AbpStepBodyInput
    {
        public Dictionary<string, WorkflowParamInput> Inputs { get; set; } = new Dictionary<string, WorkflowParamInput>();

        public string Name { get; set; }
    }
}