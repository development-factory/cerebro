using System.ComponentModel.DataAnnotations;

namespace Cerebro.Data;

public class Invoice
{
    public int Id { get; set; }

    [Display(Name = "Invoice Number")]
    [Required]
    [StringLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Display(Name = "Client Name")]
    [Required]
    [StringLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Issue Date")]
    public DateTime IssueDate { get; set; }

    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Amount { get; set; }

    [Display(Name = "Paid")]
    public bool IsPaid { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
