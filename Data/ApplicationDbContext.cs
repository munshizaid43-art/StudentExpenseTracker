using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentExpenseBudgetTracker.Models;

namespace StudentExpenseBudgetTracker.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<MonthlyBudget> MonthlyBudgets => Set<MonthlyBudget>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasPrecision(10, 2);

        builder.Entity<MonthlyBudget>()
            .Property(b => b.Amount)
            .HasPrecision(10, 2);

        builder.Entity<Expense>()
            .HasIndex(e => new { e.UserId, e.ExpenseDate });

        builder.Entity<MonthlyBudget>()
            .HasIndex(b => new { b.UserId, b.Year, b.Month })
            .IsUnique();
    }
}
