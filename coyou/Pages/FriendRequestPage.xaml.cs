using coyou.Services;
using coyou.Model;
using System.Collections.ObjectModel;
using coyou.Pages;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class FriendRequestPage : ContentPage
    {
        private readonly FriendService _friendService;
        public ObservableCollection<FriendRequestModel> FriendRequests { get; set; } = new ObservableCollection<FriendRequestModel>();

        public FriendRequestPage(FriendService friendService)
        {
            InitializeComponent();
            _friendService = friendService;

            // Freundschaftsanfragen laden
            LoadFriendRequests();
        }

        // Lädt die Liste der Freundschaftsanfragen
        private async Task LoadFriendRequests()
        {
            var requests = await _friendService.GetAllReceivedFriendRequests();
            if (requests != null)
            {
                FriendRequests.Clear();
                foreach (var request in requests)
                {
                    FriendRequests.Add(request);
                }
            }
            FriendRequestCollection.ItemsSource = new List<FriendRequestModel>();
            FriendRequestCollection.ItemsSource = FriendRequests;
        }

        // Akzeptieren einer Freundschaftsanfrage
        private async void OnAcceptFriendRequestClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var friendRequest = (FriendRequestModel)button.CommandParameter;

            if (friendRequest != null)
            {
                var result = await _friendService.AcceptFriendRequest(friendRequest.User);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    // Freundschaftsanfrage akzeptiert, Liste aktualisieren
                    await LoadFriendRequests();
                }
            }
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
