using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction
{
    public class PaidTransactionRequest
    {
        public long TransactionId { get; set; }

        public bool Paid { get; set; }
    }
}
