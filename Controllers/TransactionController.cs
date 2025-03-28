using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: /Transaction/
        // GET: /Transaction/
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";

            var transactionsQuery = from t in _context.Transactions select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                transactionsQuery = transactionsQuery.Where(t => t.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    transactionsQuery = transactionsQuery.OrderByDescending(t => t.Date);
                    break;
                case "Amount":
                    transactionsQuery = transactionsQuery.OrderBy(t => t.Amount);
                    break;
                case "amount_desc":
                    transactionsQuery = transactionsQuery.OrderByDescending(t => t.Amount);
                    break;
                default:
                    transactionsQuery = transactionsQuery.OrderBy(t => t.Date);
                    break;
            }

            // Fetch transactions
            var transactionList = await transactionsQuery.AsNoTracking().ToListAsync();

            // Compute summary values
            var totalIncome = transactionList.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpense = transactionList.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var balance = totalIncome - totalExpense;
            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpense = totalExpense;
            ViewBag.Balance = balance;

            // Prepare chart data: group by date
            var incomeData = transactionList
                .Where(t => t.Type == TransactionType.Income)
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.Amount) })
                .ToList();

            var expenseData = transactionList
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.Amount) })
                .ToList();

            // Serialize data for JavaScript consumption
            ViewBag.IncomeData = System.Text.Json.JsonSerializer.Serialize(incomeData);
            ViewBag.ExpenseData = System.Text.Json.JsonSerializer.Serialize(expenseData);

            return View(transactionList);
        }

        
        // GET: /Transaction/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: /Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }
        
        // GET: /Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();
            
            return View(transaction);
        }
        
        // POST: /Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transaction transaction)
        {
            if (id != transaction.Id)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Transactions.Any(e => e.Id == transaction.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }
        
        // GET: /Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
                return NotFound();
            
            return View(transaction);
        }
        
        // POST: /Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // Optional: Export to CSV (see below)
        public IActionResult ExportCsv()
        {
            var transactions = _context.Transactions.ToList();
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Date,Description,Amount,Type");
            foreach (var t in transactions)
            {
                csv.AppendLine($"{t.Date.ToShortDateString()},{t.Description},{t.Amount},{t.Type}");
            }
            return File(System.Text.Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Transactions.csv");
        }
    }
}
