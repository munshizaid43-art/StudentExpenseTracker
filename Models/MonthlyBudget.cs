using System.ComponentModel.DataAnnotations;

namespace StudentExpenseBudgetTracker.Models;

public class MonthlyBudget
{
    public int Id { get; set; }

    [Required, Range(typeof(decimal), "1", "100000000", ErrorMessage = "Budget must be greater than ₹0.")]
    public decimal Amount { get; set; }

    [Range(1, 12)]
    public int Month { get; set; }

    [Range(2020, 2100)]
    public int Year { get; set; }

    public string UserId { get; set; } = string.Empty;
}
