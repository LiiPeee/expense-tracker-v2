

namespace BudgetTracker.Core.Domain.Dtos.Output
{
    public class IPagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}


