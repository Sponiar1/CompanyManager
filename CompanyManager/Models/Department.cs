using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Department { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]

        public string Dep_Name { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(10, ErrorMessage = "Department cannot exceed 10 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Project is required.")]
        [ForeignKey("Project")]
        public int Id_Project { get; set; }
        public Project? Project { get; set; }

        [Required(ErrorMessage = "Department boss is required.")]
        [ForeignKey("Boss")]
        public int Id_Boss { get; set; }
        public Employee? Boss { get; set; }
    }
}
