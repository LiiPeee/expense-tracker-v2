using ExpenseTrackerV2.Application.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Service
{
    public interface IContactAppService
    {
        Task CreateAsync(ContactRequest request);
    }
}
