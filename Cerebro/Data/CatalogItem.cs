using Cerebro.Enum;
using System.ComponentModel.DataAnnotations;

namespace Cerebro.Data;

public class CatalogItem
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Type")]
    public CatalogItemType Type { get; set; }

    [Display(Name = "Default Unit Price")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal DefaultUnitPrice { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}
