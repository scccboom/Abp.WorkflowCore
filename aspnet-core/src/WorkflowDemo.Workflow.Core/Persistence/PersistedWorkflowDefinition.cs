using System.Collections.Generic;

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace WorkflowDemo.Workflows
{
    public class PersistedWorkflowDefinition : FullAuditedEntity<string>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Title { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        public string Group { get; set; }

        /// <summary>
        /// 输入
        /// </summary>
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }

        /// <summary>
        /// 流程节点
        /// </summary>
        public IEnumerable<WorkflowNode> Nodes { get; set; }
    }
}