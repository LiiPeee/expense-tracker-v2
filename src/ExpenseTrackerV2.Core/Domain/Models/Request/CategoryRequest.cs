using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerV2.Application.Dtos.Request;

public class CategoryRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
