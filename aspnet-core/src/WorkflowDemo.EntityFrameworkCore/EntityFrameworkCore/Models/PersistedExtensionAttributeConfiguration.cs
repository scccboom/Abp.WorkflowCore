using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore.Models
{
    internal class PersistedExtensionAttributeConfiguration : IEntityTypeConfiguration<PersistedExtensionAttribute>
    {
        public void Configure(EntityTypeBuilder<PersistedExtensionAttribute> builder)
        {
            builder.ToTable("WorkflowExtensionAttributes");
            builder.Property(x => x.Id).HasMaxLength(64);
            builder.Property(x => x.AttributeKey).HasMaxLength(100);
            builder.HasOne(x => x.ExecutionPointer).WithMany(x => x.ExtensionAttributes).HasForeignKey(x => x.ExecutionPointerId);
        }
    }
}
