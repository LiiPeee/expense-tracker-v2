using System;

namespace ExpenseTrackerV2.Core.Domain.Models.Output;

public class CreateAccountDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }

}
