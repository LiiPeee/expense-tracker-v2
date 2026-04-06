namespace ExpenseTrackerV2.Core.Domain.Models.Request.Account;

public class CreateAccountRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
