using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_ratings")]
    public class Rating : BaseEntity
    {
        [Column("rating")]
        public int Rate { get; set; }

        [Column("employee_guid")]
        public int? EmployeeGuid { get; set; }

        public Employee? Employee { get; set; }
    }
}
