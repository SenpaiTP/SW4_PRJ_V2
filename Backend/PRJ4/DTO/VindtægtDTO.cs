
namespace PRJ4.DTOs
{
    public class VindtægtDTO
    {
        public string brugerId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        
    }

    public class VindtægtCreateDTO
    {
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        // public int KategoriId { get; set; }
        // public string KategoriNavn { get; set; }
        // Optionally, you can include BrugerId, but it's probably better to use the authenticated user's ID in the backend
    }

    public class VindtægtUpdateDTO
        {
            public decimal? Indtægt { get; set; }  // Nullable to allow partial updates
            public string Tekst { get; set; }
            public DateTime? Dato { get; set; }
            // public int? KategoriId { get; set; }
            // public string? KategoriNavn { get; set; }
            // Optionally, you can include BrugerId, but it's probably better to use the authenticated user's ID in the backend
        }

    public class VindtægtResponseDTO
    {
        public int VindtægtId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        
    }
}