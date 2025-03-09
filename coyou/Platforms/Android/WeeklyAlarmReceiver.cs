
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Util;
using coyou.Platforms.Android.Helpers;
using coyou.Services;
using global::Android.Content;
using global::Android.Util;
using static Java.Util.Jar.Attributes;
namespace coyou.Platforms.Android;

[BroadcastReceiver(Name = "coyou.WeeklyAlarmReceiver", Enabled = true, Exported = true)]
public class WeeklyAlarmReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        Log.Info("WeeklyAlarmReceiver", "Wöchentlicher Alarm ausgelöst, starte JobService...");

        // Starte den WeeklyJobService mit JobScheduler

        Task.Run(async () =>
        {
            await PerformWeeklyTask(context);
        });
        JobSchedulerHelper.ScheduleWeeklyAlarm(context);

    }

    private async Task PerformWeeklyTask(Context context)
    {
        Log.Info("WeeklyAlarmReceiver", "🚀 Wöchentliche Hintergrundaufgabe wird ausgeführt...");

        // 📌 DEINE WÖCHENTLICHE LOGIK HIER
        await Task.Delay(2000); // Beispiel: 2 Sekunden Verzögerung (zum Testen)
        var service = App.Current.Services.GetRequiredService<EmissionsService>();
        if (!await service.IsEmissionCalculationAvailable())
        {
            Log.Info("WeeklyAlarmReceiver", "EmissionsCalculation nicht verfügbar");
            Log.Info("WeeklyAlarmReceiver", "✅ Wöchentliche Hintergrundaufgabe abgeschlossen.");
            return;
        }
        var model = await service.GenerateEmissionsModel();
        await service.CalculateEmissions(model);
        Log.Info("WeeklyAlarmReceiver", "✅ Wöchentliche Hintergrundaufgabe abgeschlossen.");
    }
}
