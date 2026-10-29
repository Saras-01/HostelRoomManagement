using HostelRoomManagement.Models;

namespace HostelRoomManagement.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();

Task<Student?> GetByIdAsync(int studentId);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task DeleteAsync(int studentId);

}
