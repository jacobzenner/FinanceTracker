using System;

namespace FinanceTracker.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public decimal Limit { get; set; }
        public decimal Spent { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
