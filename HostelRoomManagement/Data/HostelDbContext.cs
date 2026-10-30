using Microsoft.EntityFrameworkCore;
using HostelRoomManagement.Models;

namespace HostelRoomManagement.Data;

public class HostelDbContext : DbContext
{
    public HostelDbContext(DbContextOptions<HostelDbContext> options)
    : base(options)
    {
    }
public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Student> Students => Set<Student>();
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>()
            .HasMany(room => room.Students)
            .WithOne(student => student.Room)
            .HasForeignKey(student => student.RoomId)
            .IsRequired(false);
    }

}

