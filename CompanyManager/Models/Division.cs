using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Division
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Division { get; set; }

        [Required(ErrorMessage = "Division name is required.")]
        [StringLength(100, ErrorMessage = "Division name cannot exceed 100 characters.")]
        public string Div_Name { get; set; }

        [Required(ErrorMessage = "Company code is required.")]
        [StringLength(10, ErrorMessage = "Code cannot exceed 10 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [ForeignKey("Company")]
        public int Id_Company { get; set; }
        public Company? Company { get; set; }

        [Required(ErrorMessage = "Division boss is required.")]
        [ForeignKey("Boss")]
        public int Id_Boss { get; set; }
        public Employee? Boss { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
