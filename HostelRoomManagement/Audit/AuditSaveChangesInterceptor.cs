using System.Collections.Concurrent;
using System.Text.Json;
using HostelRoomManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HostelRoomManagement.Audit;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ConcurrentDictionary<Guid, List<PendingAudit>>
    _pendingAudits = new();
private sealed record PendingAudit(
    EntityEntry Entry,
    string EntityName,
    string Action,
    string? OldValues,
    string? NewValues);

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var audits = new List<PendingAudit>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not (Room or Student))
                continue;

            if (entry.State == EntityState.Added)
            {
                audits.Add(new PendingAudit(
                    entry,
                    entry.Metadata.ClrType.Name,
                    "Created",
                    null,
                    JsonSerializer.Serialize(
                        entry.CurrentValues.ToObject())));
            }
            else if (entry.State == EntityState.Modified)
            {
                audits.Add(new PendingAudit(
                    entry,
                    entry.Metadata.ClrType.Name,
                    "Updated",
                    JsonSerializer.Serialize(
                        entry.OriginalValues.ToObject()),
                    JsonSerializer.Serialize(
                        entry.CurrentValues.ToObject())));
            }
            else if (entry.State == EntityState.Deleted)
            {
                audits.Add(new PendingAudit(
                    entry,
                    entry.Metadata.ClrType.Name,
                    "Deleted",
                    JsonSerializer.Serialize(
                        entry.OriginalValues.ToObject()),
                    null));
            }
        }

        if (audits.Count > 0)
        {
            _pendingAudits[context.ContextId.InstanceId] = audits;
        }

        return base.SavingChangesAsync(
            eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null ||
            !_pendingAudits.TryRemove(
                context.ContextId.InstanceId, out var audits))
        {
            return await base.SavedChangesAsync(
                eventData, result, cancellationToken);
        }

        var auditLogs = new List<AuditLog>();

        foreach (var audit in audits)
        {
            var primaryKey = audit.Entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey());

            var entityId = primaryKey?.CurrentValue is null
                ? 0
                : Convert.ToInt32(primaryKey.CurrentValue);

            auditLogs.Add(new AuditLog
            {
                EntityName = audit.EntityName,
                EntityId = entityId,
                Action = audit.Action,
                OldValues = audit.OldValues,
                NewValues = audit.NewValues,
                ChangedBy = "System",
                ChangedAt = DateTime.UtcNow
            });
        }

        context.Set<AuditLog>().AddRange(auditLogs);

        // Save audit records separately.
        // AuditLog entities are excluded from interception.
        await context.SaveChangesAsync(cancellationToken);

        return await base.SavedChangesAsync(
            eventData, result, cancellationToken);
    }
}
