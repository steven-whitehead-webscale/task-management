using TaskManagement.Common.Models;
using TaskManagement.Infrastructure.Logging;
using TaskManagement.BusinessLogic.Queries;

namespace TaskManagement.DataAccessLayer.Queries
{
    public interface ITaskQueryHandlers
    {
        System.Threading.Tasks.Task<IEnumerable<Common.Models.Task>> HandleAsync(GetAllTasksQuery query);
        System.Threading.Tasks.Task<Common.Models.Task?> HandleAsync(GetTaskByIdQuery query);
    }

    public class TaskQueryHandlers : ITaskQueryHandlers
    {
        private readonly List<Common.Models.Task> _tasks;
        private readonly ILoggingService _loggingService;

        public TaskQueryHandlers(List<Common.Models.Task> tasks, ILoggingService loggingService)
        {
            _tasks = tasks;
            _loggingService = loggingService;
        }

        public System.Threading.Tasks.Task<IEnumerable<Common.Models.Task>> HandleAsync(GetAllTasksQuery query)
        {
            _loggingService.LogInformation("Getting all tasks");
            return System.Threading.Tasks.Task.FromResult(_tasks.AsEnumerable());
        }

        public System.Threading.Tasks.Task<Common.Models.Task?> HandleAsync(GetTaskByIdQuery query)
        {
            _loggingService.LogInformation($"Getting task with id: {query.Id}");
            var task = _tasks.FirstOrDefault(t => t.Id == query.Id);
            return System.Threading.Tasks.Task.FromResult(task);
        }
    }
} 