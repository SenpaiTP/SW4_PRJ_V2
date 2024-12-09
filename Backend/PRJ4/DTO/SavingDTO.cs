
public class SavingCreateDTO
{
    public decimal Amount { get; set; }
    public DateTime Date {get;set;}

}

public class SavingUpdateDTO
{
    public decimal Amount { get; set; }
    public DateTime Date {get;set;}

}


public class SavingResponsDTO
{
    public int savingId {get; set; }
    public decimal Amount {get; set;}
    public int KategoryId { get; set; }
    public DateTime Date {get;set;}


}