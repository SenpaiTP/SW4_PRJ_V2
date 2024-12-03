
namespace PRJ4.DTOs
{
    public class FindtægtDTO
    {
        public string brugerId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        
    }

    public class FindtægtCreateDTO
    {
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        public string KategoriNavn { get; set; }
        public int? KategoriId { get; set; }
        
    }

    public class FindtægtUpdateDTO
        {
            public decimal? Indtægt { get; set; }  // Nullable to allow partial updates
            public string Tekst { get; set; }
            public DateTime? Dato { get; set; }
             public int? KategoriId { get; set; }
            public string? KategoriNavn { get; set; }
        }

    public class FindtægtResponseDTO
    {
        public int FindtægtId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        public string KategoriNavn { get; set; }
        public int? KategoriId { get; set; }
        
    }
}