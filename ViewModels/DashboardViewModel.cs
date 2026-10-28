using StudentExpenseBudgetTracker.Models;

namespace StudentExpenseBudgetTracker.ViewModels;

public class DashboardViewModel
{
    public decimal MonthlyBudget { get; set; }
    public decimal MonthExpenses { get; set; }
    public decimal RemainingBudget => MonthlyBudget - MonthExpenses;
    public decimal TotalAllExpenses { get; set; }
    public int ExpenseCount { get; set; }
    public List<Expense> RecentExpenses { get; set; } = new();
    public int Month { get; set; }
    public int Year { get; set; }
}
