namespace TSU.IPW.API.Domain.Entities
{
    public class TaskList
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
