using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    public class WorkflowNode
    {
        public string Key { get; set; }

        public IEnumerable<WorkflowConditionNode> NextNodes { get; set; }

        public IEnumerable<string> ParentNodes { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public int[] Position { get; set; }

        public AbpStepBodyInput StepBody { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 类型[left,top]
        /// </summary>
        public string Type { get; set; }
    }
}