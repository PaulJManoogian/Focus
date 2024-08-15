# Focus
A timer tool based on the work of Ayooluwa Isaiah

The functionality of this tool is nearly identical to the work created in this repository:
[Focus](https://github.com/ayoisaiah/focus)

## Purpose
I created this version of the tool to have a C# version which will run on a Windows machine. The code will run only in console mode (just to keep the application simple), and offers a menu system and command line functionality.

Using the tool is simple. Here are some examples:

```
Focus -w 20 -b 5 -s 4 --task "Work Entry" --tag "Tag Entry"
Focus stats --start '2021-08-06' --end '2021-08-07'
Focus help
Focus list
```

If you type "help" or "?" on the command line, you'll see all of the command line options.
If you only type FOCUS, it will bring up a menu version of the application.

Menu version:
![image](https://github.com/user-attachments/assets/89ea320e-6a3b-456c-98b1-e99bbdc6b9f5)

Command line arguments:
![image](https://github.com/user-attachments/assets/2d59d926-2a5a-4a06-8bca-788acedfd6f5)

```
help or ?             : Display this help screen

list                  : List all tasks

-w [minutes]          : Set the work interval in minutes (default 25)

-b [minutes]          : Set the break interval in minutes (default 5)

-s [sessions]         : Set the number of work sessions before a long break (default 4)

--task [description]  : Set the task description

--tag [tag]           : Set a tag for the task (optional)

--sound [option]      : Set the ambient sound (1-8 or OFF)
                       1: coffee_shop, 2: rainforest, 3: wind
                       4: rain, 5: summer_night, 6: fireplace, 7: frogs, 8: bird_rain
                       
stats                 : Display task statistics
  --start [date]      : Specify the start date for stats (format: yyyy-MM-dd)
  --end [date]        : Specify the end date for stats (format: yyyy-MM-dd)
-p [today|all-time]   : Show statistics for today or all time


Examples:

Focus -w 20 -b 5 -s 4 --task "Work Entry" --tag "Tag Entry"

Focus stats --start '2021-08-06' --end '2021-08-07'
```

## Details
This solution is a quick and simple way of watching the time you are working on a project. The intention of the application isn't to try to be a substitute for more robust solutions like Toggl, but to be a simple version that is easy to use and manage.

### Let's start with STATS.
The `stats` option is specific to the command line. You currently (v1.0.0.0) cannot see the statistics in the menu version of the application. Soon.
However, the `list` option lets you see the full list of task entries and you can also use option 2 on the menu to see the same list in app mode.

***stats*** offers some specific command line options, listed above, that let you specify start and end dates to view as well as just `today` or `all-time` stats. Those are based on `Completed` tasks.

### Sounds
The sounds option on the command line, and when you elect to `start a timer` in the menu mode, lets you choose an ambient sound to play while you're using the timer. As a note: it will stop during the breaks.

There is a Work-stop sound and a Break-Stop sound, as well. These will play when your sections complete.

Sounds vary from chirping birds to rain or fireplace noises. All sounds are gentle, mostly white-noise-style, sounds to help you **Focus**. They will loop continually. They are part of the executable and don't require any other files. Currently (v1.0.0.0) you cannot play external sounds.

## Tags and Tasks
`Tags` are just as you'd expect, they help you define customers, areas, or lists of similar things to help you sort through the projects. Currently, Focus doesn't support filtering just lists of tags, but ... soon.
`Tasks` are the full names of your entries. These help you determine exact project tasks that you worked on.

Time entries are added to the tasks when they are created, and the end times are specified on the menu mode of the application for completion of the tasks.

## Timers
**Timers** are made up of three components, based on using a Pomodoro Timer: 
`Work Minutes` - how long your work time is.
`Break minutes` - how long your Pomodoro break is.
`Sessions` - How many Work and Break groupings before a long break (which is 3 times the Break minutes; a `Long Break`).

You specify the time and amount for each of these when starting the timer in the menu mode, or with the command line options above.

-=-=-=-=-=-=-=-=-

![Image](https://img.shields.io/badge/CSharp-Release-Green?style=plastic)
