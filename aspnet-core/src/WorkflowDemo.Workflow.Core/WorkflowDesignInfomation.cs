﻿using System.Collections.Generic;

namespace WorkflowDemo.Workflow
{
    public class AbpStepBodyInput
    {
        public Dictionary<string, WorkflowParamInput> Inputs { get; set; } = new Dictionary<string, WorkflowParamInput>();

        public string Name { get; set; }
    }

    public class WorkflowConditionCondition
    {
        public string Field { get; set; }

        public string Operator { get; set; }

        public object Value { get; set; }
    }

    public class WorkflowConditionNode
    {
        public IEnumerable<WorkflowConditionCondition> Conditions { get; set; } = new List<WorkflowConditionCondition>();

        public string Label { get; set; }

        public string NodeId { get; set; }
    }

    public class WorkflowFormData
    {
        public string Id { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        public IEnumerable<object> Items { get; set; }

        public string Label { get; set; }

        public int? MaxLength { get; set; }

        public int? MinLength { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public IEnumerable<object> Rules { get; set; } = new List<object>();

        public IEnumerable<object> Styles { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }
    }

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

    public class WorkflowParamInput
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}