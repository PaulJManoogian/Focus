using System;
using System.Media;
using System.Threading;


namespace FocusApp
{
    public class PomodoroTimer
    {
        private int WorkInterval { get; set; } = 25 * 60; // Default to 25 minutes
        private int BreakInterval { get; set; } = 5 * 60; // Default to 5 minutes
        private int SessionsBeforeLongBreak { get; set; } = 4; // Default to 4 sessions before long break
        private string SoundSelection { get; set; } = "OFF";

        private SoundPlayer ambientSoundPlayer;
        private SoundPlayer workEndSoundPlayer;
        private SoundPlayer breakEndSoundPlayer;

        public void SetIntervals(int workMinutes, int breakMinutes)
        {
            WorkInterval = workMinutes * 60;
            BreakInterval = breakMinutes * 60;
        }

        public void SetSound(string soundSelection)
        {
            SoundSelection = soundSelection;

            switch (SoundSelection)
            {
                case "1":
                case "COFFEE_SHOP":
                    ambientSoundPlayer = LoadSound("Focus.Resources.coffee_shop.wav");
                    break;
                case "2":
                case "RAINFOREST":
                    ambientSoundPlayer = LoadSound("Focus.Resources.rainforest.wav");
                    break;
                case "3":
                case "WIND":
                    ambientSoundPlayer = LoadSound("Focus.Resources.wind.wav");
                    break;
                case "4":
                case "RAIN":
                    ambientSoundPlayer = LoadSound("Focus.Resources.rain.wav");
                    break;
                case "5":
                case "SUMMER_NIGHT":
                    ambientSoundPlayer = LoadSound("Focus.Resources.summer_night.wav");
                    break;
                case "6":
                case "FIREPLACE":
                    ambientSoundPlayer = LoadSound("Focus.Resources.fireplace.wav");
                    break;
                case "7":
                case "FROGS":
                    ambientSoundPlayer = LoadSound("Focus.Resources.frogs.wav");
                    break;
                case "8":
                case "BIRD_RAIN":
                    ambientSoundPlayer = LoadSound("Focus.Resources.bird_rain.wav");
                    break;
                default:
                    ambientSoundPlayer = null;
                    break;
            }

            // Load the notification sounds (assuming they are in the resources)
            workEndSoundPlayer = LoadSound("Focus.Resources.work_end.wav");
            breakEndSoundPlayer = LoadSound("Focus.Resources.break_end.wav");
        }

        //private SoundPlayer LoadSound(string resourceName)
        //{
        //    var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        //    var stream = assembly.GetManifestResourceStream(resourceName);
        //    return stream != null ? new SoundPlayer(stream) : null;
        //}

        private SoundPlayer LoadSound(string resourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to load resource: {resourceName}");
                Console.ResetColor();
                Console.ReadLine();
                return null;
            }
            return new SoundPlayer(stream);
        }

        public void SetSessions(int sessions)
        {
            SessionsBeforeLongBreak = sessions;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Starting Pomodoro Timer...");

            for (int session = 1; session <= SessionsBeforeLongBreak; session++)
            {
                if (ambientSoundPlayer != null)
                {
                    ambientSoundPlayer.PlayLooping();
                }

                Console.WriteLine($"Work Session {session} of {SessionsBeforeLongBreak}");
                TimerCountdown("Work Time", WorkInterval);

                if (SoundSelection != "OFF" && workEndSoundPlayer != null)
                {
                    workEndSoundPlayer.Play();
                }

                if (ambientSoundPlayer != null)
                {
                    ambientSoundPlayer.Stop();
                }

                if (session < SessionsBeforeLongBreak)
                {
                    Console.ForegroundColor= ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Time for a short break!");
                    Console.ResetColor();
                    TimerCountdown("Break Time", BreakInterval);

                    if (SoundSelection != "OFF" && breakEndSoundPlayer != null)
                    {
                        breakEndSoundPlayer.Play();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Time for a long break!");
                    Console.ResetColor();
                    TimerCountdown("Long Break Time", BreakInterval * 3); // Example: Long break is 3 times the short break
                    if (SoundSelection != "OFF" && breakEndSoundPlayer != null)
                    {
                        breakEndSoundPlayer.Play();
                    }
                }
            }
            Console.ReadLine();
        }

        private void TimerCountdown(string phase, int seconds)
        {
            Console.Write($"{phase}: ");
            TimeSpan TotalTime = TimeSpan.FromSeconds(seconds);
            while (seconds > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(seconds);
                Console.Write($"\r{phase}: {time:mm\\:ss}");
                Thread.Sleep(1000);
                seconds--;
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine($"\r{phase}: {TotalTime:mm\\:ss} Completed.");
            Console.ResetColor();
            Console.WriteLine(); // Move to the next line after countdown completes
        }

        public void StartCountUpTimer(TaskManager taskManager, string taskDescription, string taskTag)
        {
            Console.Clear();
            Console.WriteLine("Starting Count-Up Timer...");
            Console.WriteLine("Press F10 to stop the timer and log the task.");

            DateTime startTime = DateTime.Now;
            int workSessionCount = 0;
            int seconds = 0;

            if (ambientSoundPlayer != null)
            {
                ambientSoundPlayer.PlayLooping();  // Resume ambient sound
            }
            else
            {
                Console.WriteLine("Can't load ambient sounds.");
            }


            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.F10)
            {

                // Handle the work session
                if (seconds % WorkInterval == 0 && seconds > 0)
                {
                    workSessionCount++;


                    if (SoundSelection != "OFF" && workEndSoundPlayer != null)
                    {
                        workEndSoundPlayer.PlaySync();  // Play end of work session sound
                    }

                    // Handle breaks
                    if (workSessionCount < SessionsBeforeLongBreak)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Time for a short break (Session {workSessionCount} of {SessionsBeforeLongBreak})!");
                        TimerCountdown("Break Time", BreakInterval);

                        if (SoundSelection != "OFF" && breakEndSoundPlayer != null)
                        {
                            breakEndSoundPlayer.PlaySync();  // Play end of break sound
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Time for a long break!");
                        TimerCountdown("Long Break Time", BreakInterval * 3);

                        if (SoundSelection != "OFF" && breakEndSoundPlayer != null)
                        {
                            breakEndSoundPlayer.PlaySync();  // Play end of break sound
                        }

                        // Reset the session count after a long break
                        workSessionCount = 0;
                    }


                }

                TimeSpan timeElapsed = TimeSpan.FromSeconds(seconds);
                Console.Write($"\rElapsed Time: {timeElapsed:hh\\:mm\\:ss}");
                Thread.Sleep(1000);
                seconds++;
            }

            if (ambientSoundPlayer != null)
            {
                ambientSoundPlayer.Stop(); // stop playing ambient sounds when we stop the timer
            }

            Console.WriteLine();
            DateTime endTime = DateTime.Now;
            Console.WriteLine($"Task '{taskDescription}' completed. Total time: {TimeSpan.FromSeconds(seconds):hh\\:mm\\:ss}");

            // Log the task
            taskManager.AddTaskWithTime(taskDescription, taskTag, startTime, endTime);
        }
    }
}

