using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_skills")]
    public class Skill : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name {  get; set; }
        [Column("cv_guid")]
        public Guid CvGuid { get; set; }

        public CurriculumVitae? CurriculumVitae { get; set; }
    }
}
