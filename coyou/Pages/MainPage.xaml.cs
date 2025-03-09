using coyou.Pages;
using System.Linq;
using coyou.Services; // Für ToList()

namespace coyou
{
    public partial class MainPage : ContentPage
    {
        // Hier muss auf die ObservableCollection zugegriffen werden
        private App app => Application.Current as App;
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly EmissionsService _emissionsService;
        private readonly RouteService _routeService;
        private readonly ApiService _apiService;

        public MainPage(UserService userService, FriendService friendService, EmissionsService emissionsService, RouteService routeService, ApiService apiService)
        {
            InitializeComponent();
            _userService = userService;
            _friendService = friendService;
            _emissionsService = emissionsService;
            _routeService = routeService;
            _apiService = apiService;
        }
        private async void OnUserPageClicked(object sender, EventArgs e)
        {
            var userPage = App.Current.Services.GetRequiredService<UserPage>();
            await Navigation.PushAsync(userPage);
        }
        
        private async void OnRoutePageClicked(object sender, EventArgs e)
        {
            var routePage = App.Current.Services.GetRequiredService<RoutePage>();
            await Navigation.PushAsync(routePage);
        }

        private async void OnLeaderboardPageClicked(object sender, EventArgs e)
        {
            var leaderboardPage = App.Current.Services.GetRequiredService<LeaderboardPage>();
            await Navigation.PushAsync(leaderboardPage);
        }

        private async void OnFriendsPageClicked(object sender, EventArgs e)
        {
            var friendsPage = App.Current.Services.GetRequiredService<FriendsPage>();
            await Navigation.PushAsync(friendsPage);
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            var mainPage = App.Current.Services.GetRequiredService<MainPage>();
            await Navigation.PushAsync(mainPage);
        }
    }
}
