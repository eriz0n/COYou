using coyou.Services;
using coyou.Model;
using System.Collections.ObjectModel;
using coyou.Pages;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class LeaderboardPage : ContentPage
    {
        private readonly EmissionsService _emissionsService;
        public ObservableCollection<LeaderboardModel> LeaderboardList { get; set; } = new ObservableCollection<LeaderboardModel>();

        public LeaderboardPage(EmissionsService emissionsService)
        {
            InitializeComponent();
            _emissionsService = emissionsService;

            // Leaderboard laden
            LoadLeaderboard();
        }

        // Lädt das Leaderboard
        private async void LoadLeaderboard()
        {
            var leaderboard = await _emissionsService.GetWeeklyLeaderboard();
            if (leaderboard != null)
            {
                LeaderboardList.Clear();
                foreach (var entry in leaderboard)
                {
                    LeaderboardList.Add(entry);
                }
            }
            LeaderboardCollection.ItemsSource = LeaderboardList;
        }

        // Weitere Methoden für die Navigation...
        private async void OnUserPageClicked(object sender, EventArgs e)
        {
            var userPage = App.Current.Services.GetRequiredService<UserPage>();
            await Navigation.PushAsync(userPage);
        }

        private async void OnFriendsPageClicked(object sender, EventArgs e)
        {
            var friendsPage = App.Current.Services.GetRequiredService<FriendsPage>();
            await Navigation.PushAsync(friendsPage);
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
    }
}
