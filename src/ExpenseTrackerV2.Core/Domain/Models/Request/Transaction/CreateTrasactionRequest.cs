using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;

public class CreateTrasactionRequest
{
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [Required(ErrorMessage = "TransactionName is required")]
    [JsonPropertyName("transactionName")]
    public string TransactionName { get; set; }
    [Required(ErrorMessage = "CategoryName is required")]
    [JsonPropertyName("category")]
    public string CategoryName { get; set; }
    [JsonPropertyName("subCategory")]
    public string SubCategoryName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("type")]
    public TypeTransactions TypeTransaction { get; set; }
    [JsonPropertyName("paid")]
    public bool Paid { get; set; }
    [JsonPropertyName("numberOfInstallment")]
    public int? NumberOfInstallment { get; set; }
    [JsonPropertyName("dateOfInstallment")]
    public int? DateOfInstallment { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("recurrence")]
    public Recurrence Recurrence { get; set; }
    [JsonPropertyName("contactName")]
    public string ContactName { get; set; } = null!;
}
