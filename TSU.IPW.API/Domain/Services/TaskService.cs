using TSU.IPW.API.Data.Repositories;
using TSU.IPW.API.Domain.DTOs;
using TSU.IPW.API.Domain.Entities;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;

    public TaskService(ITaskRepository taskRepository, ITagRepository tagRepository)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllTasksAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetTaskByIdAsync(id);
    }

    public async Task AddTaskAsync(TaskItem taskItem)
    {
        await _taskRepository.AddTaskAsync(taskItem);
    }

    public async Task UpdateTaskAsync(TaskItem taskItem)
    {
        await _taskRepository.UpdateTaskAsync(taskItem);
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _taskRepository.DeleteTaskAsync(id);
    }

    public async Task MarkTaskCompleteAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            throw new Exception("Task not found");
        }

        task.Completed = true;
        await _taskRepository.UpdateTaskAsync(task);
    }

    public async Task MarkTaskIncompleteAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            throw new Exception("Task not found");
        }

        task.Completed = false;
        await _taskRepository.UpdateTaskAsync(task);
    }

    public async Task AddTagToTaskAsync(int taskId, TagDto tagDto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
        {
            throw new Exception("Task not found");
        }

        var tag = await _tagRepository.GetTagByIdAsync(tagDto.Id);
        if (tag == null)
        {
            throw new Exception("Tag not found");
        }

        await _taskRepository.AddTagToTaskAsync(taskId, tag.Id);
    }

    public async Task<IEnumerable<TagDto>> GetTagsForTaskAsync(int taskId)
    {
        var tags = await _taskRepository.GetTagsForTaskAsync(taskId);
        return tags.Select(t => new TagDto
        {
            Id = t.Id,
            Name = t.Name
        });
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
        {
            throw new Exception("Task not found");
        }

        await _taskRepository.RemoveTagFromTaskAsync(taskId, tagId);
    }

    public async Task AddTagAsync(CreateTagDto createTagDto)
    {
        var tag = new Tag
        {
            Name = createTagDto.Name
        };

        await _tagRepository.AddTagAsync(tag);
    }

}