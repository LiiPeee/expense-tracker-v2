using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class CategoryRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }
}
