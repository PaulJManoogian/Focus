using System;
using System.Globalization;
using System.Media;
using System.Threading;
using System.Reflection;


namespace FocusApp
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0 && (args[0].Equals("help", StringComparison.OrdinalIgnoreCase) || args[0] == "?"))
            {
                DisplayHelp();
                return;
            }

            TaskManager taskManager = new TaskManager();
            PomodoroTimer timer = new PomodoroTimer();
            Statistics stats = new Statistics(taskManager, timer);

            // Load tasks from file
            taskManager.LoadTasks("tasks.txt");

            string taskDescription = null;
            string taskTag = null;
            int workInterval = 25;  // Default to 25 minutes
            int breakInterval = 5;  // Default to 5 minutes
            int sessions = 4;       // Default to 4 work sessions before a long break
            string soundSelection = "OFF";  // Default to no sound

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {

                    if (args[i] == "list")
                    {
                        taskManager.ListTasks();
                        return;
                    }

                    else if (args[i] == "countup")
                    {
                        taskDescription = args.Length > i + 1 ? args[i + 1] : "Untitled Task";
                        taskTag = args.Length > i + 2 ? args[i + 2] : "";
                        timer.SetIntervals(workInterval, breakInterval);
                        timer.SetSound(soundSelection);
                        timer.SetSessions(sessions);
                        timer.StartCountUpTimer(taskManager, taskDescription, taskTag);
                        return;
                    }
                    else if (args[i] == "stats")
                    {
                        DateTime? startDate = null;
                        DateTime? endDate = null;
                        bool showTodayStats = false;
                        bool showAllTimeStats = false;

                        for (int j = i + 1; j < args.Length; j++)
                        {
                            if (args[j] == "--start" && j + 1 < args.Length)
                            {
                                startDate = DateTime.Parse(args[j + 1], CultureInfo.InvariantCulture);
                            }
                            else if (args[j] == "--end" && j + 1 < args.Length)
                            {
                                endDate = DateTime.Parse(args[j + 1], CultureInfo.InvariantCulture);
                            }
                            else if (args[j] == "-p" && j + 1 < args.Length)
                            {
                                if (args[j + 1].ToLower() == "today")
                                {
                                    showTodayStats = true;
                                }
                                else if (args[j + 1].ToLower() == "all-time")
                                {
                                    showAllTimeStats = true;
                                }
                            }
                        }

                        if (showTodayStats)
                        {
                            stats.ShowStatistics("today");
                        }
                        else if (showAllTimeStats)
                        {
                            stats.ShowStatistics("all-time");
                        }
                        else if (startDate.HasValue)
                        {
                            stats.ShowStatisticsByDateRange(startDate.Value, endDate);
                        }
                        else
                        {
                            Console.WriteLine("Please provide a valid --start date or use -p 'today' or -p 'all-time'.");
                        }
                        return;
                    }
                    else if (args[i] == "-w" && i + 1 < args.Length)
                    {
                        workInterval = int.Parse(args[i + 1]);
                    }
                    else if (args[i] == "-b" && i + 1 < args.Length)
                    {
                        breakInterval = int.Parse(args[i + 1]);
                    }
                    else if (args[i] == "-s" && i + 1 < args.Length)
                    {
                        sessions = int.Parse(args[i + 1]);
                    }
                    else if (args[i] == "--task" && i + 1 < args.Length)
                    {
                        taskDescription = args[i + 1];
                    }
                    else if (args[i] == "--tag" && i + 1 < args.Length)
                    {
                        taskTag = args[i + 1];
                    }
                    else if (args[i] == "--sound" && i + 1 < args.Length)
                    {
                        soundSelection = args[i + 1].ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(taskDescription))
                {
                    taskManager.AddTask(taskDescription, taskTag);
                    Console.WriteLine($"Task '{taskDescription}' with tag '{taskTag}' added.");
                    timer.SetIntervals(workInterval, breakInterval);
                    timer.SetSound(soundSelection);
                    timer.SetSessions(sessions);
                    timer.Start();
                    taskManager.CompleteTask(taskManager.Tasks.Count - 1);  // Completing the most recent task
                    taskManager.SaveTasks("tasks.txt");
                    return;
                }
            }

            // Regular program flow...
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" _____   ___     __  __ __  _____");
                Console.WriteLine("|     | /   \\   /  ]|  |  |/ ___/");
                Console.WriteLine("|   __||     | /  / |  |  (   \\_ ");
                Console.WriteLine("|  |_  |  O  |/  /  |  |  |\\__  |");
                Console.WriteLine("|   _] |     /   \\_ |  :  |/  \\ |");
                Console.WriteLine("|  |   |     \\     ||     |\\    |");
                Console.WriteLine("|__|    \\___/ \\____| \\__,_| \\___|");
                Console.WriteLine("                                 ");
                Console.ResetColor();
                Console.WriteLine("");

                // Get the assembly that contains this code
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get the assembly title
                AssemblyTitleAttribute title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
                string appName = title?.Title ?? "Unknown";

                // Get the version
                Version version = assembly.GetName().Version;
                string versionNumber = version != null ? version.ToString() : "Unknown";

                // Get the description
                AssemblyDescriptionAttribute Appdescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                string appDescription = Appdescription?.Description ?? "No description available.";

                // Get the build number (AssemblyFileVersion)
                AssemblyFileVersionAttribute fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
                string buildNumber = fileVersion?.Version ?? "Unknown";

                // Get the copyright information
                AssemblyCopyrightAttribute copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                string appCopyright = copyright?.Copyright ?? "No copyright information available.";



                // Display the information
                Console.WriteLine($"-={appName}=-");
                Console.WriteLine($"Version: {versionNumber}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Build: {buildNumber}");
                Console.WriteLine($"Description: {appDescription}");
                Console.WriteLine($"Copyright: {appCopyright}");
                Console.WriteLine();

                //Console.WriteLine("-=Focus=-");
                //Console.WriteLine("v1.0.0.1");

                //Console.WriteLine("(c)2024 Manoogian Media, Inc.");
                //Console.WriteLine("Based off of 'Focus' by Ayooluwa Isaiah");
                Console.ResetColor();
                Console.WriteLine("");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. List Tasks");
                Console.WriteLine("3. Complete Task");
                Console.WriteLine("4. Abandon Task");
                Console.WriteLine("5. Delete Task");
                Console.WriteLine("6. Start Pomodoro Timer");
                Console.WriteLine("7. Start Count-Up Timer");
                Console.WriteLine("8. Exit");
                Console.WriteLine("");
                Console.Write("Select an option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        Console.Write("Enter tag (optional): ");
                        string tag = Console.ReadLine();
                        taskManager.AddTask(description, tag);
                        break;
                    case "2":
                        taskManager.ListTasks();
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Write("Enter task number to complete: ");
                        int completeTaskIndex = int.Parse(Console.ReadLine()) - 1;
                        taskManager.CompleteTask(completeTaskIndex);
                        break;
                    case "4":
                        Console.Write("Enter task number to abandon: ");
                        int abandonTaskIndex = int.Parse(Console.ReadLine()) - 1;
                        taskManager.AbandonTask(abandonTaskIndex);
                        break;
                    case "5":
                        Console.Write("Enter task number to delete: ");
                        int deleteTaskIndex = int.Parse(Console.ReadLine()) - 1;
                        taskManager.DeleteTask(deleteTaskIndex);
                        break;
                    case "6":
                        Console.Write("Enter work interval in minutes (default 25): ");
                        int wInterval = int.TryParse(Console.ReadLine(), out wInterval) ? wInterval : 25;
                        Console.Write("Enter break interval in minutes (default 5): ");
                        int bInterval = int.TryParse(Console.ReadLine(), out bInterval) ? bInterval : 5;
                        Console.Write("Enter number of sessions before long break (default 4): ");
                        int sessionCount = int.TryParse(Console.ReadLine(), out sessionCount) ? sessionCount : 4;
                        Console.Write("Select sound (1: coffee_shop, 2: rainforest, 3: wind, 4: rain, 5: summer_night, 6: fireplace, 7: frogs, 8: bird_rain, OFF): ");
                        string sound = Console.ReadLine().ToUpper();
                        timer.SetIntervals(wInterval, bInterval);
                        timer.SetSound(sound);
                        timer.SetSessions(sessionCount);
                        timer.Start();
                        break;
                    case "7":
                        Console.Write("Enter task description: ");
                        string countUpDescription = Console.ReadLine();
                        Console.Write("Enter tag (optional): ");
                        string countUpTag = Console.ReadLine();
                        Console.Write("Enter work interval in minutes (default 25): ");
                        int cuwInterval = int.TryParse(Console.ReadLine(), out wInterval) ? wInterval : 25;
                        Console.Write("Enter break interval in minutes (default 5): ");
                        int cubInterval = int.TryParse(Console.ReadLine(), out bInterval) ? bInterval : 5;
                        Console.Write("Enter number of sessions before long break (default 4): ");
                        int cusessionCount = int.TryParse(Console.ReadLine(), out sessionCount) ? sessionCount : 4;
                        Console.Write("Select sound (1: coffee_shop, 2: rainforest, 3: wind, 4: rain, 5: summer_night, 6: fireplace, 7: frogs, 8: bird_rain, OFF): ");
                        string cusound = Console.ReadLine().ToUpper();

                        timer.SetIntervals(cuwInterval, cubInterval);
                        timer.SetSound(cusound);
                        timer.SetSessions(cusessionCount);
                        timer.StartCountUpTimer(taskManager, countUpDescription, countUpTag);
                        break;
                    case "8":
                        taskManager.SaveTasks("tasks.txt");
                        return;
                }
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Focus Application - Command Line Options:");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("help or ?             : Display this help screen");
            Console.WriteLine("list                  : List all tasks");
            Console.WriteLine("countup [task] [tag]  : Start a count-up timer. Press F10 to stop and log the task.");
            Console.WriteLine("-w [minutes]          : Set the work interval in minutes (default 25)");
            Console.WriteLine("-b [minutes]          : Set the break interval in minutes (default 5)");
            Console.WriteLine("-s [sessions]         : Set the number of work sessions before a long break (default 4)");
            Console.WriteLine("--task [description]  : Set the task description");
            Console.WriteLine("--tag [tag]           : Set a tag for the task (optional)");
            Console.WriteLine("--sound [option]      : Set the ambient sound (1-8 or OFF)");
            Console.WriteLine("                       1: coffee_shop, 2: rainforest, 3: wind");
            Console.WriteLine("                       4: rain, 5: summer_night, 6: fireplace, 7: frogs, 8: bird_rain");
            Console.WriteLine("stats                 : Display task statistics");
            Console.WriteLine("  --start [date]      : Specify the start date for stats (format: yyyy-MM-dd)");
            Console.WriteLine("  --end [date]        : Specify the end date for stats (format: yyyy-MM-dd)");
            Console.WriteLine("-p [today|all-time]   : Show statistics for today or all time");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("Focus -w 20 -b 5 -s 4 --task \"Work Entry\" --tag \"Tag Entry\"");
            Console.WriteLine("Focus stats --start '2021-08-06' --end '2021-08-07'");
        }
    }


}
