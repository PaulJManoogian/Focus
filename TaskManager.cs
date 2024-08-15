using System;
using System.Collections.Generic;
using System.IO;

namespace FocusApp
{
    public class TaskManager
    {
        private List<TaskRecord> tasks = new List<TaskRecord>();

        public List<TaskRecord> Tasks
        {
            get { return tasks; }
        }

        public void AddTask(string description, string tag = "")
        {
            var task = new TaskRecord
            {
                Description = description,
                StartDate = DateTime.Now,
                EndDate = DateTime.MinValue,
                Tag = tag,
                Status = "Pending"
            };
            tasks.Add(task);
        }

        public void AddTaskWithTime(string description, string tag, DateTime startTime, DateTime endTime)
        {
            var task = new TaskRecord
            {
                Description = description,
                StartDate = startTime,
                EndDate = endTime,
                Tag = tag,
                Status = "Completed"
            };
            tasks.Add(task);
            SaveTasks("tasks.txt"); // Automatically save the task after completion
        }

        public void CompleteTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks[taskIndex].EndDate = DateTime.Now;
                tasks[taskIndex].Status = "Completed";
            }
        }


        public void AbandonTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks[taskIndex].EndDate = DateTime.Now;
                tasks[taskIndex].Status = "Abandoned";
            }
        }

        public void DeleteTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks.RemoveAt(taskIndex);
            }
        }

        public void ListTasks()
        {
            Console.WriteLine("┌───────────────────────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("|  #  | TASK NAME           | START DATE            | END DATE              | TAGGED       | STATUS     |");
            Console.WriteLine("├───────────────────────────────────────────────────────────────────────────────────────────────────────┤");


            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var taskcounter = i + 1;
                Console.WriteLine($"| {taskcounter.ToString().PadRight(3)} | {task.Description.PadRight(19)} | {task.StartDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20)} | {task.EndDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20)} | {task.Tag.PadRight(12)} | {task.Status.PadRight(10)} |");
            }
            Console.WriteLine("└───────────────────────────────────────────────────────────────────────────────────────────────────────┘");

        }

        public List<TaskRecord> GetTasksCompletedToday()
        {
            DateTime today = DateTime.Today;
            return tasks.FindAll(task => task.Status == "Completed" && task.EndDate.Date == today);
        }

        public List<TaskRecord> GetTasksCompletedAllTime()
        {
            return tasks.FindAll(task => task.Status == "Completed");
        }

        public List<TaskRecord> GetTasksByDateRange(DateTime startDate, DateTime? endDate = null)
        {
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            return tasks.FindAll(task => task.StartDate >= startDate && task.StartDate <= endDate);
        }


        public void SaveTasks(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var task in tasks)
                {
                    string line = $"{task.Description}|{task.StartDate:O}|{task.EndDate:O}|{task.Tag}|{task.Status}";
                    writer.WriteLine(line);
                }
            }
        }

        public void LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                tasks.Clear();
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 5)
                    {
                        tasks.Add(new TaskRecord
                        {
                            Description = parts[0],
                            StartDate = DateTime.Parse(parts[1]),
                            EndDate = DateTime.Parse(parts[2]),
                            Tag = parts[3],
                            Status = parts[4]
                        });
                    }
                }
            }
        }
    }

    public class TaskRecord
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Tag { get; set; }
        public string Status { get; set; }
    }



}
