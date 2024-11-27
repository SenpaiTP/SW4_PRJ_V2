namespace PRJ4.DTOs;


public class BudgetCreateDTO
{
    public string BudgetName { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
}

public class BudgetResponseDTO
{
    public int BudgetId { get; set; }
    public string BrugerId { get; set; }
    public string BudgetName { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetSlut { get; set; }
    public int MonthlySavingsAmount { get; set;}
    public decimal MoneySaved { get; set; } 
    //public decimal PercentOfGoal { get; set; }


}