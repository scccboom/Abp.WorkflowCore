using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Attachments.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class FormFileWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public IFormFile FormFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }
    }
}
