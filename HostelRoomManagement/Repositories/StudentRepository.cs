using Microsoft.EntityFrameworkCore;
using HostelRoomManagement.Data;
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories.Interfaces;

namespace HostelRoomManagement.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly HostelDbContext _context;
public StudentRepository(HostelDbContext context)
    {
        _context = context;
    }

    // Retrieve all students
    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .ToListAsync();
    }

    // Retrieve a student by ID
    public async Task<Student?> GetByIdAsync(int studentId)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == studentId);
    }

    // Add a new student
    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    // Update an existing student safely
    public async Task UpdateAsync(Student student)
    {
        var existingStudent = await _context.Students
            .FindAsync(student.StudentId);

        if (existingStudent is null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        existingStudent.FullName = student.FullName;
        existingStudent.RegistrationNumber = student.RegistrationNumber;
        existingStudent.PhoneNumber = student.PhoneNumber;
        existingStudent.RoomId = student.RoomId;

        await _context.SaveChangesAsync();
    }

    // Delete a student
    public async Task DeleteAsync(int studentId)
    {
        var student = await _context.Students
            .FindAsync(studentId);

        if (student is null)
        {
            return;
        }

        _context.Students.Remove(student);

        await _context.SaveChangesAsync();
    }
}
