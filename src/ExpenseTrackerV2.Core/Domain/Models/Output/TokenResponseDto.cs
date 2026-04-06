using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Models.Output
{
    public class TokenResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
