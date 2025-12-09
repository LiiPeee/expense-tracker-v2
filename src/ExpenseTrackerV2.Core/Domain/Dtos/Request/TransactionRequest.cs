using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using System;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class TransactionRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public string TransactionName { get; set; }
    public required string CategoryName { get; set; }
    public string TypeTransaction { get; set; } 
    public bool Paid { get; set; }
    public int? NumberOfInstallment { get; set; }
    public int? DateOfInstallment { get; set; }
    public string Recurrence { get; set; }
    public string ContactName { get; set; } = null!;
}
