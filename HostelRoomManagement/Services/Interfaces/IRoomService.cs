using HostelRoomManagement.Models;

namespace HostelRoomManagement.Services.Interfaces;

public interface IRoomService
{
    Task<List<Room>> GetAllRoomsAsync();
Task<Room?> GetRoomByIdAsync(int roomId);

    Task AddRoomAsync(Room room);

    Task UpdateRoomAsync(Room room);

    Task DeleteRoomAsync(int roomId);
}

