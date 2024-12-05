
namespace PRJ4.DTOs
{
    public class VindtægtDTO
    {
        public string brugerId { get; set; }
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        public string KategoriNavn { get; set; }
        public int? KategoriId { get; set; }
        
    }

    public class VindtægtCreateDTO
    {
        public decimal? Indtægt { get; set; }
        public string Tekst { get; set; }
        public DateTime? Dato { get; set; }
        public string KategoriNavn { get; set; }
        public int? KategoriId { get; set; }
    }

    public class VindtægtUpdateDTO
        {
            public decimal? Indtægt { get; set; }  // Nullable to allow partial updates
            public string Tekst { get; set; }
            public DateTime? Dato { get; set; }
            public string KategoriNavn { get; set; }
            public int? KategoriId { get; set; }
        }

        public class VindtægtResponseDTO
        {
            public int VindtægtId { get; set; }
            public decimal? Indtægt { get; set; }
            public string Tekst { get; set; }
            public DateTime? Dato { get; set; }
            public string KategoriNavn { get; set; }
            public int? KategoriId { get; set; }
            
        }
}