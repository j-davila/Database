using System.ComponentModel.DataAnnotations;

namespace Database.Dtos
{
    public record CreateItemDto
    {
        [Required] // makes sure there is no null values entered for name
        public string Name {get; init;}
        [Range(1,1000)] // makes sure no negative values are entered for price
        public decimal Price{get; init;}
    }
}