using System;
using TaskManagement.Common.Models;

namespace TaskManagement.BusinessLogic.Commands
{
    public class UpdateTaskCommand : BaseCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
} 