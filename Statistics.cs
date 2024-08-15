﻿//---------------------------------------------------------------------------------
// Application: Focus
// Description: A Pomodoro Timer application based on the work of  Ayooluwa Isaiah
// Created By : Paul J Manoogian, Manoogian Media, Inc.
// Created    : 2024-Aug-13
// Modified   : 2024-Aug-15
// Language   : C#
// File       : Statistics.cs
// Notes      : Supports the statistics (stats) command line function
//---------------------------------------------------------------------------------



using System;
using System.Collections.Generic;

namespace FocusApp
{
    public class Statistics
    {
        private TaskManager _taskManager;
        private PomodoroTimer _timer;

        public Statistics(TaskManager taskManager, PomodoroTimer timer)
        {
            _taskManager = taskManager;
            _timer = timer;
        }

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

        public void ShowStatisticsByDateRange(DateTime startDate, DateTime? endDate = null)
        {
            List<TaskRecord> tasksInRange = _taskManager.GetTasksByDateRange(startDate, endDate);
            DisplayStatisticsTable(tasksInRange);
        }

        private void DisplayStatisticsTable(List<TaskRecord> tasks)
        {
            Console.WriteLine("┌──────────────────────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("| #   | TASK NAME           | START DATE            | END DATE              | TAGGED       | STATUS    |");
            Console.WriteLine("├──────────────────────────────────────────────────────────────────────────────────────────────────────┤");

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var taskcounter = i + 1;
                string taskName = task.Description.PadRight(19);
                string startDate = task.StartDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20);
                string endDate = task.EndDate == DateTime.MinValue ? "N/A".PadRight(20) : task.EndDate.ToString("MMM dd, yyyy hh:mm tt").PadRight(20);
                string tagged = string.IsNullOrEmpty(task.Tag) ? "".PadRight(12) : task.Tag.PadRight(12);
                string status = task.Status.PadRight(8);

                Console.WriteLine($"| {taskcounter.ToString().PadRight(3)} | {taskName} | {startDate} | {endDate} | {tagged} | {status} |");
            }

            Console.WriteLine("└──────────────────────────────────────────────────────────────────────────────────────────────────────┘");
        }
    }


}
