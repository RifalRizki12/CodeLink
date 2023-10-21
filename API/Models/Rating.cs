using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_ratings")]
    public class Rating : BaseEntity
    {
        [Column("rating", TypeName = "int(1)")]
        public int Rate { get; set; }

        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        //kardinalitas
        public Employee? Employee { get; set; }
    }
}
