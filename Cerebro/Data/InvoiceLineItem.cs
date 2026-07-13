using System.ComponentModel.DataAnnotations;

namespace Cerebro.Data;

public class InvoiceLineItem
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    public int CatalogItemId { get; set; }

    public CatalogItem? CatalogItem { get; set; }

    public int Quantity { get; set; }

    [Display(Name = "Unit Price")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Line Total")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal LineTotal => Quantity * UnitPrice;
}
