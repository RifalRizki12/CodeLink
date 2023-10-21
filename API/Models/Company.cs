using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_companies")]
    public class Company : BaseEntity
    {
        [Column("name", TypeName ="nvarchar(100)")]
        public string Name { get; set; }

        [Column("address", TypeName ="nvarchar(100)")]
        public string Address { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("employee_guid")]
        public Guid? EmployeeGuid { get; set; }

        public Employee? Employee { get; set; }
        public ICollection<Employee>? Employees { get; set; }

    }
}
