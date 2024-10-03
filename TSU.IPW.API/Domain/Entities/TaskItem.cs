﻿namespace TSU.IPW.API.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int ListId { get; set; }
    }
}
