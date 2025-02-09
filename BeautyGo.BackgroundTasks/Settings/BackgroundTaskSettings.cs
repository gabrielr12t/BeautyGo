﻿namespace BeautyGo.BackgroundTasks.Settings;

public class BackgroundTaskSettings
{
    public const string SettingsKey = "BackgroundTasks";

    public int AllowedNotificationTimeDiscrepancyInMinutes { get; set; }

    public int AttendeesBatchSize { get; set; }

    public int PersonalEventsBatchSize { get; set; }

    public int NotificationsBatchSize { get; set; }

    public int SleepTimeInMilliseconds { get; set; }
}