
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class Saving
{
    [Key]
    public int SavingId { get; set; }
    public decimal Amount {get; set;}
    public DateTime Date {get;set;}
    public int BudgetId { get; set; }

    [ForeignKey(nameof(BudgetId))]
    public Budget Budget { get; set; } 

}