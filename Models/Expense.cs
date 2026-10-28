using System.ComponentModel.DataAnnotations;

namespace StudentExpenseBudgetTracker.Models;

public class Expense
{
    public int Id { get; set; }

    [Required, StringLength(80, MinimumLength = 2)]
    [Display(Name = "Expense title")]
    public string Title { get; set; } = string.Empty;

    [Required, Range(typeof(decimal), "0.01", "10000000", ErrorMessage = "Amount must be greater than ₹0.")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required, StringLength(30)]
    public string Category { get; set; } = "Other";

    [Required, DataType(DataType.Date)]
    [Display(Name = "Expense date")]
    public DateTime ExpenseDate { get; set; } = DateTime.Today;

    [Required, StringLength(30)]
    [Display(Name = "Payment method")]
    public string PaymentMethod { get; set; } = "Cash";

    [StringLength(300)]
    public string? Description { get; set; }

    public string UserId { get; set; } = string.Empty;
}
