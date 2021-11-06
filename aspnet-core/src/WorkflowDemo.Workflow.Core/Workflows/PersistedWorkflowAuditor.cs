﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkflowDemo.Workflow
{
    public class PersistedWorkflowAuditor : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        [ForeignKey("ExecutionPointerId")]
        public PersistedExecutionPointer ExecutionPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExecutionPointerId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EnumAuditStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UserHeadPhoto { get; set; }

        /// <summary>
        /// 审核人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string UserIdentityName { get; set; }

        [ForeignKey("WorkflowId")]
        public PersistedWorkflow Workflow { get; set; }

        public Guid WorkflowId { get; set; }
    }

    public class PersistedWorkflowAuditorEntityTypeConfiguration : IEntityTypeConfiguration<PersistedWorkflowAuditor>
    {
        public void Configure(EntityTypeBuilder<PersistedWorkflowAuditor> builder)
        {
            builder.ToTable("WorkflowAuditors");
            builder.Property(u => u.Remark).HasMaxLength(500);
        }
    }
}
