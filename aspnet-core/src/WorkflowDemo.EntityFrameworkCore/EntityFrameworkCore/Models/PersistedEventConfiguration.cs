using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedEventConfiguration : IEntityTypeConfiguration<PersistedEvent>
    {
        public void Configure(EntityTypeBuilder<PersistedEvent> builder)
        {
            builder.ToTable("WorkflowEvents");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.EventName).HasMaxLength(100);
            builder.Property(x => x.EventKey).HasMaxLength(100);
        }
    }
}
