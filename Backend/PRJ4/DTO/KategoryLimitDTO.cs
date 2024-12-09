namespace PRJ4.Models;

public class KategoryLimitGetDTO
{
    public int KategoryId { get; set; }
    public string KategoryName { get; set; }
    public int Limit { get; set; }
}

public class KategoryLimitReturnDTO
{
    public int KategoryId { get; set; }
    public int Limit { get; set; }
}