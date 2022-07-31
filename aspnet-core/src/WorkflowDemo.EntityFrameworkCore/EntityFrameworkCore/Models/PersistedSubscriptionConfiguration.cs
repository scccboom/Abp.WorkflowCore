using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedSubscriptionConfiguration : IEntityTypeConfiguration<PersistedSubscription>
    {
        public void Configure(EntityTypeBuilder<PersistedSubscription> builder)
        {
            builder.ToTable("WorkflowSubscriptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.WorkflowId).HasMaxLength(64);
            builder.Property(x => x.ExecutionPointerId).HasMaxLength(64);
            builder.Property(x => x.ExternalWorkerId).HasMaxLength(64);
            builder.Property(x => x.EventKey).HasMaxLength(100);
            builder.Property(x => x.EventName).HasMaxLength(100);
            builder.Property(x => x.ExternalToken).HasMaxLength(100);
        }
    }
}
