using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class CategoryLimit
{
    [Key]
    public int CategoryLimitId { get; set; }
    public int CategoryId { get; set; }
    public string BrugerId { get; set; }
    public int Limit { get; set; }
    
//ForeignKeys
    [ForeignKey(nameof(CategoryId))]
    public Kategori Category { get; set; }

    [ForeignKey(nameof(BrugerId))]
    public ApiUser? Bruger { get; set; }

}