using Microsoft.Extensions.Logging;
using TaskManagement.BusinessLogic.Commands;
using TaskManagement.Common.Models;

namespace TaskManagement.DataAccessLayer.Commands
{
    public interface ITaskCommandHandlers
    {
        System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(CreateTaskCommand command);
        System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(UpdateTaskCommand command);
        System.Threading.Tasks.Task HandleAsync(DeleteTaskCommand command);
        System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(CompleteTaskCommand command);
    }

    public class TaskCommandHandlers : ITaskCommandHandlers
    {
        private readonly ILogger<TaskCommandHandlers> _logger;
        private readonly List<Common.Models.Task> _tasks;
        private static int _nextId = 1;

        public TaskCommandHandlers(ILogger<TaskCommandHandlers> logger, List<Common.Models.Task> tasks)
        {
            _logger = logger;
            _tasks = tasks;
        }

        public System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(CreateTaskCommand command)
        {
            _logger.LogInformation("Handling CreateTaskCommand: {Title}", command.Title);

            var task = new Common.Models.Task
            {
                Id = _nextId++,
                Title = command.Title,
                Description = command.Description,
                DueDate = command.DueDate,
                Priority = command.Priority,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _tasks.Add(task);
            return System.Threading.Tasks.Task.FromResult(task);
        }

        public System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(UpdateTaskCommand command)
        {
            _logger.LogInformation("Handling UpdateTaskCommand: {TaskId}", command.Id);

            var task = _tasks.FirstOrDefault(t => t.Id == command.Id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {command.Id} not found");
            }

            task.Title = command.Title;
            task.Description = command.Description;
            if (command.DueDate.HasValue)
            {
                task.DueDate = command.DueDate.Value;
            }
            task.Priority = command.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            return System.Threading.Tasks.Task.FromResult(task);
        }

        public System.Threading.Tasks.Task HandleAsync(DeleteTaskCommand command)
        {
            _logger.LogInformation("Handling DeleteTaskCommand: {TaskId}", command.Id);

            var task = _tasks.FirstOrDefault(t => t.Id == command.Id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {command.Id} not found");
            }

            _tasks.Remove(task);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task<Common.Models.Task> HandleAsync(CompleteTaskCommand command)
        {
            _logger.LogInformation("Handling CompleteTaskCommand: {TaskId}", command.Id);

            var task = _tasks.FirstOrDefault(t => t.Id == command.Id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {command.Id} not found");
            }

            task.IsCompleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            return System.Threading.Tasks.Task.FromResult(task);
        }
    }
} 