using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models;

public partial class Findtægt
{
    [Key]
    public int FindtægtId { get; set; }

    public string BrugerId { get; set; }

    public string Tekst { get; set; }

    public decimal? Indtægt { get; set; }

    public DateTime? Dato { get; set; }

    public virtual ApiUser Bruger { get; set; } = null!;

}