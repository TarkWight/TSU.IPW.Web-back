﻿using TSU.IPW.API.Data.Repositories;
using TSU.IPW.API.Domain.Entities;
using TSU.IPW.API.Domain.Interfaces;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
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

    public async Task AddTasksAsync(List<TaskItem> taskItems)
    {
        foreach (var task in taskItems)
        {
            await _taskRepository.AddTaskAsync(task);
        }
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
        if (task != null)
        {
            task.IsCompleted = true;
            await _taskRepository.UpdateTaskAsync(task);
        }
    }

    public async Task MarkTaskIncompleteAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task != null)
        {
            task.IsCompleted = false;
            await _taskRepository.UpdateTaskAsync(task);
        }
    }
}