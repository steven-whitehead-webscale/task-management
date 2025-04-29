using TaskManagement.BusinessLogic.Models;

namespace TaskManagement.Application.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<IEnumerable<TaskManagement.BusinessLogic.Models.Task>> GetAllTasksAsync();
        System.Threading.Tasks.Task<TaskManagement.BusinessLogic.Models.Task> GetTaskByIdAsync(int id);
        System.Threading.Tasks.Task<TaskManagement.BusinessLogic.Models.Task> CreateTaskAsync(TaskManagement.BusinessLogic.Models.Task task);
        System.Threading.Tasks.Task<TaskManagement.BusinessLogic.Models.Task> UpdateTaskAsync(TaskManagement.BusinessLogic.Models.Task task);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
    }
} 