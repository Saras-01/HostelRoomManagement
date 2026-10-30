namespace HostelRoomManagement.Models;

public class AuditLog
{
    public int AuditId { get; set; }
public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string ChangedBy { get; set; } = "System";

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
