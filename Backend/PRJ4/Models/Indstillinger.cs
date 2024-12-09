// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models;

public  class Indstillinger
{
    [Key]
    public int IndstillingerId { get; set; }
    public string BrugerId { get; set; }
    public bool SetTheme {get; set;} = false;
    public bool SetPieChart {get; set;} = false;
    public bool SetSøjlediagram { get; set; } = true;
    public bool SetIndtægter { get; set; } = true;
    public bool SetUdgifter { get; set; } = true;
    public bool SetBudget { get; set; } = true;

    public virtual ApiUser Bruger { get; set; } = null!;

}