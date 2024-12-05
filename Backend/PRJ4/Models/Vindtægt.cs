using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models
{
    public partial class Vindtægt
    {
        [Key]
        public int VindtægtId { get; set; }

        public string BrugerId { get; set; }
        public string Tekst { get; set; }
        public decimal? Indtægt { get; set; }
        public DateTime? Dato { get; set; }

        public int? KategoriId { get; set; }

        [ForeignKey(nameof(KategoriId))]
        public virtual Kategori? Kategori { get; set; } = null!;

        public virtual ApiUser Bruger { get; set; } = null!;
    }
}