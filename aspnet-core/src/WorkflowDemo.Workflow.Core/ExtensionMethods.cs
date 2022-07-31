using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using WorkflowCore.Models;

namespace WorkflowDemo.Workflows
{
    internal static class ExtensionMethods
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings() 
        { 
            TypeNameHandling = TypeNameHandling.All 
        };

        internal static PersistedWorkflow ToPersistable(this WorkflowInstance instance, PersistedWorkflow workflow = null)
        {
            if (workflow == null)
            {
                workflow = new PersistedWorkflow();
            }
            workflow.Id = instance.Id;
            workflow.Data = JsonConvert.SerializeObject(instance.Data, SerializerSettings);
            workflow.Description = instance.Description;
            workflow.Reference = instance.Reference;
            workflow.NextExecution = instance.NextExecution;
            workflow.Version = instance.Version;
            workflow.WorkflowDefinitionId = instance.WorkflowDefinitionId;
            workflow.Status = instance.Status;
            workflow.CreateTime = instance.CreateTime;
            workflow.CompleteTime = instance.CompleteTime;

            foreach (var ep in instance.ExecutionPointers)
            {
                var pointer = workflow.ExecutionPointers.FindById(ep.Id);

                if (pointer == null)
                {
                    pointer = new PersistedExecutionPointer
                    {
                        Id = ep.Id ?? Guid.NewGuid().ToString()
                    };
                    workflow.ExecutionPointers.Add(pointer);
                }

                pointer.StepId = ep.StepId;
                pointer.Active = ep.Active;
                pointer.SleepUntil = ep.SleepUntil;
                pointer.PersistenceData = JsonConvert.SerializeObject(ep.PersistenceData, SerializerSettings);
                pointer.StartTime = ep.StartTime;
                pointer.EndTime = ep.EndTime;
                pointer.StepName = ep.StepName;
                pointer.RetryCount = ep.RetryCount;
                pointer.PredecessorId = ep.PredecessorId;
                pointer.ContextItem = JsonConvert.SerializeObject(ep.ContextItem, SerializerSettings);
                pointer.Children = string.Empty;

                foreach (var child in ep.Children)
                {
                    pointer.Children += child + ";";
                }

                pointer.EventName = ep.EventName;
                pointer.EventKey = ep.EventKey;
                pointer.EventPublished = ep.EventPublished;
                pointer.EventData = JsonConvert.SerializeObject(ep.EventData, SerializerSettings);
                pointer.Outcome = JsonConvert.SerializeObject(ep.Outcome, SerializerSettings);
                pointer.Status = ep.Status;

                pointer.Scope = string.Empty;
                foreach (var item in ep.Scope)
                {
                    pointer.Scope += item + ";";
                }

                foreach (var attr in ep.ExtensionAttributes)
                {
                    var extensionAttribute = pointer.ExtensionAttributes.FirstOrDefault(x => x.AttributeKey == attr.Key);
                    if (extensionAttribute == null)
                    {
                        extensionAttribute = new PersistedExtensionAttribute();
                        pointer.ExtensionAttributes.Add(extensionAttribute);
                    }

                    extensionAttribute.AttributeKey = attr.Key;
                    extensionAttribute.AttributeValue = JsonConvert.SerializeObject(attr.Value, SerializerSettings);
                }
            }

            return workflow;
        }

        internal static PersistedExecutionError ToPersistable(this ExecutionError instance)
        {
            var result = new PersistedExecutionError
            {
                Id = Guid.NewGuid().ToString(),
                ErrorTime = instance.ErrorTime,
                Message = instance.Message,
                ExecutionPointerId = instance.ExecutionPointerId,
                WorkflowId = instance.WorkflowId
            };

            return result;
        }

        internal static PersistedSubscription ToPersistable(this EventSubscription instance)
        {
            PersistedSubscription result = new PersistedSubscription
            {
                Id = instance.Id,
                EventKey = instance.EventKey,
                EventName = instance.EventName,
                StepId = instance.StepId,
                ExecutionPointerId = instance.ExecutionPointerId,
                WorkflowId = instance.WorkflowId,
                SubscribeAsOf = DateTime.SpecifyKind(instance.SubscribeAsOf, DateTimeKind.Utc),
                SubscriptionData = JsonConvert.SerializeObject(instance.SubscriptionData, SerializerSettings),
                ExternalToken = instance.ExternalToken,
                ExternalTokenExpiry = instance.ExternalTokenExpiry,
                ExternalWorkerId = instance.ExternalWorkerId
            };

            return result;
        }

