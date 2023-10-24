using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_ratings")]
    public class Rating : BaseEntity
    {
        [Column("rate")]
        public int? Rate { get; set; }

        [Column("feedback")]
        public string? Feedback { get; set; }

        //kardinalitas
        public Interview? Interview { get; set; }
    }
}
