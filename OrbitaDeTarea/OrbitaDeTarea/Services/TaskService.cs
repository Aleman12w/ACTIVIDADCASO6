using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using OrbitaDeTarea.Models;

namespace OrbitaDeTarea.Services
{
    public class TaskService
    {
        // Singleton instance
        private static TaskService? _instance;

        public static TaskService Instance
        {
            get
            {
                _instance ??= new TaskService();
                return _instance;
            }
        }

        // Collection that stores all tasks in memory
        public ObservableCollection<TaskItem> Tasks { get; } = new();

        private int nextId = 1;

        private TaskService()
        {
            // Sample tasks
            Tasks.Add(new TaskItem
            {
                Id = nextId++,
                Title = "Design login screen",
                Description = "Create the login interface.",
                Assignee = "Antonio",
                Priority = "High",
                Column = "To do"
            });

            Tasks.Add(new TaskItem
            {
                Id = nextId++,
                Title = "Create database model",
                Description = "Create the basic data model.",
                Assignee = "Paola",
                Priority = "Medium",
                Column = "In progress"
            });

            Tasks.Add(new TaskItem
            {
                Id = nextId++,
                Title = "Write documentation",
                Description = "Complete the project documentation.",
                Assignee = "Jhoseline",
                Priority = "Low",
                Column = "Done"
            });
        }

        public void AddTask(TaskItem task)
        {
            task.Id = nextId++;
            task.CreatedOn = DateTime.Now;

            Tasks.Add(task);
        }

        public void UpdateTask(TaskItem task)
        {
            var existingTask = Tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existingTask == null)
                return;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Assignee = task.Assignee;
            existingTask.Priority = task.Priority;
            existingTask.Column = task.Column;
        }

        public void DeleteTask(TaskItem task)
        {
            Tasks.Remove(task);
        }

        public void MoveTask(TaskItem task, string newColumn)
        {
            task.Column = newColumn;
        }
    }
}
