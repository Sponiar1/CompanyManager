namespace Company.Mappers
{
    public class DepartmentDTO
    {
        public int Id_Department { get; set; }
        public string Dep_Name { get; set; }
        public string Code { get; set; }
        public int Id_Project { get; set; }
        public int BossId { get; set; }
    }
}
