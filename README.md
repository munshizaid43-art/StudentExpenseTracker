# RupeeTrack — Student Expense & Budget Tracker

A student mini-project built with ASP.NET Core MVC, Entity Framework Core, MySQL, ASP.NET Core Identity and Bootstrap 5.

## Features
- Register, login, logout, and protected user-specific expense data
- Dashboard with current-month spending, budget remaining, lifetime totals and recent transactions
- Full expense CRUD (create, read, update, delete)
- Categories, payment methods, date filtering, search and sorting
- Monthly budget and over-budget warning
- Data Annotation validation, anti-forgery tokens and per-user data filtering
- Session and TempData usage
- Responsive Bootstrap UI with Indian rupee formatting

## Requirements
- .NET 8 SDK
- MySQL Server 8.x (or compatible MySQL server)
- Visual Studio 2022 with ASP.NET and web development workload, or .NET CLI
- Internet connection for Bootstrap/jQuery validation CDN resources

## Setup
1. Create the database in MySQL Workbench:
   ```sql
   CREATE DATABASE StudentExpenseTrackerDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
2. Open `appsettings.json` and replace `YOUR_MYSQL_USERNAME` and `YOUR_MYSQL_PASSWORD` with your local MySQL credentials. Do not commit real credentials to a public repository.
3. Open a terminal in the project folder and run:
   ```bash
   dotnet restore
   dotnet tool install --global dotnet-ef --version 8.0.11
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet run
   ```
   If `dotnet-ef` is already installed, use `dotnet tool update --global dotnet-ef --version 8.0.11`.
4. Open the local URL printed in the terminal. Register a new account before using the dashboard.

## Important notes
- The first migration is created locally by `dotnet ef migrations add InitialCreate`; the generated `Migrations` directory is then used to create tables in MySQL. Commit the generated migration files to GitHub.
- User passwords are handled by ASP.NET Core Identity's password hashing; passwords are not stored as plain text.
- This starter project has not been compiled or tested in this environment. Run the setup and test checklist on your Windows machine and fix any environment-specific issues before submission.
- Bootstrap and client-side validation scripts load from CDNs. Server-side validation still runs if those scripts are unavailable.

## Test checklist
- [ ] Register with a valid email and password
- [ ] Duplicate email is rejected
- [ ] Login with correct credentials works; incorrect credentials fail
- [ ] Unauthenticated user is redirected to login
- [ ] Add expense and verify it appears in MySQL
- [ ] Edit and delete an expense
- [ ] Try an invalid/negative amount and verify validation
- [ ] Search by title, filter by category and dates, sort by amount
- [ ] Set monthly budget and verify remaining budget calculation
- [ ] Register a second account and confirm it cannot see the first account's expenses
- [ ] Logout and confirm protected pages require login

## Architecture
- **Models:** `ApplicationUser`, `Expense`, `MonthlyBudget`
- **ViewModels:** `LoginViewModel`, `RegisterViewModel`, `DashboardViewModel`
- **Data:** `ApplicationDbContext` extends `IdentityDbContext`
- **Controllers:** `AccountController`, `HomeController`, `ExpensesController`, `BudgetsController`
- **Views:** Razor pages organized by controller
- **Database:** Identity tables plus `Expenses` and `MonthlyBudgets`

## State management
- `TempData["Success"]` shows a one-time success message after CRUD/budget actions.
- Session stores a welcome message and last-login timestamp for demonstration.
- Authentication state is managed by ASP.NET Core Identity cookies.
