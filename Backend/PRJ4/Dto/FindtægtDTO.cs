
namespace PRJ4.DTOs
{
    public class FindtægtDTO
    {
        public decimal Indtægt { get; set; }
        public string Tekst { get; set; } = null!;
        public DateTime? Dato { get; set; }
        
    }

    public class FindtægtResponseDTO
    {
        public decimal Indtægt { get; set; }
        public string Tekst { get; set; } = null!;
        public DateTime? Dato { get; set; }
        public int FindtægtId { get; set; }
    }
}