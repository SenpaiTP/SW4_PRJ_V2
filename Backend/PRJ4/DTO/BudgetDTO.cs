namespace PRJ4.DTOs;


public class BudgetDTO
{
    public int SavingsGoal { get; set; }
    public DateOnly BudgetStart { get; set; }
    public DateOnly BudgetSlut { get; set; }
}

public class BudgetGetDTO
{
    public int BudgetId { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetStart { get; set; }
    public DateOnly BudgetSlut { get; set; }
    public int MonthlySavingsAmount { get; set;}
}