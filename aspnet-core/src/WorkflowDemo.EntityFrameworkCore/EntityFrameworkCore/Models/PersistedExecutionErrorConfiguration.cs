using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedExecutionErrorConfiguration : IEntityTypeConfiguration<PersistedExecutionError>
    {
        public void Configure(EntityTypeBuilder<PersistedExecutionError> builder)
        {
            builder.ToTable("WorkflowExecutionErrors");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.WorkflowId).HasMaxLength(64);
            builder.Property(x => x.ExecutionPointerId).HasMaxLength(64);
        }
    }
}
