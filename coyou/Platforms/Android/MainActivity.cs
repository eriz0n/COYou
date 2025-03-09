using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using coyou.Platforms.Android.Helpers;

namespace coyou.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Prüfe und fordere Berechtigung an, falls sie fehlt
            if (!AlarmPermissionHelper.HasExactAlarmPermission(this))
            {
                Log.Warn("MainActivity", "Keine Berechtigung für exakte Alarme! Benutzer wird weitergeleitet...");
                AlarmPermissionHelper.RequestExactAlarmPermission(this);
            }
            else
            {
                Log.Info("MainActivity", "Berechtigung für exakte Alarme vorhanden.");
            }

            // Alarm für Sonntag planen
            JobSchedulerHelper.ScheduleWeeklyAlarm(this);
        }


    }
}
