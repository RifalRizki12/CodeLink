using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_experiences")]
    public class Experience
    {
        [Column ("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column ("position", TypeName = "nvarchar(100)")]
        public string Position { get; set; }
        [Column ("company")]
        public string Company { get; set; }
        [Column ("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        //kardinalitas
        public Employee? Employee { get; set; }
        public ICollection<ExperienceSkill>? ExperienceSkills { get; set;}
    }
}
