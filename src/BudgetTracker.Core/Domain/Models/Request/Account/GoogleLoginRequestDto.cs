using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Models.Request.Account
{
    public record GoogleLoginRequestDto(string IdToken)
    {
    }
}
