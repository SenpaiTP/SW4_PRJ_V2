using System;
using System.Text.Json.Serialization;
using PRJ4.Models;
// using MealApp.Converters;

namespace PRJ4.DTOs
    {
        public class newVudgifterDTO
        {
            public decimal Pris {get; set;}
            public int KategoriId {get; set;}
            public string KategoriNavn {get; set;}
            public DateTime Dato {get;set;}
            public string? Tekst { get; set; }
        }
        public class VudgifterResponseDTO
        {
            public int VudgiftId { get; set; }
            public decimal Pris { get; set; }
            public string? Tekst { get; set; }
            public string? KategoriNavn { get; set; }
            public DateTime? Dato { get; set; }
        }
        public class VudgifterUpdateDTO
        {
            public decimal? Pris { get; set; }  // Nullable to allow partial updates
            public string? Tekst { get; set; }
            public DateTime? Dato { get; set; }
            public int? KategoriId { get; set; }
            public string? KategoriNavn { get; set; }
            // Optionally, you can include BrugerId, but it's probably better to use the authenticated user's ID in the backend
        }
    }
