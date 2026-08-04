using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET ALL: api/students
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }

    // 2. GET BY ID: api/students/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        return Ok(student);
    }

    // 3. POST (CREATE): api/students
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] Student newStudent)
    {
        _context.Students.Add(newStudent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
    }

    // 4. PUT (UPDATE): api/students/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        student.Name = updatedStudent.Name;
        student.Email = updatedStudent.Email;
        student.CGPA = updatedStudent.CGPA;

        await _context.SaveChangesAsync();
        return Ok(student);
    }

    // 5. DELETE: api/students/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}