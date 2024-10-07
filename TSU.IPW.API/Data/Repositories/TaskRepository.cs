using Microsoft.EntityFrameworkCore;
using TSU.IPW.API.Domain.Entities;

namespace TSU.IPW.API.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskItem taskItem);
        Task UpdateTaskAsync(TaskItem taskItem);
        Task DeleteTaskAsync(int id);
        Task AddTagToTaskAsync(int taskId, int tagId);
        Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId);
        Task RemoveTagFromTaskAsync(int taskId, int tagId);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task AddTaskAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTagToTaskAsync(int taskId, int tagId)
        {
            // Найти задачу по ID
            var task = await _context.TaskItems.Include(t => t.TaskTags).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new Exception("Task not found");
            }

            // Найти тег по ID
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
            {
                throw new Exception("Tag not found");
            }

            // Проверить, существует ли уже связь между задачей и тегом
            if (!task.TaskTags.Any(tt => tt.TagId == tagId))
            {
                // Создать новую связь между задачей и тегом
                var taskTag = new TaskTag
                {
                    TaskId = taskId,
                    TagId = tagId
                };

                task.TaskTags.Add(taskTag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId)
        {
            // Найти задачу по ID
            var task = await _context.TaskItems
                .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new Exception("Task not found");
            }

            // Получить список тегов для задачи
            return task.TaskTags.Select(tt => tt.Tag).ToList();
        }

        public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
        {
            // Найти задачу по ID
            var task = await _context.TaskItems
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new Exception("Task not found");
            }

            // Найти связь между задачей и тегом
            var taskTag = task.TaskTags.FirstOrDefault(tt => tt.TagId == tagId);
            if (taskTag != null)
            {
                task.TaskTags.Remove(taskTag);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Tag not found for this task");
            }
        }

    }
}
