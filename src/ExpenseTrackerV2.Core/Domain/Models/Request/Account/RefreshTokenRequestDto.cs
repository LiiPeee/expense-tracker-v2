using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class RefreshTokenRequestDto
    {
        public long AccountId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
