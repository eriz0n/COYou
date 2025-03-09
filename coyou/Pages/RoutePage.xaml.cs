using coyou.Services;
using coyou.Model;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class RoutePage : ContentPage
    {
        private readonly RouteService _routeService;
        public ObservableCollection<FullRouteModel> RoutesList { get; set; } = new ObservableCollection<FullRouteModel>();

        public RoutePage(RouteService routeService)
        {
            InitializeComponent();
            _routeService = routeService;

            // Daten laden
            LoadRoutes();
        }

        // Lädt die Liste der Routen
        private async Task LoadRoutes()
        {
            var routes = await _routeService.GetUserRoutes();
            if (routes != null)
            {
                RoutesList.Clear();
                foreach (var route in routes)
                {
                    RoutesList.Add(route);
                }
            }
            RoutesCollection.ItemsSource = RoutesList;
        }

        protected override async void OnAppearing()
        {
            await LoadRoutes();
        }

        // Bearbeiten einer Route
        private async void OnEditRouteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var route = (FullRouteModel)button.CommandParameter;

            if (route != null)
            {
                // RouteTrackingPage mit der bestehenden Route als Parameter aufrufen
                var routeTrackingPage = new RouteTrackingPage(_routeService, route);
                await Navigation.PushAsync(routeTrackingPage);
            }
        }

        // Löschen einer Route
        private async void OnDeleteRouteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var route = (FullRouteModel)button.CommandParameter;

            if (route != null)
            {
                var success = await _routeService.DeleteRoute(route.Id.Value);
                if (success.HasValue && success.Value)
                {
                    // Route nach dem Löschen aus der Liste entfernen
                    RoutesList.Remove(route);
                }
            }
        }

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

        private async void OnLeaderboardPageClicked(object sender, EventArgs e)
        {
            var leaderboardPage = App.Current.Services.GetRequiredService<LeaderboardPage>();
            await Navigation.PushAsync(leaderboardPage);
        }

     

        private async void OnRouteTrackingPageClicked(object? sender, EventArgs e)
        {
            var routeTrackingPage = new RouteTrackingPage(_routeService, null);
            await Navigation.PushAsync(routeTrackingPage);
        }

        private void OnRoutePageClicked(object? sender, EventArgs e)
        {
        }
    }
}
