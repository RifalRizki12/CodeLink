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

        [Column("phone_number", TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }

        //kardinalitas
        public ICollection<Experience>? Bookings { get; set; }
        public Salary? Salary { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public Account? Account { get; set; }
        public ICollection<Test>? Tests { get; set; }
    }
}
