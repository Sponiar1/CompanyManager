using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Company { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string Com_Name { get; set; }

        [Required(ErrorMessage = "Company code is required.")]
        [StringLength(10, ErrorMessage = "Code cannot exceed 10 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Company manager is required.")]
        [ForeignKey("Boss")]
        public int Id_Boss { get; set; }
        public Employee? Boss { get; set; }

        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }
}
