using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_skills")]
    public class Skill : BaseEntity
    {
        [Column("hard", TypeName = "nvarchar(50)")]
        public string? Hard {  get; set; }

        [Column("soft", TypeName = "nvarchar(50)")]
        public string? Soft { get; set; }

        public ICollection<ExperienceSkill>? ExperienceSkills { get; set; }
    }
}
