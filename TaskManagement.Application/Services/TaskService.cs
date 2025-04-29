using TaskManagement.BusinessLogic.Commands;
using TaskManagement.BusinessLogic.Models;
using TaskManagement.BusinessLogic.Queries;
using TaskManagement.Common.Models;
using TaskManagement.DataAccessLayer.Commands;
using TaskManagement.DataAccessLayer.Queries;
using TaskManagement.Infrastructure.Logging;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskCommandHandlers _commandHandlers;
        private readonly ITaskQueryHandlers _queries;
        private readonly ILoggingService _loggingService;

        public TaskService(ITaskCommandHandlers commandHandlers, ITaskQueryHandlers queries, ILoggingService loggingService)
        {
            _commandHandlers = commandHandlers;
            _queries = queries;
            _loggingService = loggingService;
        }

        public async System.Threading.Tasks.Task<IEnumerable<BusinessLogic.Models.Task>> GetAllTasksAsync()
        {
            _loggingService.LogInformation("Getting all tasks");
            var commonTasks = await _queries.HandleAsync(new GetAllTasksQuery());
            return commonTasks.Select(task => ConvertToBusinessLogicTask(task));
        }

        public async System.Threading.Tasks.Task<BusinessLogic.Models.Task> GetTaskByIdAsync(int id)
        {
            _loggingService.LogInformation($"Getting task with id: {id}");
            var commonTask = await _queries.HandleAsync(new GetTaskByIdQuery { Id = id });
            if (commonTask == null)
            {
                return null;
            }
            return ConvertToBusinessLogicTask(commonTask);
        }

        public async System.Threading.Tasks.Task<BusinessLogic.Models.Task> CreateTaskAsync(BusinessLogic.Models.Task task)
        {
            _loggingService.LogInformation($"Creating new task: {task.Title}");
            var command = new CreateTaskCommand
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = DateTime.UtcNow.AddDays(7), // Default due date
                Priority = TaskPriority.Medium // Default priority
            };

            var commonTask = await _commandHandlers.HandleAsync(command);
            return ConvertToBusinessLogicTask(commonTask);
        }

        public async System.Threading.Tasks.Task<BusinessLogic.Models.Task> UpdateTaskAsync(BusinessLogic.Models.Task task)
        {
            _loggingService.LogInformation($"Updating task: {task.Id}");
            var command = new UpdateTaskCommand
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = null // Not updating due date
            };

            var commonTask = await _commandHandlers.HandleAsync(command);
            return ConvertToBusinessLogicTask(commonTask);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            _loggingService.LogInformation($"Deleting task: {id}");
            var command = new DeleteTaskCommand { Id = id };
            await _commandHandlers.HandleAsync(command);
        }

        private BusinessLogic.Models.Task ConvertToBusinessLogicTask(Common.Models.Task commonTask)
        {
            if (commonTask == null)
            {
                return null;
            }

            return new BusinessLogic.Models.Task
            {
                Id = commonTask.Id,
                Title = commonTask.Title,
                Description = commonTask.Description,
                IsCompleted = commonTask.IsCompleted,
                CreatedDate = commonTask.CreatedAt,
                CompletedDate = commonTask.IsCompleted ? commonTask.UpdatedAt : null
            };
        }
    }
} 