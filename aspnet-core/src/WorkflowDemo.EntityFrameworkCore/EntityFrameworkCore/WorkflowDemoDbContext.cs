﻿using Abp.Domain.Repositories;
using Abp.Zero.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using WorkflowDemo.Authorization.Roles;
using WorkflowDemo.Authorization.Users;
using WorkflowDemo.EntityFrameworkCore.Repositories;
using WorkflowDemo.MultiTenancy;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.EntityFrameworkCore
{
    [AutoRepositoryTypes(
        typeof(IRepository<>),
        typeof(IRepository<,>),
        typeof(WorkflowDemoRepositoryBase<>),
        typeof(WorkflowDemoRepositoryBase<,>)
    )]
    public class WorkflowDemoDbContext : AbpZeroDbContext<Tenant, Role, User, WorkflowDemoDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<PersistedEvent> PersistedEvents { get; set; }

        public DbSet<PersistedExecutionError> PersistedExecutionErrors { get; set; }

        public DbSet<PersistedExecutionPointer> PersistedExecutionPointers { get; set; }

        public DbSet<PersistedExtensionAttribute> PersistedExtensionAttributes { get; set; }

        public DbSet<PersistedSubscription> PersistedSubscriptions { get; set; }

        public DbSet<PersistedWorkflow> PersistedWorkflows { get; set; }

        public DbSet<PersistedWorkflowDefinition> PersistedWorkflowDefinitions { get; set; }

        public DbSet<PersistedWorkflowAuditor> PersistedWorkflowAuditors { get; set; }

        public WorkflowDemoDbContext(DbContextOptions<WorkflowDemoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkflowDemoDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}