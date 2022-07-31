using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities;

using WorkflowCore.Models;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedExecutionPointer : Entity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersistedWorkflow Workflow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? SleepUntil { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PersistenceData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EventPublished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PersistedExtensionAttribute> ExtensionAttributes { get; set; } = new List<PersistedExtensionAttribute>();

        /// <summary>
        /// 
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Children { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContextItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PredecessorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Outcome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PointerStatus Status { get; set; } = PointerStatus.Legacy;

        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; set; }
    }
}
