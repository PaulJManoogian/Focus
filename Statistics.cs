//---------------------------------------------------------------------------------
// Application: Focus
// Description: A Pomodoro Timer application based on the work of Ayooluwa Isaiah
// Created By : Paul J Manoogian, Manoogian Media, Inc.
// Created    : 2024-Aug-13
// Modified   : 2024-Aug-15
// Language   : C#
// File       : Statistics.cs
// Notes      : Supports the statistics (stats) command line function
//---------------------------------------------------------------------------------



using System;
using System.Collections.Generic;

/*! 
 *  \brief     Statistics Class.
 *  \details   Statistics Class: This class is used to create a chart for the details of the tasks.
 *  \author    Paul J Manoogian
 *  \author    Manoogian Media, Inc.
 *  \version   v1.0.0.0
 *  \date      2024-Aug-21
 *  \pre       First initialize the system.
 *  \bug       Statistics: None
 *  \warning   Improper use of JSON without the Newtonsoft DLL will crash the application.
 *  \copyright (c) 2024 Manoogian Media, Inc.
 */
namespace FocusApp
{
    public class Statistics
    // ********************************************************************************
    /// <summary>
    /// Application: Focus. Statistics Module.  
    /// Description: Statistics Details Visuals. 
    /// Notes      : Shows statistics based on the "stats" command line option.
    /// </summary>
    // <created>PJM,8/14/2024</created>
    // <changed>PJM,8/21/2024</changed>
    // ********************************************************************************
    {

        private TaskManager _taskManager;
        private PomodoroTimer _timer;

        // ********************************************************************************
        /// <summary>
        /// Call to the Statistics Task Manager and Timer
        /// </summary>
        /// <param name="taskManager">Starts the task manager</param>
        /// <param name="timer">Starts the timer</param>
        /// <returns></returns>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public Statistics(TaskManager taskManager, PomodoroTimer timer)
        {
            _taskManager = taskManager;
            _timer = timer;
        }

        // ********************************************************************************
        /// <summary>
        /// ShowStatistics: Shows the statistical data based on the word for the time period
        /// </summary>
        /// <param name="period"> : Word based task display for reporting</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ShowStatistics(string period)
        {
            if (period.ToLower() == "today")
            {
                List<TaskRecord> tasksToday = _taskManager.GetTasksCompletedToday();
                DisplayStatisticsTable(tasksToday);
            }
            else if (period.ToLower() == "all-time")
            {
                List<TaskRecord> allTasks = _taskManager.GetTasksCompletedAllTime();
                DisplayStatisticsTable(allTasks);
            }
        }


        // ********************************************************************************
        /// <summary>
        /// Statistics By Date Range: Shows the chart of information based on the date range
        /// </summary>
        /// <param name="startDate"> : Starting date for the date range</param>
        /// <param name="endDate"> : Ending date for the date range</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void ShowStatisticsByDateRange(DateTime startDate, DateTime? endDate = null)
        {
            List<TaskRecord> tasksInRange = _taskManager.GetTasksByDateRange(startDate, endDate);
            DisplayStatisticsTable(tasksInRange);
        }

        // ********************************************************************************
        /// <summary>
        /// Statistics Table: Display the table of data with the tasks record
        /// </summary>
        /// <param name="tasks">Requested tasks from Statistics table</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        private void DisplayStatisticsTable(List<TaskRecord> tasks)
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
    }


}
