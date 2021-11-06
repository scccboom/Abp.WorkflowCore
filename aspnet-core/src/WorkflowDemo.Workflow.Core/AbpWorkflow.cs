﻿using Abp.Dependency;

using WorkflowCore.Interface;

namespace WorkflowDemo.Workflow
{
    public abstract class AbpWorkflow : IAbpWorkflow, ISingletonDependency
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public abstract void Build(IWorkflowBuilder<WorkflowParamDictionary> builder);
    }
}