using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories.Interfaces;
using HostelRoomManagement.Services.Interfaces;

namespace HostelRoomManagement.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int roomId)
    {
        if (roomId <= 0)
        {
            throw new ArgumentException("Room ID must be greater than zero.");
        }

        return await _roomRepository.GetByIdAsync(roomId);
    }

    public async Task AddRoomAsync(Room room)
    {
        ValidateRoom(room);

        if (room.OccupiedBeds != 0)
        {
            throw new ArgumentException(
                "A new room must have zero occupied beds. Allocate students separately.");
        }

        await _roomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(Room room)
    {
        ValidateRoom(room);

        if (room.RoomId <= 0)
        {
            throw new ArgumentException("Room ID must be greater than zero.");
        }

        var existingRoom = await _roomRepository.GetByIdAsync(room.RoomId);

        if (existingRoom is null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        if (room.Capacity < existingRoom.OccupiedBeds)
        {
            throw new InvalidOperationException(
                "Capacity cannot be less than the number of occupied beds.");
        }

        room.OccupiedBeds = existingRoom.OccupiedBeds;

        await _roomRepository.UpdateAsync(room);
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        if (roomId <= 0)
        {
            throw new ArgumentException("Room ID must be greater than zero.");
        }

        var room = await _roomRepository.GetByIdAsync(roomId);

        if (room is null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        if (room.OccupiedBeds > 0)
        {
            throw new InvalidOperationException(
                "Cannot delete a room that has occupied beds.");
        }

        await _roomRepository.DeleteAsync(roomId);
    }

    private static void ValidateRoom(Room room)
    {
        ArgumentNullException.ThrowIfNull(room);

        if (string.IsNullOrWhiteSpace(room.RoomNumber))
        {
            throw new ArgumentException("Room number is required.");
        }

        if (room.Capacity <= 0)
        {
            throw new ArgumentException("Room capacity must be greater than zero.");
        }

        if (room.OccupiedBeds < 0 || room.OccupiedBeds > room.Capacity)
        {
            throw new ArgumentException(
                "Occupied beds must be between zero and room capacity.");
        }
    }
}
