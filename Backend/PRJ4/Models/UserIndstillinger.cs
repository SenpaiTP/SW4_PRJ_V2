using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models
{
    public class UserIndstillinger
    {
        [Key]
        public int Id {get; set;}
        public string BrugerId {get; set;}
        [ForeignKey(nameof(BrugerId))]
        public ApiUser Bruger {get; set;}

        public bool PieChartUdgifter {get; set;}
        public bool PieChartIndtægter {get; set;}
        public bool SenesteUdgifter {get; set;}
        public bool SenesteIndtægter {get; set;}
        public bool DarkLightMode {get; set;}
        
    }
}