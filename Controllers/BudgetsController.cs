using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseBudgetTracker.Data;
using StudentExpenseBudgetTracker.Models;

namespace StudentExpenseBudgetTracker.Controllers;

[Authorize]
public class BudgetsController : Controller
{
    private readonly ApplicationDbContext _context;
    public BudgetsController(ApplicationDbContext context) => _context = context;
    private string CurrentUserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Today;
        var budget = await _context.MonthlyBudgets.FirstOrDefaultAsync(b => b.UserId == CurrentUserId && b.Year == now.Year && b.Month == now.Month);
        var spent = await _context.Expenses.Where(e => e.UserId == CurrentUserId && e.ExpenseDate.Year == now.Year && e.ExpenseDate.Month == now.Month).SumAsync(e => (decimal?)e.Amount) ?? 0;
        ViewBag.Spent = spent;
        return View(budget ?? new MonthlyBudget { Month = now.Month, Year = now.Year });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MonthlyBudget input)
    {
        var now = DateTime.Today;
        input.Month = now.Month;
        input.Year = now.Year;
        if (!ModelState.IsValid) return await ReturnIndexWithErrors(input);

        var existing = await _context.MonthlyBudgets.FirstOrDefaultAsync(b => b.UserId == CurrentUserId && b.Year == input.Year && b.Month == input.Month);
        if (existing == null)
        {
            input.UserId = CurrentUserId;
            _context.MonthlyBudgets.Add(input);
        }
        else existing.Amount = input.Amount;

        await _context.SaveChangesAsync();
        TempData["Success"] = "Monthly budget saved.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> ReturnIndexWithErrors(MonthlyBudget input)
    {
        ViewBag.Spent = await _context.Expenses.Where(e => e.UserId == CurrentUserId && e.ExpenseDate.Year == input.Year && e.ExpenseDate.Month == input.Month).SumAsync(e => (decimal?)e.Amount) ?? 0;
        return View("Index", input);
    }
}
