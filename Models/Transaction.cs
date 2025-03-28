using System;

namespace FinanceTracker.Models
{
public class Transaction
{
    public int Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "REAL")]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;

    public TransactionType Type { get; set; } // Income or Expense
}


    public enum TransactionType
    {
        Income,
        Expense
    }
}
