using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_tr_cv")]
    public class CurriculumVitae : BaseEntity
    {
        [Column("skills_guid")]
        public Guid? SkillGuid { get; set;}

        [Column("experiences_guid")]
        public Guid? ExperienceGuid { get; set;}

        //kardinalitas
        public Experience? Experience { get; set;}
        public Skill? Skill { get; set;}
        public Employee? Employee { get; set;}
    }
}
