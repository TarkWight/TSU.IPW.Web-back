namespace TSU.IPW.API.Domain.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }


}
