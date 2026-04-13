using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class CategoryRequest
{
    [Required]
    [StringLength(30)]
    public required string Name { get; set; }
    [Required]
    [StringLength(30)]
    public string? Description { get; set; }
}
