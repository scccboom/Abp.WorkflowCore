using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class StartWorkflowInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();
    }
}
