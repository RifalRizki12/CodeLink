using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class BaseEntity
    {
        [Key, Column("guid")] // Menentukan atribut sebagai kunci utama dan kolom "guid".
        public Guid Guid { get; set; } // Properti yang menyimpan GUID unik untuk entitas.

        [Column("created_date")] // Menentukan kolom "created_date".
        public DateTime? CreatedDate { get; set; } // Properti yang menyimpan tanggal dan waktu pembuatan entitas.

        [Column("modified_date")] // Menentukan kolom "modified_date".
        public DateTime? ModifiedDate { get; set; } // Properti yang menyimpan tanggal dan waktu terakhir entitas diubah.
    }
}
