

namespace ExpenseTrackerV2.Core.Domain.Dtos.Output
{
    public class IPagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Items { get; set; }

    }
}
