using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account
{
    public class VerifyTokenRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
