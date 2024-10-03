namespace TSU.IPW.API.Domain.Entities
{
    public class TaskList
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<TaskItem>? Tasks { get; set; }
    }
}
