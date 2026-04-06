using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class EditAccount
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
