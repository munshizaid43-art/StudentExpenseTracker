using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseBudgetTracker.Data;
using StudentExpenseBudgetTracker.Models;
using StudentExpenseBudgetTracker.ViewModels;

namespace StudentExpenseBudgetTracker.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated != true) return RedirectToAction("Login", "Account");

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
        var now = DateTime.Today;
        var expenses = _context.Expenses.Where(e => e.UserId == userId);
        var monthly = expenses.Where(e => e.ExpenseDate.Year == now.Year && e.ExpenseDate.Month == now.Month);
        var budget = await _context.MonthlyBudgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Year == now.Year && b.Month == now.Month);

        var vm = new DashboardViewModel
        {
            Month = now.Month,
            Year = now.Year,
            MonthlyBudget = budget?.Amount ?? 0,
            MonthExpenses = await monthly.SumAsync(e => (decimal?)e.Amount) ?? 0,
            TotalAllExpenses = await expenses.SumAsync(e => (decimal?)e.Amount) ?? 0,
            ExpenseCount = await expenses.CountAsync(),
            RecentExpenses = await expenses.OrderByDescending(e => e.ExpenseDate).ThenByDescending(e => e.Id).Take(5).ToListAsync()
        };

        ViewBag.WelcomeMessage = HttpContext.Session.GetString("WelcomeMessage");
        HttpContext.Session.Remove("WelcomeMessage");
        ViewBag.LastLogin = HttpContext.Session.GetString("LastLogin");
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
