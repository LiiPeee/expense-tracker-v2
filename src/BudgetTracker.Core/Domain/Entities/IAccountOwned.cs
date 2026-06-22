namespace BudgetTracker.Core.Domain.Entities;

/// <summary>
/// Marks an entity as belonging to a single account (tenant). Repositories for
/// these entities must scope every read/write by <see cref="AccountId"/>.
/// </summary>
public interface IAccountOwned
{
    long AccountId { get; }
}
