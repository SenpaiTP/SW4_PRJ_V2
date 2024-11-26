
namespace PRJ4.DTOs
{
    public class FindtægtDTO
    {
        public int brugerId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; } = null!;
        public DateTime? Dato { get; set; }
        
    }

    public class FindtægtCreateDTO
    {
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; } = null!;
        public DateTime? Dato { get; set; }
        // public int KategoriId { get; set; }
        // public string KategoriNavn { get; set; }
        // Optionally, you can include BrugerId, but it's probably better to use the authenticated user's ID in the backend
    }

    public class FindtægtUpdateDTO
        {
            public decimal? Indtægt { get; set; }  // Nullable to allow partial updates
            public string? Tekst { get; set; }
            public DateTime? Dato { get; set; }
            // public int? KategoriId { get; set; }
            // public string? KategoriNavn { get; set; }
            // Optionally, you can include BrugerId, but it's probably better to use the authenticated user's ID in the backend
        }

    public class FindtægtResponseDTO
    {
        public int FindtægtId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; } = null!;
        public DateTime? Dato { get; set; }
        
    }
}