using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Models.Request.BudgetLimit
{
    public class CreateBudgetLimit
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public string CategoryName { get; set; }

        public long AccountId { get; set; }

        public decimal LimitAmount { get; set; }
    }
}
