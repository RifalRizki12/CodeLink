using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_experiences")]
    public class Experience
    {
        [Column ("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column ("position", TypeName = "nvarchar(50)")]
        public string Position { get; set; }
        [Column ("company", TypeName = "nvarchar(50)")]
        public string Company { get; set; }
        [Column ("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        //kardinalitas
        public ICollection<ExperienceSkill>? ExperienceSkills { get; set;}
    }
}
