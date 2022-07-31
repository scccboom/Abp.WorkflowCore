using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class PublishEventInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string  EventKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string  EventName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object EventData { get; set; }
    }
}
