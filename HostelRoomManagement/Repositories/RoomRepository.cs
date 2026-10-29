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

    // Retrieve all rooms
    public async Task<List<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .AsNoTracking()
            .ToListAsync();
    }

    // Retrieve a room by ID
    public async Task<Room?> GetByIdAsync(int roomId)
    {
        return await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoomId == roomId);
    }

    // Add a new room
    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    // Update an existing room safely
    public async Task UpdateAsync(Room room)
    {
        var existingRoom = await _context.Rooms
            .FindAsync(room.RoomId);

        if (existingRoom is null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        existingRoom.RoomNumber = room.RoomNumber;
        existingRoom.Capacity = room.Capacity;
        existingRoom.OccupiedBeds = room.OccupiedBeds;

        await _context.SaveChangesAsync();
    }

    // Delete a room
    public async Task DeleteAsync(int roomId)
    {
        var room = await _context.Rooms
            .FindAsync(roomId);

        if (room is null)
        {
            return;
        }

        _context.Rooms.Remove(room);

        await _context.SaveChangesAsync();
    }
}
