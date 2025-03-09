using coyou.Services;
using coyou.Model;
using System.Collections.ObjectModel;
using coyou.Pages;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class FriendsPage : ContentPage
    {
        private readonly FriendService _friendService;
        public ObservableCollection<FriendModel> FriendsList { get; set; } = new ObservableCollection<FriendModel>();

        public FriendsPage(FriendService friendService)
        {
            InitializeComponent();
            _friendService = friendService;

            // Daten laden
            LoadFriends();
        }

        // Lädt die Liste der Freunde
        private async Task LoadFriends()
        {
            var friends = await _friendService.GetAllFriends();
            if (friends != null)
            {
                FriendsList.Clear();
                foreach (var friend in friends)
                {
                    FriendsList.Add(friend);
                }
            }
            FriendsCollection.ItemsSource = new List<FriendModel>();
            FriendsCollection.ItemsSource = friends;
        }

        protected override async void OnAppearing()
        {
            await LoadFriends();
        }

        // Senden einer Freundschaftsanfrage
        private async void OnSendFriendRequestClicked(object sender, EventArgs e)
        {
            var username = FriendRequestEntry.Text;
            if (!string.IsNullOrWhiteSpace(username))
            {
                await _friendService.SendFriendRequest(username);
                FriendRequestEntry.Text = "";
                // Hier könntest du eine Bestätigung anzeigen oder die Liste aktualisieren
            }
            
        }

        // Entfernen eines Freundes
        private async void OnRemoveFriendClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var friend = (FriendModel)button.CommandParameter;

            if (friend != null)
            {
                await _friendService.EndFriendShip(friend.Username);
                await LoadFriends();

            }
        }

        private async void OnUserPageClicked(object sender, EventArgs e)
        {
            var userPage = App.Current.Services.GetRequiredService<UserPage>();
            await Navigation.PushAsync(userPage);
        }

        private async void OnFriendsPageClicked(object sender, EventArgs e)
        {
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

        private async void OnFriendRequestPageClicked(object? sender, EventArgs e)
        {
            var friendRequestPage = App.Current.Services.GetRequiredService<FriendRequestPage>();
            await Navigation.PushAsync(friendRequestPage);
        }
    }
}