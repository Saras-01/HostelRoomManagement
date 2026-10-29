using HostelRoomManagement.Models;

namespace HostelRoomManagement.Services.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllStudentsAsync();
Task<Student?> GetStudentByIdAsync(int studentId);

    Task AddStudentAsync(Student student);

    Task UpdateStudentAsync(Student student);

    Task DeleteStudentAsync(int studentId);

    Task AllocateRoomAsync(int studentId, int roomId);

    Task VacateRoomAsync(int studentId);
}
