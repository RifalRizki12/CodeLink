using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_skills")]
    public class Skill : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name {  get; set; }

        public ICollection<CurriculumVitae>? CurriculumVitaes { get; set; }
    }
}
