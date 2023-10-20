using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_skills")]
    public class Skill
    {
        [Column("hard")]
        public string Hard {  get; set; }

        [Column("soft")]
        public string Soft { get; set; }

        [Column("categories_guid")]
        public int? CategoryGuid { get; set; }

        public ICollection<ExperienceSkill>? ExperienceSkills { get; set; }
        public Category? Category { get; set; }
    }
}
