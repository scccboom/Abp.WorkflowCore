using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    public class WorkflowConditionNode
    {
        public IEnumerable<WorkflowCondition> Conditions { get; set; } = new List<WorkflowCondition>();

        public string Label { get; set; }

        public string NodeId { get; set; }
    }
}