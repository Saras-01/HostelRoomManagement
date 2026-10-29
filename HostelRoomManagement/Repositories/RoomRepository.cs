using Microsoft.EntityFrameworkCore;
using HostelRoomManagement.Data;
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories.Interfaces;

namespace HostelRoomManagement.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HostelDbContext _context;

    public RoomRepository(HostelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(int roomId)
    {
        return await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(room => room.RoomId == roomId);
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);

        if (room is null)
        {
            return;
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}