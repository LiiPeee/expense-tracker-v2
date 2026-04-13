using System;

namespace ExpenseTrackerV2.WebApi.Models;

public class RefreshTokenAccountRequestDto
{
    public required string RefreshToken { get; set; }

}
