using System.ComponentModel.DataAnnotations;

namespace StudentApi.Models;

public class Student
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = string.Empty;

    [Range(0.0, 4.0, ErrorMessage = "CGPA must be between 0.00 and 4.00.")]
    public double CGPA { get; set; }
}