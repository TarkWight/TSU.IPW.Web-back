using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TSU.IPW.API.Domain.DTOs;
using TSU.IPW.API.Domain.Entities;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet(Name = "GetAllTasks")]
    [SwaggerOperation(Summary = "Получить все задачи", Description = "Возвращает список всех задач.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Completed = t.Completed
        }).ToList();

        return Ok(taskDtos);
    }

    [HttpGet("{id:int}", Name = "GetTaskById")]
    [SwaggerOperation(Summary = "Получить задачу по ID", Description = "Возвращает задачу по её ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed
        };

        return Ok(taskDto);
    }

    [HttpPost(Name = "CreateTask")]
    [SwaggerOperation(Summary = "Создать задачу", Description = "Создаёт новую задачу.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var taskItem = new TaskItem
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Completed = createTaskDto.Completed ?? false
        };

        await _taskService.AddTaskAsync(taskItem);

        var taskDto = new TaskDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            Completed = taskItem.Completed
        };

        return CreatedAtRoute("GetTaskById", new { id = taskItem.Id }, taskDto);
    }

    [HttpPut("{id:int}", Name = "UpdateTask")]
    [SwaggerOperation(Summary = "Обновить задачу", Description = "Обновляет существующую задачу.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        if (id != updateTaskDto.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        if (updateTaskDto.Completed.HasValue && task.Completed != updateTaskDto.Completed.Value)
        {
            if (updateTaskDto.Completed.Value)
            {
                await _taskService.MarkTaskCompleteAsync(id);
            }
            else
            {
                await _taskService.MarkTaskIncompleteAsync(id);
            }
        }
        else
        {
            await _taskService.UpdateTaskAsync(task);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteTask")]
    [SwaggerOperation(Summary = "Удалить задачу", Description = "Удаляет существующую задачу.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/complete", Name = "MarkTaskComplete")]
    [SwaggerOperation(Summary = "Завершить задачу", Description = "Устанавливает статус задачи как завершённый.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkTaskComplete(int id)
    {
        try
        {
            await _taskService.MarkTaskCompleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPatch("{id:int}/incomplete", Name = "MarkTaskIncomplete")]
    [SwaggerOperation(Summary = "Отменить завершение задачи", Description = "Устанавливает статус задачи как незавершённый.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkTaskIncomplete(int id)
    {
        try
        {
            await _taskService.MarkTaskIncompleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
