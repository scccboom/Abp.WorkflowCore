using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedExecutionPointerConfiguration : IEntityTypeConfiguration<PersistedExecutionPointer>
    {
        public void Configure(EntityTypeBuilder<PersistedExecutionPointer> builder)
        {
            builder.ToTable("WorkflowExecutionPointers");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.EventName).HasMaxLength(100);
            builder.Property(x => x.EventKey).HasMaxLength(100);
            builder.Property(x => x.StepName).HasMaxLength(100);
            builder.Property(x => x.PredecessorId).HasMaxLength(64);
            builder.HasOne(x => x.Workflow).WithMany(x => x.ExecutionPointers).HasForeignKey(x => x.WorkflowId);
        }
    }
}
