
namespace PRJ4.DTOs
{
    public class FindtægtDTO
    {
        public int BrugerId { get; set; }
        public string Tekst { get; set; } = null!;
        public decimal Indtægt { get; set; }
        public DateTime? Dato { get; set; }
        
    }
}