namespace PRJ4.Models;

public class KategoryLimitResponseDTO
{
    public int KategoryId { get; set; }
    public string KategoryName { get; set; }
    public int Limit { get; set; }
}

public class KategoryLimitCreateDTO
{
    public int KategoryId { get; set; }
    public int Limit { get; set; }
}