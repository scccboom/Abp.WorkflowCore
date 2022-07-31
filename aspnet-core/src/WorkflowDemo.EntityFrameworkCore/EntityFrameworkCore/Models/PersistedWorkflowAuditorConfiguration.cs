using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedWorkflowAuditorConfiguration : IEntityTypeConfiguration<PersistedWorkflowAuditor>
    {
        public void Configure(EntityTypeBuilder<PersistedWorkflowAuditor> builder)
        {
            builder.ToTable("WorkflowAuditors");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(u => u.Remark).HasMaxLength(500);
            builder.HasOne(x => x.Workflow).WithMany().HasForeignKey(x => x.WorkflowId);
            builder.HasOne(x => x.ExecutionPointer).WithMany().HasForeignKey(x => x.ExecutionPointerId);
        }
    }
}
