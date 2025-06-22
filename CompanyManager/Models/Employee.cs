using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Employee { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, ErrorMessage = "First name cannot exceed 30 characters.")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, ErrorMessage = "Last name cannot exceed 30 characters.")]
        public string Last_Name { get; set; }

        [StringLength(6, ErrorMessage = "Title cannot exceed 6 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(12, ErrorMessage = "Phone number is too long.")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "Phone number must be between 9 and 12 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email is too long.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public ICollection<Division> Divisions { get; set; } = new List<Division>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
