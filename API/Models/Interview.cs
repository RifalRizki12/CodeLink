using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_interviews")]
    public class Interview : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("start_contract")]
        public DateTime? StartContract {  get; set; }
        
        [Column("end_contract")]
        public DateTime? EndContract {  get; set; }

        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        [Column("owner_guid")]
        public Guid OwnerGuid { get; set; }

        //kardinalitas
        public Employee? Employee { get; set; }
        public Rating? Rating { get; set; }
    }
}
