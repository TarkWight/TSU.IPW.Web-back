using System.ComponentModel.DataAnnotations;

namespace TSU.IPW.API.Domain.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool? Completed { get; set; }
    }

}
