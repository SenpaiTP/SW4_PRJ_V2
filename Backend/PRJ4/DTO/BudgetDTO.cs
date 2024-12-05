namespace PRJ4.DTOs;

//Create budget
public class BudgetCreateUpdateDTO
{
    public required string BudgetName { get; set; }  
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
}

//Get one budget to show all info
public class BudgetGetbyIdDTO
{
    public int BudgetId { get; set; }
    public required string BudgetName { get; set; }
    public int KategoryId { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
    public int MonthlySavingsAmount { get; set;}
    public decimal MoneySaved { get; set; } 
}


//View all user budget on frontpage for a user
public class BudgetGetAllDTO
{
    public int BudgetId { get; set; }
    public int KategoryId { get; set; }
    public required string BudgetName { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }

}


public class SavingDTO
{
    public decimal Saving { get; set; }
    public DateTime Date {get;set;}

}


public class SavingPostDTO
{
    public decimal Pris {get; set;}
    public int KategoryId { get; set; }
    public string ?KategoriNavn {get; set;}
    public DateTime Dato {get;set;}

}