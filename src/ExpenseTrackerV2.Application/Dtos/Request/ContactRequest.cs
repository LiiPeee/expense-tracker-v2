using System;
using System.ComponentModel.DataAnnotations;
using ExpenseTrackerV2.Core.Domain.Enums;

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
    public TypeContact TypeContact { get; set; }
}
