using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRJ4.Models;

public partial class Kategori
{
    [Key]
    public int KategoriId { get; set; }
    public string Name {get; set;}
}
