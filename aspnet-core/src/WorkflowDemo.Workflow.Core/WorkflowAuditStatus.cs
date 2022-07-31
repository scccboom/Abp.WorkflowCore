using System.ComponentModel;

namespace WorkflowDemo.Workflows
{
    public enum WorkflowAuditStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("未审核")]
        UnAudited = 0,
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Pass = 1,
        /// <summary>
        /// 审核未通过
        /// </summary>
        [Description("审核未通过")]
        Unapprove = 2,
    }
}
