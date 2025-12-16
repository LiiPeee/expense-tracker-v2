using ExpenseTrackerV2.Core.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class ContactRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Phone { get; set; }
    public string? Document { get; set; }
    [Required(ErrorMessage = "TypeContact is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TypeContact TypeContact { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ContactName { get; set; } = null!;
    public bool IsPrimary { get; set; }
}
