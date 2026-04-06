using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.WebApi.Models.Category
{
    public class CreateCategoryInput
    {
        [Required(ErrorMessage = "Name is required")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
