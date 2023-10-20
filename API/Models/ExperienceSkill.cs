using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_experiences_skills")]
    public class ExperienceSkill : BaseEntity
    {
        [Column("skills_guid")]
        public Guid? SkillGuid { get; set;}

        [Column("experiences_guid")]
        public Guid? ExperienceGuid { get; set;}
        
        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set;}

        //kardinalitas
        public Experience? Experience { get; set;}
        public Skill? Skill { get; set;}
        public Employee? Employee { get; set;}
    }
}
