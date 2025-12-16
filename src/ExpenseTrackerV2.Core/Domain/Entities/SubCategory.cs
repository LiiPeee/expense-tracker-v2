using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
