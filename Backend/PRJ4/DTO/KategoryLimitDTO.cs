using Microsoft.EntityFrameworkCore.Storage;

namespace PRJ4.Models;


public class KategoryLimitCreateDTO
{
    public int KategoryId { get; set; }
    public int Limit { get; set; }
}

public class KategoryLimitUpdateDTO
{
    public int KategoryId { get; set; }
    public int Limit { get; set; }
}

public class KategoryLimitResponseDTO
{
    public int KategoryLimitId { get; set; }
    public int KategoryId { get; set; }
    public string KategoryName { get; set; }
    public int Limit { get; set; }
    
}

