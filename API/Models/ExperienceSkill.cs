using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_experiences_skills")]
    public class ExperienceSkill : BaseEntity
    {
        [Column("skills_guid")]
        public int? SkillId { get; set;}

        [Column("experiences_guid")]
        public int? ExperienceGuid { get; set;}

        //kardinalitas
        public Experience? Experience { get; set;}
        public Skill? Skill { get; set;}
    }
}
