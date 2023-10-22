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

        [Column("phone_number", TypeName = "nvarchar(16)")]
        public string PhoneNumber { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }
        [Column("expired_date")]
        public DateTime? ExpiredDate { get; set; }

        [Column("company_id")]
        public Guid? CompanyGuid { get; set; }

        //kardinalitas
        public ICollection<ExperienceSkill> ExperienceSkills { get; set; }
        public Salary? Salary { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public Account? Account { get; set; }
        public ICollection<Interview>? Tests { get; set; }
        public Company? Company { get; set; }
        public ICollection<Company>? Companies { get; set; }
        
    }
}
