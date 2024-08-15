# Focus
A timer tool based on the work of [Ayooluwa Isaiah](https://github.com/ayoisaiah)

The functionality of this tool is nearly identical to the work created in this repository:
[Focus](https://github.com/ayoisaiah/focus)

**This version (c)2024 Manoogian Media, Inc.**

## Purpose
I created this version of the tool to have a C# version which will run on a Windows machine. The code runs only in console mode (just to keep the application simple), but offers a menu system and command line functionality. The data file where the tasks are stored is called `tasks.txt`. The intention here is to keep the entire solution simple and easy to update and maintain.

## Installation
This product only runs on Windows as a console/command line executable. It requires *no DLLs* and is completely self-contained in the [Release version 1.0.0.0](https://github.com/PaulJManoogian/Focus/releases/tag/v1.0.0.0) listed to the right.

Simply **download the executable and run it**. *IT IS NOT SIGNED.*

-----

## Usage
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

```
help or ?             : Display this help screen

list                  : List all tasks

countup [desc] [tag]  : Start a timer that counts up, with support for pomodoro and sounds

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

### Command Line Usage
When using `list` or `stats` you cannot use any of the other command line options. They will be ignored or an error will be thrown.
- `list` is exclusive to simply listing all of the entries in the tasks.txt file.
- `stats` is used with either a `-p` (time period) as words or with the `--start` and `--end` date filters. You can use the `--start` date without specifying the end date.
- All other arguments can be used together (as in the first example above). This includes the `countup` argument to start at count-up timer instead of a countdown timer. Otherwise, ***Focus*** defaults to countdown.

-----

## Details
This solution is a quick and simple way of watching the time you are working on a project. The intention of the application isn't to try to be a substitute for more robust solutions like Toggl, but to be a simple version that is easy to use and manage.

### Stats
The `stats` option is specific to the command line. You currently (v1.0.0.0) cannot see the statistics in the menu version of the application. Soon.

However, the `list` option lets you see the full list of task entries and you can also use option 2 on the menu to see the same list in app mode.

***stats*** offers some specific command line options, listed above, that let you specify start and end dates to view as well as just `today` or `all-time` stats. Those are based on `Completed` tasks.

### Sounds
The sounds option on the command line, and when you elect to `start a timer` in the menu mode, lets you choose an ambient sound to play while you're using the timer. As a note: it will stop during the breaks.

There is a Work-stop sound and a Break-Stop sound, as well. These will play when your sections complete.

Sounds vary from chirping birds to rain or fireplace noises. All sounds are gentle, mostly white-noise-style, sounds to help you ***Focus***. They will loop continually. They are part of the executable and don't require any other files. Currently (v1.0.0.0) you cannot play external sounds.

## Tags and Tasks
- `Tags` are just as you'd expect, they help you define customers, areas, or lists of similar things to help you sort through the projects. Currently, Focus doesn't support filtering just lists of tags, but ... soon.
- `Tasks` are the full names of your entries. These help you determine exact project tasks that you worked on.

Time entries are added to the tasks when they are created, and the end times are specified on the menu mode of the application for completion of the tasks.

## Timers
**Timers** are made up of three components, based on using a Pomodoro Timer: 
- `Work Minutes` - how long your work time is.
- `Break minutes` - how long your Pomodoro break is.
- `Sessions` - How many Work and Break groupings before a long break (which is 3 times the Break minutes; a `Long Break`).

You specify the time and amount for each of these when starting the timer in the menu mode, or with the command line options above.

Countdown versus Count-Up timers:

- `Countdown` - uses the pomodoro interval, break minutes, and sessions to count time downward until it runs out. Does not complete entries.
- `Count-up` - uses the pomodoro interval, break minutes, and sessions to count the time upward until the user hits F10 to stop the timer. When the user presses F10, the entry is marked `Completed`.

-----

## The TASKS.TXT file format

Fields in the file are separated by the `|` (pipe) character.

The format is as follows:
`task_name|start_time|end_time|tags|status`

- `task_name` Name of the task
- `start_time` Full UTC time coding for date, time, and GMT offset for when the task was created (started)
- `end_time` Full UTC time coding for date, time, and GMT offset for when the task was marked complete (completed)
- `tags` Any tags to help identify the item so that it can be grouped with similar items
- `status` One of three possible status notations: `Pending` (new or working item), `Completed` (completed work item), `Abandoned` (dead or deleted task item)

 Items that don't have a status of "Completed" do not show up on the "stats" display at the command line. 

-----

![Image](https://img.shields.io/badge/CSharp-Release-Green?style=plastic)
