using System;

namespace TaskManagement.Common.Models
{
    public class TaskFilter
    {
        public string SearchTerm { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public bool? IsOverdue { get; set; }
        public string[] Tags { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "DueDate";
        public bool SortDescending { get; set; } = false;
    }
} 