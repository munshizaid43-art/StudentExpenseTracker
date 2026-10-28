using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentExpenseBudgetTracker.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(60)]
    public string FullName { get; set; } = string.Empty;
}
