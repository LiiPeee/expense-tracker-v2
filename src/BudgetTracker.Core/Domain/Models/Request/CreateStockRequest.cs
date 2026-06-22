using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Models.Request
{
    public class CreateStockRequest
    {
        public required string Ticker { get; set; }

        public required string Title { get; set; }

        public required decimal Price { get; set; }

        public long Quantity { get; set; }

        public string Description { get; set; }
    }
}
