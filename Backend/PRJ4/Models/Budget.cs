
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class Budget
{
    [Key]
    public int BudgetId { get; set; }
    [Required]
    public string BudgetName { get; set; }
    [Required]
    public string BrugerId { get; set; }
    public int SavingsGoal { get; set; }
    public DateOnly BudgetStart { get; set; }
    public DateOnly BudgetSlut { get; set; }
    
    [ForeignKey(nameof(BrugerId))]
    public ApiUser Bruger { get; set; } 

    public virtual ICollection<Saving> Savings { get; set; }

}