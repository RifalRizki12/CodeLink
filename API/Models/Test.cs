using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_test")]
    public class Test : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("employee_guid")]
        public int EmployeeGuid { get; set; }

        //kardinalitas
        public Employee? Employee { get; set; }
    }
}
