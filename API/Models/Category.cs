using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_categories")]
    public class Category : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        public ICollection<Skill>? Skills { get; set;}
    }
}
