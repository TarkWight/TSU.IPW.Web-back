using System.ComponentModel.DataAnnotations;

namespace TSU.IPW.API.Domain.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TaskTag
    {
        public int TaskId { get; set; }
        public TaskItem Task { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }

}
