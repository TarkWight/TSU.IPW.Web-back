using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU.IPW.API.Domain.Entities;
using TSU.IPW.API.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}", Name = "GetTaskById")]
    [SwaggerOperation(Summary = "Получить задачу по Id", Description = "Возвращает задачу по указанному Id.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost(Name = "CreateTask")]
    [SwaggerOperation(Summary = "Создать задачу", Description = "Создает новую задачу и возвращает ее.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem task)
    {
        if (task == null)
        {
            return BadRequest();
        }

        await _taskService.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}", Name = "UpdateTask")]
    [SwaggerOperation(Summary = "Обновить задачу", Description = "Обновляет существующую задачу по указанному Id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
    {
        if (id != updatedTask.Id || updatedTask == null)
        {
            return BadRequest();
        }

        await _taskService.UpdateTaskAsync(updatedTask);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteTask")]
    [SwaggerOperation(Summary = "Удалить задачу", Description = "Удаляет задачу по указанному Id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/complete", Name = "MarkTaskComplete")]
    [SwaggerOperation(Summary = "Отметить задачу как выполненную", Description = "Отмечает задачу как выполненную по указанному Id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkTaskComplete(int id)
    {
        await _taskService.MarkTaskCompleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/incomplete", Name = "MarkTaskIncomplete")]
    [SwaggerOperation(Summary = "Отметить задачу как невыполненную", Description = "Отмечает задачу как невыполненную по указанному Id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkTaskIncomplete(int id)
    {
        await _taskService.MarkTaskIncompleteAsync(id);
        return NoContent();
    }

    [HttpPost("import", Name = "ImportTasks")]
    [SwaggerOperation(Summary = "Импортировать список задач", Description = "Импортирует список задач из предоставленного массива.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportTasks([FromBody] List<TaskItem> tasks)
    {
        if (tasks == null || !tasks.Any())
        {
            return BadRequest();
        }

        await _taskService.AddTasksAsync(tasks);
        return Ok();
    }
}
