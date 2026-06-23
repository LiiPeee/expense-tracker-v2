namespace BudgetTracker.Core.Domain.Entities
{
    public class BudgetLimit : BaseEntity, IAccountOwned
    {
        public bool IsLimit { get; set; }

        public int Month { get; set; }
        
        public int Year { get; set; }

        public long CategoryId { get; set; }

        public decimal Percentage { get; set; }

        public Category Category { get; set; }

        public Account? Account { get; set; }

        public long AccountId { get; set; }

        public decimal LimitAmount { get; set; }
    }
}
