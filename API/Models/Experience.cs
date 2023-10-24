using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_experiences")]
    public class Experience : BaseEntity
    {
        [Column ("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column ("position", TypeName = "nvarchar(50)")]
        public string Position { get; set; }
        [Column ("company", TypeName = "nvarchar(50)")]
        public string Company { get; set; }
        [Column ("employee_guid")]

        //kardinalitas
        public ICollection<CurriculumVitae>? CurriculumVitaes { get; set;}
    }
}
