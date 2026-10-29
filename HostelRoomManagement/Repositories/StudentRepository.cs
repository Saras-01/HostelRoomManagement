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

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int studentId)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(student => student.StudentId == studentId);
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);

        if (student is null)
        {
            return;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }
}
