using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class Vudgifter
{
    [Key]
    public int VudgiftId { get; set; }
    public decimal Pris { get; set; }

    public string? Tekst { get; set; }
    public DateTime? Dato { get; set; }

    public int BrugerId { get; set; }

    public int? KategoriId { get; set; }
    [ForeignKey(nameof(KategoriId))]
    public Kategori Kategori {get; set;}
    [ForeignKey(nameof(BrugerId))]
    public virtual Bruger Bruger { get; set; } = null!;
}
