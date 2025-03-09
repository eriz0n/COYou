using coyou.Services;
using Microsoft.Maui.Controls;
using System;
using coyou.Pages;

namespace coyou
{
    public partial class UserPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly ApiService _apiService;

        public UserPage(UserService userService, ApiService apiService)
        {
            InitializeComponent();
            _userService = userService;
            _apiService = apiService;
            BindingContext = this;
            LoadUserData();
        }


        private async Task LoadUserData()
        {
            var user = await _userService.GetUser();
            if (user != null)
            {
                FirstNameEntry.Text = user.FirstName;
                LastNameEntry.Text = user.LastName;
                UserNameEntry.Text = user.Username;
                ProfilePictureView.Source = user.GetProfileImageSource();
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Speichern der Änderungen
            var oldUser = await _userService.GetUser();
            var change = oldUser.FirstName != FirstNameEntry.Text ||
                         oldUser.LastName != LastNameEntry.Text ||
                         oldUser.Username != UserNameEntry.Text;
            var newUser = await _userService.EditUser(FirstNameEntry.Text, LastNameEntry.Text, UserNameEntry.Text);
            if (newUser == null || (change && oldUser.Equals(newUser)))
            {
                await DisplayAlert("Fehler", "Änderungen wurden nicht übernommen", "OK");
                await LoadUserData();
                return;
            }

            // Optional: Eine Bestätigung oder Nachricht anzeigen, dass die Änderungen gespeichert wurden
            await DisplayAlert("Erfolg", "Änderungen wurden gespeichert!", "OK");
        }

        // Navigation methods (if required)
        private async void OnUserPageClicked(object sender, EventArgs e)
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

        private async void OnEmissionsPageClicked(object? sender, EventArgs e)
        {
            var emissionsPage = App.Current.Services.GetRequiredService<EmissionCalculatorPage>();
            await Navigation.PushAsync(emissionsPage);
        }

        private async void OnLogOutClicked(object? sender, EventArgs e)
        {
            _apiService.LogOut();
            var loginPage = App.Current.Services.GetRequiredService<LoginPage>();

            // Insert the new page before the root page
            await Navigation.PushAsync(loginPage);

            // Remove all previous pages
            foreach (var page in Navigation.NavigationStack.ToList())
            {
                if (page != loginPage)
                    Navigation.RemovePage(page);
            }
        }
    }
}