using Microsoft.EntityFrameworkCore.Storage;

namespace PRJ4.Models;


public class CategoryLimitCreateDTO
{
    public int CategoryId { get; set; }
    public int Limit { get; set; }
}

public class CategoryLimitUpdateDTO
{
    public int CategoryId { get; set; }
    public int Limit { get; set; }
}

public class CategoryLimitResponseDTO
{
    public int CategoryLimitId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int Limit { get; set; }
    
}

