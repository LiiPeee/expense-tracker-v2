using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request.BudgetLimit;
using BudgetTracker.WebApi.Models.BudgetLimit;

namespace BudgetTracker.WebApi.Mapper
{
    public static class CreateBudgetLimitRequestMapper
    {
        public static CreateBudgetLimit ToCreateBudgetLimit(this CreateBudgetLimitRequest request, long accountId)

        {
            return new CreateBudgetLimit
            {
                Month = request.Month,
                Year = request.Year,
                CategoryName = request.CategoryName,
                AccountId = accountId,
                LimitAmount = request.LimitAmount
            };
        }
    }
}
