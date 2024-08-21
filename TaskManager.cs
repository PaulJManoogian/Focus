//---------------------------------------------------------------------------------
// Application: Focus
// Description: A Pomodoro Timer application based on the work of Ayooluwa Isaiah
// Created By : Paul J Manoogian, Manoogian Media, Inc.
// Created    : 2024-Aug-13
// Modified   : 2024-Aug-18
// Language   : C#
// File       : TaskManager.cs
// Notes      : Data file details management of time, date, desc, and status
//---------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Web;
using System.Threading.Tasks;


/*! 
 *  \brief     Task Manager Class.
 *  \details   Task Manager Class: This class is used to manage all timer functions.
 *  \author    Paul J Manoogian
 *  \author    Manoogian Media, Inc.
 *  \version   v1.0.0.0
 *  \date      2024-Aug-21
 *  \pre       First initialize the system.
 *  \bug       TaskManager: None
 *  \warning   Improper use of JSON without the Newtonsoft DLL will crash the application.
 *  \copyright (c) 2024 Manoogian Media, Inc.
 */
namespace FocusApp
{
    public class TaskManager
    {
        private List<TaskRecord> tasks = new List<TaskRecord>();

        public List<TaskRecord> Tasks
        {
            get { return tasks; }
        }

        // ********************************************************************************
        /// <summary>
        /// AddTask creates a task with just project information and sets it to Pending.
        /// </summary>
        /// <param name="description"> : Describes the task item</param>
        /// <param name="tag"> : A searchable tag for grouping elements</param>
        /// <param name="project"> : The Project name to associate the task</param>
        /// <param name="client"> : The client name or code to group the tasks</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void AddTask(string description, string tag = "", string project = "", string client = "")
        {
            var task = new TaskRecord
            {
                Description = description,
                StartDate = DateTime.Now,
                EndDate = DateTime.MinValue,
                Tag = tag,
                Status = "Pending",
                Project = project,  // Set the Project
                Client = client     // Set the Client
            };
            tasks.Add(task);
            SaveTasks("tasks.txt");
        }

        // ********************************************************************************
        /// <summary>
        /// AddTaskWithTime creates a task in the tasks file without starting a timer.
        /// but does add a time start entry.
        /// </summary>
        /// <param name="description"> : Describes the task item</param>
        /// <param name="tag"> : A searchable tag for grouping elements</param>
        /// <param name="startTime"> : The starting time (automatically entered)</param>
        /// <param name="endTime"> : The end time (current end time and 'Completed')</param>
        /// <param name="project"> : The Project name to associate the task</param>
        /// <param name="client"> : The client name or code to group the tasks</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void AddTaskWithTime(string description, string tag, DateTime startTime, DateTime endTime, string project = "", string client = "")
        {
            var task = new TaskRecord
            {
                Description = description,
                StartDate = startTime,
                EndDate = endTime,
                Tag = tag,
                Status = "Completed",
                Project = project,  // Set the Project
                Client = client     // Set the Client
            };
            tasks.Add(task);
            SaveTasks("tasks.txt");
        }


