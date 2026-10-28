using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseBudgetTracker.Data;
using StudentExpenseBudgetTracker.Models;

namespace StudentExpenseBudgetTracker.Controllers;

[Authorize]
public class ExpensesController : Controller
{
    private readonly ApplicationDbContext _context;
    private static readonly string[] Categories = { "Food", "Travel", "Education", "Shopping", "Bills", "Health", "Other" };
    private static readonly string[] PaymentMethods = { "Cash", "UPI", "Debit/Credit Card", "Net Banking", "Other" };

    public ExpensesController(ApplicationDbContext context) => _context = context;

    private string CurrentUserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public async Task<IActionResult> Index(string? search, string? category, DateTime? fromDate, DateTime? toDate, string? sort)
    {
        var query = _context.Expenses.Where(e => e.UserId == CurrentUserId);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title.Contains(search));
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(e => e.Category == category);
        if (fromDate.HasValue) query = query.Where(e => e.ExpenseDate >= fromDate.Value.Date);
        if (toDate.HasValue) query = query.Where(e => e.ExpenseDate < toDate.Value.Date.AddDays(1));

        query = sort == "amount" ? query.OrderByDescending(e => e.Amount) : query.OrderByDescending(e => e.ExpenseDate);
        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
        ViewBag.Sort = sort;
        ViewBag.Categories = Categories;
        ViewBag.FilteredTotal = await query.SumAsync(e => (decimal?)e.Amount) ?? 0;
        return View(await query.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = Categories;
        ViewBag.PaymentMethods = PaymentMethods;
        return View(new Expense { ExpenseDate = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (!Categories.Contains(expense.Category)) ModelState.AddModelError(nameof(expense.Category), "Select a valid category.");
        if (!PaymentMethods.Contains(expense.PaymentMethod)) ModelState.AddModelError(nameof(expense.PaymentMethod), "Select a valid payment method.");
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = Categories; ViewBag.PaymentMethods = PaymentMethods;
            return View(expense);
        }

        expense.UserId = CurrentUserId;
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Expense added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
        if (expense == null) return NotFound();
        ViewBag.Categories = Categories; ViewBag.PaymentMethods = PaymentMethods;
        return View(expense);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Expense input)
    {
        if (id != input.Id) return BadRequest();
        if (!Categories.Contains(input.Category)) ModelState.AddModelError(nameof(input.Category), "Select a valid category.");
        if (!PaymentMethods.Contains(input.PaymentMethod)) ModelState.AddModelError(nameof(input.PaymentMethod), "Select a valid payment method.");
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
        if (expense == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = Categories; ViewBag.PaymentMethods = PaymentMethods;
            return View(input);
        }

        expense.Title = input.Title;
        expense.Amount = input.Amount;
        expense.Category = input.Category;
        expense.ExpenseDate = input.ExpenseDate;
        expense.PaymentMethod = input.PaymentMethod;
        expense.Description = input.Description;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Expense updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
        return expense == null ? NotFound() : View(expense);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == CurrentUserId);
        if (expense == null) return NotFound();
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Expense deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
