using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditService.Infrastructure.Idempotency;
using AuditService.Model.Admin;
using AuditService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuditService.Data
{

    public class AuditContext : DbContext 
    {
        private readonly Guid _tenantId;

        private IDbContextTransaction _currentTransaction;
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditLevel> AuditLevels { get; set; }
        public DbSet<AuditType> AuditTypes { get; set; }
        public DbSet<ClientRequest> ClientRequests { get; set; }

        public DbSet<AuditApplication> AuditApplications { get; set; }

        public AuditContext(DbContextOptions<AuditContext> options, Guid tenantId)
            : base(options)
        {
            _tenantId = tenantId;
        }

        public AuditContext(DbContextOptions<AuditContext> options)
            :base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>().Property<Guid>("_tenantId").HasColumnName("TenantId");
            modelBuilder.Entity<Audit>().HasQueryFilter(b => EF.Property<Guid>(b, "_tenantId") == _tenantId);

            modelBuilder.Entity<AuditApplication>().Property<Guid>("_tenantId").HasColumnName("TenantId");
            modelBuilder.Entity<AuditApplication>().HasQueryFilter(b => EF.Property<Guid>(b, "_tenantId") == _tenantId);

            base.OnModelCreating(modelBuilder);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).DateModified = DateTime.Now;

                if (entityEntry.State != EntityState.Added) continue;

                ((BaseEntity)entityEntry.Entity).DateCreated = DateTime.Now;

                if (entityEntry.Metadata.GetProperties().Any(p => p.Name == "_tenantId"))
                    entityEntry.CurrentValues["_tenantId"] = _tenantId;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}