using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories.Interfaces;
using HostelRoomManagement.Services.Interfaces;

namespace HostelRoomManagement.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IRoomRepository _roomRepository;
public StudentService(
    IStudentRepository studentRepository,
    IRoomRepository roomRepository)
    {
        _studentRepository = studentRepository;
        _roomRepository = roomRepository;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException(
                "Student ID must be greater than zero.");
        }

        return await _studentRepository.GetByIdAsync(studentId);
    }

    public async Task AddStudentAsync(Student student)
    {
        ValidateStudent(student);

        var existingStudents = await _studentRepository.GetAllAsync();

        if (existingStudents.Any(s =>
                s.RegistrationNumber.Equals(
                    student.RegistrationNumber,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "A student with this registration number already exists.");
        }

        student.StudentId = 0;
        student.RoomId = null;
        student.Room = null;

        await _studentRepository.AddAsync(student);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        ValidateStudent(student);

        if (student.StudentId <= 0)
        {
            throw new ArgumentException(
                "Student ID must be greater than zero.");
        }

        var existingStudent =
            await _studentRepository.GetByIdAsync(student.StudentId);

        if (existingStudent is null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        var allStudents = await _studentRepository.GetAllAsync();

        if (allStudents.Any(s =>
                s.StudentId != student.StudentId &&
                s.RegistrationNumber.Equals(
                    student.RegistrationNumber,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Another student already uses this registration number.");
        }

        existingStudent.FullName = student.FullName.Trim();
        existingStudent.RegistrationNumber =
            student.RegistrationNumber.Trim();
        existingStudent.PhoneNumber = student.PhoneNumber.Trim();

        await _studentRepository.UpdateAsync(existingStudent);
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        var student = await GetStudentByIdAsync(studentId);

        if (student is null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        if (student.RoomId.HasValue)
        {
            await VacateRoomAsync(studentId);
        }

        await _studentRepository.DeleteAsync(studentId);
    }

    public async Task AllocateRoomAsync(int studentId, int roomId)
    {
        var student = await GetStudentByIdAsync(studentId);

        if (student is null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        if (student.RoomId.HasValue)
        {
            throw new InvalidOperationException(
                "This student already has a room allocated.");
        }

        var room = await _roomRepository.GetByIdAsync(roomId);

        if (room is null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        if (room.OccupiedBeds >= room.Capacity)
        {
            throw new InvalidOperationException(
                "The room is full. No beds are available.");
        }

        student.RoomId = roomId;
        room.OccupiedBeds++;

        await _studentRepository.UpdateAsync(student);
        await _roomRepository.UpdateAsync(room);
    }

    public async Task VacateRoomAsync(int studentId)
    {
        var student = await GetStudentByIdAsync(studentId);

        if (student is null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        if (!student.RoomId.HasValue)
        {
            throw new InvalidOperationException(
                "This student does not have a room allocated.");
        }

        var room = await _roomRepository.GetByIdAsync(student.RoomId.Value);

        if (room is null)
        {
            throw new InvalidOperationException(
                "The student's allocated room could not be found.");
        }

        student.RoomId = null;
        room.OccupiedBeds = Math.Max(0, room.OccupiedBeds - 1);

        await _studentRepository.UpdateAsync(student);
        await _roomRepository.UpdateAsync(room);
    }

    private static void ValidateStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (string.IsNullOrWhiteSpace(student.FullName))
        {
            throw new ArgumentException("Student name is required.");
        }

        if (string.IsNullOrWhiteSpace(student.RegistrationNumber))
        {
            throw new ArgumentException(
                "Registration number is required.");
        }

        if (string.IsNullOrWhiteSpace(student.PhoneNumber))
        {
            throw new ArgumentException("Phone number is required.");
        }

        student.FullName = student.FullName.Trim();
        student.RegistrationNumber =
            student.RegistrationNumber.Trim();
        student.PhoneNumber = student.PhoneNumber.Trim();
    }
}
