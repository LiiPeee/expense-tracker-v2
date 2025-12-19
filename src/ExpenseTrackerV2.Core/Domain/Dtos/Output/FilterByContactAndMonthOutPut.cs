using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Dtos.Output
{
    public class FilterByContactAndMonthOutPut
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool Paid { get; set; }
        public string Name { get; set; }

        public ContactOutput Contact { get; set; }
    }

    public class ContactOutput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
