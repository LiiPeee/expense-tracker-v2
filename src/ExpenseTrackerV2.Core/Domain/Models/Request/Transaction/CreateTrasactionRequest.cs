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
    [Range(0.01, 999999.99)]
    public decimal Amount { get; set; }

    [JsonPropertyName("description")]
    [StringLength(30)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "TransactionName is required")]
    [StringLength(50)]
    [JsonPropertyName("transactionName")]
    public string TransactionName { get; set; }

    [Required(ErrorMessage = "CategoryName is required")]
    [JsonPropertyName("category")]
    public string CategoryName { get; set; }

    [JsonPropertyName("subCategory")]
    [StringLength(30)]
    public string SubCategoryName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("type")]
    public TypeTransactions TypeTransaction { get; set; }

    [JsonPropertyName("paid")]
    [Required(ErrorMessage = "Paid is required")]
    public bool Paid { get; set; }

    [Range(1, 420)]
    [JsonPropertyName("numberOfInstallment")]
    public int? NumberOfInstallment { get; set; }

    [Range(1, 31)]
    [JsonPropertyName("dateOfInstallment")]
    public int? DateOfInstallment { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("recurrence")]
    public Recurrence Recurrence { get; set; }

    [StringLength(50)]
    [JsonPropertyName("contactName")]
    public string ContactName { get; set; } = null!;
}
