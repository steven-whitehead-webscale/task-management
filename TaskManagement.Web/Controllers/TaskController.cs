using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.BusinessLogic.Models;

namespace TaskManagement.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<TaskManagement.BusinessLogic.Models.Task>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<TaskManagement.BusinessLogic.Models.Task>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult<TaskManagement.BusinessLogic.Models.Task>> CreateTask(TaskManagement.BusinessLogic.Models.Task task)
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateTask(int id, TaskManagement.BusinessLogic.Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            await _taskService.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
} 