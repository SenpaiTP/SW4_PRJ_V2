using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ4.Models;

public partial class KategoryLimit
{
    [Key]
    public int KategoryLimitId { get; set; }
    public string BrugerId { get; set; }
    public int KategoriId { get; set; }
    public int Limit { get; set; }
    
//ForeignKeys
    [ForeignKey(nameof(BrugerId))]
    public ApiUser Bruger { get; set; }

    [ForeignKey(nameof(KategoriId))]
    public Kategori Kategori { get; set; }

}