        internal static PersistedEvent ToPersistable(this Event instance)
        {
            PersistedEvent result = new PersistedEvent
            {
                Id = instance.Id,
                EventKey = instance.EventKey,
                EventName = instance.EventName,
                EventTime = DateTime.SpecifyKind(instance.EventTime, DateTimeKind.Utc),
                IsProcessed = instance.IsProcessed,
                EventData = JsonConvert.SerializeObject(instance.EventData, SerializerSettings)
            };

            return result;
        }

        internal static WorkflowInstance ToWorkflowInstance(this PersistedWorkflow instance)
        {
            WorkflowInstance result = new WorkflowInstance
            {
                Data = JsonConvert.DeserializeObject(instance.Data, SerializerSettings),
                Description = instance.Description,
                Reference = instance.Reference,
                Id = instance.Id.ToString(),
                NextExecution = instance.NextExecution,
                Version = instance.Version,
                WorkflowDefinitionId = instance.WorkflowDefinitionId.ToString(),
                Status = instance.Status,
                CreateTime = DateTime.SpecifyKind(instance.CreateTime, DateTimeKind.Utc)
            };
            if (instance.CompleteTime.HasValue)
                result.CompleteTime = DateTime.SpecifyKind(instance.CompleteTime.Value, DateTimeKind.Utc);

            result.ExecutionPointers = new ExecutionPointerCollection(instance.ExecutionPointers.Count + 8);

            foreach (var ep in instance.ExecutionPointers)
            {
                var pointer = new ExecutionPointer
                {
                    Id = ep.Id,
                    StepId = ep.StepId,
                    Active = ep.Active
                };

                if (ep.SleepUntil.HasValue)
                    pointer.SleepUntil = DateTime.SpecifyKind(ep.SleepUntil.Value, DateTimeKind.Utc);

                pointer.PersistenceData = JsonConvert.DeserializeObject(ep.PersistenceData ?? string.Empty, SerializerSettings);

                if (ep.StartTime.HasValue)
                    pointer.StartTime = DateTime.SpecifyKind(ep.StartTime.Value, DateTimeKind.Utc);

                if (ep.EndTime.HasValue)
                    pointer.EndTime = DateTime.SpecifyKind(ep.EndTime.Value, DateTimeKind.Utc);

                pointer.StepName = ep.StepName;

                pointer.RetryCount = ep.RetryCount;
                pointer.PredecessorId = ep.PredecessorId;
                pointer.ContextItem = JsonConvert.DeserializeObject(ep.ContextItem ?? string.Empty, SerializerSettings);

                if (!string.IsNullOrEmpty(ep.Children))
                    pointer.Children = ep.Children.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                pointer.EventName = ep.EventName;
                pointer.EventKey = ep.EventKey;
                pointer.EventPublished = ep.EventPublished;
                pointer.EventData = JsonConvert.DeserializeObject(ep.EventData ?? string.Empty, SerializerSettings);
                pointer.Outcome = JsonConvert.DeserializeObject(ep.Outcome ?? string.Empty, SerializerSettings);
                pointer.Status = ep.Status;

                if (!string.IsNullOrEmpty(ep.Scope))
                    pointer.Scope = new List<string>(ep.Scope.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                foreach (var attr in ep.ExtensionAttributes)
                {
                    pointer.ExtensionAttributes[attr.AttributeKey] = JsonConvert.DeserializeObject(attr.AttributeValue, SerializerSettings);
                }

                result.ExecutionPointers.Add(pointer);
            }

            return result;
        }

        internal static EventSubscription ToEventSubscription(this PersistedSubscription instance)
        {
            EventSubscription result = new EventSubscription
            {
                Id = instance.Id.ToString(),
                EventKey = instance.EventKey,
                EventName = instance.EventName,
                StepId = instance.StepId,
                ExecutionPointerId = instance.ExecutionPointerId,
                WorkflowId = instance.WorkflowId,
                SubscribeAsOf = DateTime.SpecifyKind(instance.SubscribeAsOf, DateTimeKind.Utc),
                SubscriptionData = JsonConvert.DeserializeObject(instance.SubscriptionData, SerializerSettings),
                ExternalToken = instance.ExternalToken,
                ExternalTokenExpiry = instance.ExternalTokenExpiry,
                ExternalWorkerId = instance.ExternalWorkerId
            };

            return result;
        }

        internal static Event ToEvent(this PersistedEvent instance)
        {
            Event result = new Event
            {
                Id = instance.Id.ToString(),
                EventKey = instance.EventKey,
                EventName = instance.EventName,
                EventTime = DateTime.SpecifyKind(instance.EventTime, DateTimeKind.Utc),
                IsProcessed = instance.IsProcessed,
                EventData = JsonConvert.DeserializeObject(instance.EventData, SerializerSettings)
            };

            return result;
        }
    }
}