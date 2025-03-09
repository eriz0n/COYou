
using Android.App;
using Android.App.Job;
using Android.Content;
using global::Android.App.Job;
using global::Android.Content;
using System;
using Android.Util;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Android.OS;

namespace coyou.Platforms.Android.Helpers;

public static class JobSchedulerHelper
{
    private const int AlarmRequestCode = 1001; // Eindeutige Alarm-ID
    //public static void ScheduleWeeklyAlarm(Context context)
    //{
    //    AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
    //    Intent intent = new Intent(context, typeof(WeeklyAlarmReceiver));
    //    PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, AlarmRequestCode, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

    //    // Setze den Alarm auf 1 Minute nach der aktuellen Zeit
    //    DateTime now = DateTime.UtcNow;
    //    DateTime targetTime = now.AddSeconds(15); // Für Testzwecke: 1 Minute warten

    //    long triggerTime = (long)(targetTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

    //    Log.Info("JobSchedulerHelper", $"TEST-Alarm geplant für: {targetTime} UTC");

    //    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
    //    {
    //        alarmManager.SetAndAllowWhileIdle(AlarmType.RtcWakeup, triggerTime, pendingIntent);
    //    }
    //    else
    //    {
    //        alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
    //    }
    //}

    public static void ScheduleWeeklyAlarm(Context context)
    {
        AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
        Intent intent = new Intent(context, typeof(WeeklyAlarmReceiver));
        PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, AlarmRequestCode, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        // Bestimme den nächsten Sonntag um 12:00 UTC
        DateTime now = DateTime.UtcNow;
        DateTime nextSunday = now.AddDays((7 - (int)now.DayOfWeek) % 7); // Finde nächsten Sonntag
        DateTime targetTime = new DateTime(nextSunday.Year, nextSunday.Month, nextSunday.Day, 12, 0, 0, DateTimeKind.Utc); // Setze 12:00 Uhr UTC

        long triggerTime = (long)(targetTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        Log.Info("JobSchedulerHelper", $"Alarm geplant für: {targetTime} UTC");

        // Setze den Alarm
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }
        else
        {
            alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }
    }
}