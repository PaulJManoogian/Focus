//---------------------------------------------------------------------------------
// Application: Focus
// Description: A Pomodoro Timer application based on the work of Ayooluwa Isaiah
// Created By : Paul J Manoogian, Manoogian Media, Inc.
// Created    : 2024-Aug-13
// Modified   : 2024-Aug-16
// Language   : C#
// File       : PomodoroTimer.cs
// Notes      : All of the Pomodoro Timer functions and audio playback during work
//---------------------------------------------------------------------------------


using System;
using System.Media;
using System.Threading;

/*! 
 *  \brief     Pomodoro Timer Class.
 *  \details   Pomodoro Timer: This class is used to manage timer logic and play sounds while the timer counts up or down
 *  \author    Paul J Manoogian
 *  \author    Manoogian Media, Inc.
 *  \version   v1.0.0.0
 *  \date      2024-Aug-21
 *  \pre       Called from Program Class
 *  \bug       PomodoroTimer: None
 *  \warning   None
 *  \copyright (c) 2024 Manoogian Media, Inc.
 */
namespace FocusApp
{
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Pomodoro Timer Class Start
    /// </summary>
    // --------------------------------------------------------------------------------
    public class PomodoroTimer
    {

        /// <summary>WorkInterval: Timer value * 60 seconds to create minutes of work time. Default is 60.</summary>
        private int WorkInterval { get; set; } = 25 * 60; // Default to 25 minutes
        /// <summary>BreakInterval: Timer value * 60 for break minutes. Default is 5. </summary>
        private int BreakInterval { get; set; } = 5 * 60; // Default to 5 minutes
        /// <summary>SessionsBeforeLongBreak: Number of loops of the work and break intervals before longer break time. Default is 4. </summary>
        private int SessionsBeforeLongBreak { get; set; } = 4; // Default to 4 sessions before long break
        /// <summary>SoundSelection: What ambient sound number will be played for relaxation noise. Default is OFF.</summary>
        private string SoundSelection { get; set; } = "OFF";

        private SoundPlayer ambientSoundPlayer;
        private SoundPlayer workEndSoundPlayer;
        private SoundPlayer breakEndSoundPlayer;

        // ********************************************************************************
        /// <summary>
        /// Interval Minutes and Break Minutes. Each work session is based on these.
        /// </summary>
        /// <param name="workMinutes"></param>
        /// <param name="breakMinutes"></param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void SetIntervals(int workMinutes, int breakMinutes)
        {
            WorkInterval = workMinutes * 60;
            BreakInterval = breakMinutes * 60;
        }

        // ********************************************************************************
        /// <summary>
        /// SetSound: Sets the sound based on the selection from the command line or menu.
        /// </summary>
        /// <param name="soundSelection">Number or Name of the sound that will be played as ambient background sounds.</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
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


        // ********************************************************************************
        /// <summary>
        /// SoundPlayer: Loads the sounds that will be played. Based on the work end, break end, or ambient sounds.
        /// </summary>
        /// <param name="resourceName">File name of the resource that will be played.</param>
        /// <returns></returns>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
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

        // ********************************************************************************
        /// <summary>
        /// Number of sessions to loop with interval work and break timers.
        /// </summary>
        /// <param name="sessions"></param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
        public void SetSessions(int sessions)
        {
            SessionsBeforeLongBreak = sessions;
        }

        // ********************************************************************************
        /// <summary>
        /// Start: Starts the Pomodoro timer loop. Each phase is looped through here.
        /// </summary>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
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
                        if (ambientSoundPlayer != null)
                        {
                            ambientSoundPlayer.Stop();
                        }

                        workEndSoundPlayer.Play();
                    }



                    if (session < SessionsBeforeLongBreak)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Time for a short break!");
                            Console.ResetColor();
                            TimerCountdown("Break Time", BreakInterval);

                            if (SoundSelection != "OFF" && breakEndSoundPlayer != null)
                            {
                                if (ambientSoundPlayer != null)
                                {
                                    ambientSoundPlayer.Stop();
                                }

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
                                if (ambientSoundPlayer != null)
                                {
                                    ambientSoundPlayer.Stop();
                                }

                                breakEndSoundPlayer.Play();
                            }
                        }
                }
            
            Console.ReadLine();
        }

        // ********************************************************************************
        /// <summary>
        /// TimerCountdown: The actual count-down timer for the Pomodoro Timer interval
        /// </summary>
        /// <param name="phase">Writes the phase that the timer is in (Timer, break, session break)</param>
        /// <param name="seconds">Shows the time in minutes and seconds from the seconds amount</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
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

        // ********************************************************************************
        /// <summary>
        /// StartCountUpTimer: creates the counter which will start from zero and count up to accumulate time for the timer.
        /// Still functions as a Pomodoro Timer with the same work/break/session intervals.
        /// </summary>
        /// <param name="taskManager">Creates the timer task management instance</param>
        /// <param name="taskDescription">Creates the task that will be written to the task file</param>
        /// <param name="taskTag">Creates the tag to associate with this task for grouping</param>
        // <created>PJM,8/21/2024</created>
        // <changed>PJM,8/21/2024</changed>
        // ********************************************************************************
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Task '{taskDescription}' completed. Total time: {TimeSpan.FromSeconds(seconds):hh\\:mm\\:ss}");
            Console.ResetColor();

            // Log the task
            taskManager.AddTaskWithTime(taskDescription, taskTag, startTime, endTime);
        }
    }
}

