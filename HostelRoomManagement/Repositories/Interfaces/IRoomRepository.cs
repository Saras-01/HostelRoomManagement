using HostelRoomManagement.Models;

namespace HostelRoomManagement.Repositories.Interfaces;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();

    Task<Room?> GetByIdAsync(int roomId);

    Task AddAsync(Room room);

    Task UpdateAsync(Room room);

    Task DeleteAsync(int roomId);
}