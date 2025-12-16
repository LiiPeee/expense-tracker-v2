using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;

public class CreateTrasactionRequest
{
    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    [Required(ErrorMessage = "TransactionName is required")]
    public string TransactionName { get; set; }
    [Required(ErrorMessage = "CategoryName is required")]
    public string CategoryName { get; set; }
    public string SubCategoryName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TypeTransactions TypeTransaction { get; set; } 
    public bool Paid { get; set; }
    public int? NumberOfInstallment { get; set; }
    public int? DateOfInstallment { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Recurrence Recurrence { get; set; }
    public string ContactName { get; set; } = null!;
}
