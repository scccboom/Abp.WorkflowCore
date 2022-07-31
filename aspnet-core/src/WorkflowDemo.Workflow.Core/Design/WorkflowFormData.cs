using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowFormData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        public IEnumerable<object> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public IEnumerable<object> Rules { get; set; } = new List<object>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> Styles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }
    }
}