using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Entities
{
    public interface  IBaseEntity
    {
        long Id { get; }
    }
}
