using System.Data;
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
        private IDbContextTransaction _currentTransaction;
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditLevel> AuditLevels { get; set; }
        public DbSet<AuditType> AuditTypes { get; set; }
        public DbSet<ClientRequest> ClientRequests { get; set; }

        public DbSet<AuditApplication> AuditApplications { get; set; }


        public AuditContext(DbContextOptions<AuditContext> options)
            :base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
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