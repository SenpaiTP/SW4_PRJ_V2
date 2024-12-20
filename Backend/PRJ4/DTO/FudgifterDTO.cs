using System;
using System.Text.Json.Serialization;
using PRJ4.Models;
// using MealApp.Converters;

namespace PRJ4.DTOs
    {
        public class nyFudgifterDTO
        {
            public decimal Pris {get; set;}
            public int KategoriId {get; set;}
            public string KategoriNavn {get; set;}
            public DateTime Dato {get;set;}
            public string? Tekst { get; set; }
        }
        public class FudgifterResponseDTO
        {
            public int FudgiftId { get; set; }
            public decimal Pris { get; set; }
            public string? Tekst { get; set; }
            public string? KategoriNavn { get; set; }
            public DateTime? Dato { get; set; }
        }
        public class FudgifterUpdateDTO
        {
            public decimal? Pris { get; set; } 
            public string? Tekst { get; set; }
            public DateTime? Dato { get; set; }
            public int? KategoriId { get; set; }
            public string? KategoriNavn { get; set; }
            
        }

    }