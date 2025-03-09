
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Util;
using global::Android.App;
using global::Android.Content;
using global::Android.OS;

public static class AlarmPermissionHelper
{
    public static bool HasExactAlarmPermission(Context context)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.S) // Android 12+
        {
            var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
            return alarmManager?.CanScheduleExactAlarms() ?? false;
        }
        return true; // Ältere Versionen brauchen keine spezielle Berechtigung
    }

    public static void RequestExactAlarmPermission(Activity activity)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.UpsideDownCake) // Android 14+
        {
            Intent intent = new Intent(Settings.ActionRequestScheduleExactAlarm);
            intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
            activity.StartActivity(intent);
        }
        else if (Build.VERSION.SdkInt >= BuildVersionCodes.S) // Android 12+ (API 31)
        {
            Intent intent = new Intent(Settings.ActionAppNotificationSettings);
            intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
            activity.StartActivity(intent);
        }
    }
}
