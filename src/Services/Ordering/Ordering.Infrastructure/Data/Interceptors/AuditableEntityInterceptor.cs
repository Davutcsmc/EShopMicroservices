using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null)
                return;

            var entries = context.ChangeTracker.Entries<IEntity>();
            var now = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    // entry.Entity.CreatedBy = GetCurrentUserId(); // Implement user tracking as needed
                    entry.Entity.CreatedBy = "mehmet";
                }
                
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModified = now;
                    // entry.Entity.LastModifiedBy = GetCurrentUserId(); // Implement user tracking as needed
                    entry.Entity.LastModifiedBy = "mehmet";
                }
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => r.TargetEntry != null
        && r.TargetEntry.Metadata.IsOwned()
        && (r.TargetEntry.State == EntityState.Added ||
             r.TargetEntry.State == EntityState.Modified ||
             r.TargetEntry.State == EntityState.Deleted ));

}
