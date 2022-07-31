using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedWorkflowConfiguration : IEntityTypeConfiguration<PersistedWorkflow>
    {
        public void Configure(EntityTypeBuilder<PersistedWorkflow> builder)
        {
            builder.ToTable("Workflows");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Reference).HasMaxLength(64);
            builder.HasOne(x => x.WorkflowDefinition).WithMany().HasForeignKey(u => new { u.WorkflowDefinitionId, u.Version });
            builder.HasMany(x => x.ExecutionPointers).WithOne(x => x.Workflow);
        }
    }
}
