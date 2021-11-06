﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WorkflowCore.Interface;

namespace WorkflowDemo.Workflow
{
    public interface IAbpPersistenceProvider : IPersistenceProvider
    {
        Task<PersistedWorkflow> GetPersistedWorkflow(Guid id);

        Task<IEnumerable<PersistedWorkflow>> GetAllRunnablePersistedWorkflow(string definitionId, int version);

        Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id);

        Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version);
    }
}