        // ********************************************************************************
        /// <summary>
        /// CompleteTask: Mark the task Completed as indicated by the index number of the task
        /// </summary>
        /// <param name="taskIndex"> : Index number of the item in the task list</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void CompleteTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks[taskIndex].EndDate = DateTime.Now;
                tasks[taskIndex].Status = "Completed";
            }
            SaveTasks("tasks.txt");
        }

        // ********************************************************************************
        /// <summary>
        /// AbandonTask: Mark the task Abandoned as indicated by the index number of the task
        /// </summary>
        /// <param name="taskIndex"> : Index number of the item in the task list</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void AbandonTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks[taskIndex].EndDate = DateTime.Now;
                tasks[taskIndex].Status = "Abandoned";
            }
        }

        // ********************************************************************************
        /// <summary>
        /// DeleteTask: Remove the task (delete it) as indicated by the index number of the task
        /// </summary>
        /// <param name="taskIndex"> : Index number of the item in the task list</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void DeleteTask(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks.RemoveAt(taskIndex);
            }
            SaveTasks("tasks.txt");
        }

        // ********************************************************************************
        /// <summary>
        /// ListTasks: Displays (in a chart) the tasks in the task file in the order they were added
        /// </summary>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ListTasks()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("| #   | TASK NAME            | START DATE            | END DATE              | TAGGED       | STATUS     | PROJECT      | CLIENT       | TIME SPENT   |");
            Console.WriteLine("├─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┤");

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var taskcounter = i + 1;
                
                string taskName = task.Description.PadRight(20);
                string startDate = task.StartDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20);
                string endDate = task.EndDate == DateTime.MinValue ? "N/A".PadRight(20) : task.EndDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20);
                string tagged = string.IsNullOrEmpty(task.Tag) ? "".PadRight(12) : task.Tag.PadRight(12);
                string status = task.Status.PadRight(10);
                string project = task.Project.PadRight(12);  // Display the Project
                string client = task.Client.PadRight(12);    // Display the Client

                // Calculate the time spent if the task is completed
                string timeSpent = task.EndDate == DateTime.MinValue
                    ? "N/A".PadRight(12)
                    : (task.EndDate - task.StartDate).ToString(@"hh\:mm\:ss").PadRight(12);

                Console.WriteLine($"| {taskcounter.ToString().PadRight(3)} | {taskName} | {startDate} | {endDate} | {tagged} | {status} | {project} | {client} | {timeSpent} |");

            }

            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘");
        }

        // ********************************************************************************
        /// <summary>
        /// CompletedToday For the -p parameter, list the task record completed with today's date
        /// </summary>
        /// <returns>Returns all tasks completed that have today's date</returns>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public List<TaskRecord> GetTasksCompletedToday()
        {
            DateTime today = DateTime.Today;
            return tasks.FindAll(task => task.Status == "Completed" && task.EndDate.Date == today);
        }

        // ********************************************************************************
        /// <summary>
        /// ComplatedAllTime For the -p parameter, list all of the completed task records
        /// </summary>
        /// <returns>Returns all tasks completed</returns>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public List<TaskRecord> GetTasksCompletedAllTime()
        {
            return tasks.FindAll(task => task.Status == "Completed");
        }

        // ********************************************************************************
        /// <summary>
        /// DateRange For the --start and --end parameters, list all of the completed task records
        /// </summary>
        /// <returns>Returns all tasks completed within the date range</returns>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public List<TaskRecord> GetTasksByDateRange(DateTime startDate, DateTime? endDate = null)
        {
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            return tasks.FindAll(task => task.StartDate >= startDate && task.StartDate <= endDate);
        }


        // ********************************************************************************
        /// <summary>
        /// Saves the task file
        /// </summary>
        /// <param name="filePath"> : The full file path and file name of the tasks file</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void SaveTasks(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var task in tasks)
                {
                    string line = $"{task.Description}|{task.StartDate:O}|{task.EndDate:O}|{task.Tag}|{task.Status}|{task.Project}|{task.Client}";
                    writer.WriteLine(line);
                }
            }
        }


        // ********************************************************************************
        /// <summary>
        /// Loads the tasks file from the full file path
        /// </summary>
        /// <param name="filePath">: The full file path and file name of the tasks file</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                tasks.Clear();
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 7)  // Updated to expect 7 parts
                    {
                        tasks.Add(new TaskRecord
                        {
                            Description = parts[0],
                            StartDate = DateTime.Parse(parts[1]),
                            EndDate = DateTime.Parse(parts[2]),
                            Tag = parts[3],
                            Status = parts[4],
                            Project = parts[5],  // Load the Project
                            Client = parts[6]    // Load the Client
                        });
                    }
                }
            }
        }


        // ********************************************************************************
        /// <summary>
        /// Exports the full task list data to a CSV format fie based on the file path name
        /// </summary>
        /// <param name="filePath"> : Full file name and path for the CSV export</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ExportTasksToCsv(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Description,StartDate,EndDate,Tag,Status,Project,Client");
                foreach (var task in tasks)
                {
                    string line = $"{task.Description},{task.StartDate:O},{task.EndDate:O},{task.Tag},{task.Status},{task.Project},{task.Client}";
                    writer.WriteLine(line);
                }
            }
        }


        // ********************************************************************************
        /// <summary>
        /// Exports the full task list data to a XML format fie based on the file path name
        /// </summary>
        /// <param name="filePath"> : Full file name and path for the XML export</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ExportTasksToXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TaskRecord>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, tasks);
                }
        }

        // ********************************************************************************
        /// <summary>
        /// Exports the full task list data serialized to a JSON format fie based on the file path name
        /// </summary>
        /// <param name="filePath"> : Full file name and path for the JSON export</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ExportTasksToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

    }


public class TaskRecord
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Tag { get; set; }
        public string Status { get; set; }
        public string Project { get; set; }  // New property for Project
        public string Client { get; set; }   // New property for Client
    }



}
