using System.ComponentModel.DataAnnotations;

namespace TSU.IPW.API.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }

}
