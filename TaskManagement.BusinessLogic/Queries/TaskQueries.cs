using TaskManagement.BusinessLogic.Models;

namespace TaskManagement.BusinessLogic.Queries
{
    public interface ITaskQueries
    {
        System.Threading.Tasks.Task<IEnumerable<TaskManagement.BusinessLogic.Models.Task>> GetAllTasksAsync();
        System.Threading.Tasks.Task<TaskManagement.BusinessLogic.Models.Task> GetTaskByIdAsync(int id);
    }

    public class TaskQueries : ITaskQueries
    {
        private readonly List<TaskManagement.BusinessLogic.Models.Task> _tasks;

        public TaskQueries(List<TaskManagement.BusinessLogic.Models.Task> tasks)
        {
            _tasks = tasks;
        }

        public System.Threading.Tasks.Task<IEnumerable<TaskManagement.BusinessLogic.Models.Task>> GetAllTasksAsync()
        {
            return System.Threading.Tasks.Task.FromResult(_tasks.AsEnumerable());
        }

        public System.Threading.Tasks.Task<TaskManagement.BusinessLogic.Models.Task> GetTaskByIdAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return System.Threading.Tasks.Task.FromResult(task);
        }
    }
} 