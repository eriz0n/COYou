using coyou.Pages;
using coyou.Pages.ApiTest;
using coyou.Services;
using Microsoft.Extensions.Logging;

namespace coyou
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<LoginService>();
            builder.Services.AddSingleton<DummyDataService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<RouteService>();
            builder.Services.AddSingleton<FriendService>();
            builder.Services.AddSingleton<EmissionsService>();

            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddTransient<ApiNavigation>();
            builder.Services.AddTransient<UserApiTest>();
            builder.Services.AddTransient<RoutesApiTest>();
            builder.Services.AddTransient<FriendsApiTest>();
            builder.Services.AddTransient<EmissionsApiTest>();
            builder.Services.AddTransient<EmissionCalculatorPage>();

            builder.Services.AddTransient<UserPage>();
            builder.Services.AddTransient<FriendsPage>();
            builder.Services.AddTransient<FriendRequestPage>();
            builder.Services.AddTransient<RoutePage>();
            builder.Services.AddTransient<RouteTrackingPage>();
            builder.Services.AddTransient<LeaderboardPage>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MilestonesPage>();
            builder.Services.AddTransient<StreckeAufzeichnenPage>();
            builder.Services.AddTransient<StreckenuebersichtPage>();
            return builder.Build(); 
        }
    }
}
