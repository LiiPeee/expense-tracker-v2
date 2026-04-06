using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Dtos.Output
{
    public class FilterByMonthAndCategoryOutPut
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool Paid { get; set; }
        public string Name { get; set; }
    }
}
