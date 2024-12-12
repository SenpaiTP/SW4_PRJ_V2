namespace PRJ4.DTOs;


public class BudgetCreateDTO
{
    public required string BudgetName { get; set; }  
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
}

public class BudgetUpdateDTO
{
    public required string BudgetName { get; set; }  
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
}

public class BudgetResponseDTO
{
    public int BudgetId { get; set; }
    public required string BudgetName { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
    public decimal MonthlySavingsAmount { get; set;}
    public decimal MoneySaved { get; set; } 
}

public class BudgetAllResponseDTO
{
    public int BudgetId { get; set; }
    public required string BudgetName { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }

}


