using CompanyManager.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Mappers
{
    public class DivisionDTO
    {
        public string Div_Name { get; set; }

        public string Code { get; set; }

        public int Id_Company { get; set; }

        public int Id_Boss { get; set; }
    }
}
