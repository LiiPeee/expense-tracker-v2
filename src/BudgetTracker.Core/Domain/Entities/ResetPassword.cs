using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Entities
{
    public class ResetPassword : BaseEntity, IAccountOwned
    {
        public long AccountId { get; set; }

        public string HashedToken { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}


