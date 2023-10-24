using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee : BaseEntity
    {

        [Column("first_name", TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Column("last_name", TypeName = "nvarchar(100)")]
        public string? LastName { get; set; }

        [Column("gender")]
        public GenderLevel Gender { get; set; }

        [Column("email", TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column("foto")]
        public string? Foto { get; set; }

        [Column("phone_number", TypeName = "nvarchar(16)")]
        public string PhoneNumber { get; set; }

        [Column("status")]
        public StatusEmployee StatusEmployee { get; set; }

        [Column("grade")]
        public GradeLevel? Grade { get; set; }

        [Column("hire_metrodata")]
        public DateTime? HireMetro { get; set; }
        [Column("end_metrodata")]
        public DateTime? EndMetro { get; set; }

        [Column("company_id")]
        public Guid? CompanyGuid { get; set; }

        //kardinalitas
        public CurriculumVitae? CurriculumVitae { get; set; }
        public Account? Account { get; set; }
        public ICollection<Interview>? Tests { get; set; }
        public Company? Company { get; set; }
        public ICollection<Company>? Companies { get; set; }
        
    }
}
