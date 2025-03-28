using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var totalIncome = await _context.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .SumAsync(t => t.Amount);
            
            var totalExpense = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .SumAsync(t => t.Amount);
            
            var balance = totalIncome - totalExpense;
            
            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpense = totalExpense;
            ViewBag.Balance = balance;
            
            // Group transactions by date for charting
            var incomeData = await _context.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();
            
            var expenseData = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.Amount) })
                .ToListAsync();
            
            // Serialize the data to JSON for the chart
            ViewBag.IncomeData = System.Text.Json.JsonSerializer.Serialize(incomeData);
            ViewBag.ExpenseData = System.Text.Json.JsonSerializer.Serialize(expenseData);
            
            return View();
        }
    }
}
