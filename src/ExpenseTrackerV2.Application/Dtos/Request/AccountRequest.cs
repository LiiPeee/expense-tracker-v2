using System;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class AccountRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required decimal Balance { get; set; }
    public required long OrganizationId { get; set; }

}
