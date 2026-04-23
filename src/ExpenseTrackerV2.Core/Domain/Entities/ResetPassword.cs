using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Entities
{
    public class ResetPassword : BaseEntity
    {
        public long AccountId { get; set; }

        public string HashedToken { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
