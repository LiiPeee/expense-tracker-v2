namespace BudgetTracker.Core.Domain.Entities
{
    public class Stock : BaseEntity, IAccountOwned
    {
        public long AccountId { get; set; }

        public string Ticker { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public decimal PriceBuyed { get; set; }

        public decimal PriceMarket {  get; set; }

        public decimal Avarage { get; set; }

        public long Quantity { get; set; }
    }
}
