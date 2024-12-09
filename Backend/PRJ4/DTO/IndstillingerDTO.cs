using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.DTOs
{
public class IndstillingerDTO
{
    public bool SetPieChart {get; set;} = false;
    public bool SetSøjlediagram { get; set; } = false;
    public bool SetIndtægter { get; set; } = true;
    public bool SetUdgifter { get; set; } = true;
    public bool SetBudget { get; set; } = true;

}

public class UpdateThemeDTO
{
    public bool SetTheme {get; set;}
}

}