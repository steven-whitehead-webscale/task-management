using System;
using System.Collections.Generic;

namespace TaskManagement.Common.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsOverdue => !IsCompleted && DueDate < DateTime.UtcNow;
    }
} 