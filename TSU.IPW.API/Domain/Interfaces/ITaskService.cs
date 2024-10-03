using TSU.IPW.API.Domain.Entities;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task AddTaskAsync(TaskItem taskItem);
    Task UpdateTaskAsync(TaskItem taskItem);
    Task DeleteTaskAsync(int id);

    Task MarkTaskCompleteAsync(int id);
    Task MarkTaskIncompleteAsync(int id);
}
