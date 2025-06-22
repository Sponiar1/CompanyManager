using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Project { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public string Pro_Name { get; set; }

        [Required(ErrorMessage = "Project code is required.")]
        [StringLength(10, ErrorMessage = "Project cannot exceed 10 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Division is required.")]
        [ForeignKey("Division")]
        public int Id_Division { get; set; }
        public Division? Division { get; set; }

        [Required(ErrorMessage = "Project boss is required.")]
        [ForeignKey("Boss")]
        public int BossId { get; set; }
        public Employee? Boss { get; set; }

        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}
