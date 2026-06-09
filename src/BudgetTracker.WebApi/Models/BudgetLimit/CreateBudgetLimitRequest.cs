using BudgetTracker.Core.Domain.Entities;

namespace BudgetTracker.WebApi.Models.BudgetLimit
{
    public class CreateBudgetLimitRequest
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public string CategoryName { get; set; }

        public decimal LimitAmount { get; set; }
    }
}
