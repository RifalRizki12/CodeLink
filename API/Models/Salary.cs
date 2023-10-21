using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Salary : BaseEntity
    {
        [Column("basic_salary")]
        public int BasicSalary {  get; set; }

        [Column("overtime_pay")]
        public int? OvertimePay {  get; set; }

        public Employee? Employee { get; set; }
    }
}
