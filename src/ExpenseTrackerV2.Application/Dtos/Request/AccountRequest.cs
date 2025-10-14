using System;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class AccountRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public decimal Balance { get; set; }

    public long OrganizationId { get; set; }

}
