using TSU.IPW.API.Domain.Entities;

namespace TSU.IPW.API.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskItem taskItem);
        Task AddTasksAsync(List<TaskItem> taskItems);
        Task UpdateTaskAsync(TaskItem taskItem);
        Task DeleteTaskAsync(int id);
        Task MarkTaskCompleteAsync(int id);
        Task MarkTaskIncompleteAsync(int id);
    }

}
