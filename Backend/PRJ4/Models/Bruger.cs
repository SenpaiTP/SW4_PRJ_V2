using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class Bruger
{
    [Key]
    public int BrugerId { get; set; }

    public string Fornavn { get; set; } = null!;
    public string Efternavn { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    
    //public virtual ICollection<Findtægt> Findtægts { get; set; } = new List<Findtægt>();

    //public virtual ICollection<Fudgifter> Fudgifters { get; set; } = new List<Fudgifter>();

    //public virtual ICollection<Vindtægter> Vindtægters { get; set; } = new List<Vindtægter>();

   // public virtual ICollection<Vudgifter> Vudgifters { get; set; } = new List<Vudgifter>();

    //public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
