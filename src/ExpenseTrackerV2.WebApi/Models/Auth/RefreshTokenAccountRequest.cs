using System;

namespace ExpenseTrackerV2.WebApi.Models.Auth;

public class RefreshTokenAccountRequest
{
    public required string RefreshToken { get; set; }

}
