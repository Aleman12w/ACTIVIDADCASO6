using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitaDeTarea.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Assignee { get; set; } = "";

        public string Priority { get; set; } = "Medium";

        public string Column { get; set; } = "To do";

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Returns a color according to the task priority
        public Color PriorityColor
        {
            get
            {
                return Priority switch
                {
                    "High" => Colors.Red,
                    "Medium" => Colors.Orange,
                    "Low" => Colors.Green,
                    _ => Colors.Gray
                };
            }
        }
    }
    }
