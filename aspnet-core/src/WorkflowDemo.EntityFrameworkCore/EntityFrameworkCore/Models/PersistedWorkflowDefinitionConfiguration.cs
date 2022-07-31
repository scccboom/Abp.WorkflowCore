using System;
using System.Collections.Generic;
using System.Text;

using Abp.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedWorkflowDefinitionConfiguration : IEntityTypeConfiguration<PersistedWorkflowDefinition>
    {
        public void Configure(EntityTypeBuilder<PersistedWorkflowDefinition> builder)
        {
            builder.ToTable("WorkflowDefinitions");
            builder.HasKey(x => new { x.Id, x.Version });
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.Version).HasDefaultValue(1);
            builder.Property(x => x.Title).HasMaxLength(256);
            builder.Property(x => x.Group).HasMaxLength(100);
            builder.Property(x => x.Icon).HasMaxLength(50);
            builder.Property(x => x.Color).HasMaxLength(50);
            builder.Property(x => x.Inputs)
                .HasConversion(x => x.ToJsonString(false, false), x => x.FromJsonString<IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>>>());
            builder.Property(x => x.Nodes)
                .HasConversion(x => x.ToJsonString(false, false), x => x.FromJsonString<IEnumerable<WorkflowNode>>());
        }
    }
}
