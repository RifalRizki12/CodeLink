using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_tr_cv")]
    public class CurriculumVitae : BaseEntity
    {
        [Column("cv")]
        public string? Cv {  get; set; }

        //kardinalitas
        public ICollection<Skill>? Skills { get; set;}
        public Employee? Employee { get; set;}
    }
}
