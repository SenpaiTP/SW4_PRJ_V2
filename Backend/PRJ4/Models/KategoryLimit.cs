using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class KategoryLimit
{
    [Key]
    public int KategoryLimitId { get; set; }
    public int KategoryId { get; set; }
    public string BrugerId { get; set; }
    public int Limit { get; set; }
    
//ForeignKeys
    [ForeignKey(nameof(KategoryId))]
    public Kategori Kategory { get; set; }

    [ForeignKey(nameof(BrugerId))]
    public ApiUser? Bruger { get; set; }

}