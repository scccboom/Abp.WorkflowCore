using Abp.UI.Inputs;

namespace WorkflowDemo.Workflow
{
    public class WorkflowParam
    {
        public string DisplayName { get; set; }

        public IInputType InputType { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}
