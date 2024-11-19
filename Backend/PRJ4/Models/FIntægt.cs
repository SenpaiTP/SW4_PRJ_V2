using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models;

public partial class Findtægt
{
    [Key]
    public int FindtægtId { get; set; }

    public int BrugerId { get; set; }

    public string Tekst { get; set; } = null!;

    public decimal Indtægt { get; set; }

    public DateTime? Dato { get; set; }

    public virtual Bruger Bruger { get; set; } = null!;
}