using coyou.Services;
using coyou.Model;
using coyou.Pages;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class RouteTrackingPage : ContentPage
    {
        private readonly RouteService _routeService;
        private RouteModel Route { get; set; }
        private long? RouteId { get; set; }
        private readonly List<KeyValuePair<string, string>> _transportationTypes = [];


        // Konstruktor für das Erstellen einer neuen Route
        public RouteTrackingPage(RouteService routeService, FullRouteModel? fullRouteModel)
        {
            InitializeComponent();
            _routeService = routeService;

            Route = fullRouteModel != null
                ? new RouteModel(fullRouteModel.Start, fullRouteModel.End, fullRouteModel.MovementType,
                    fullRouteModel.LengthKm)
                : new RouteModel("", "", "", null); // Neue Route ohne Werte
            RouteId = fullRouteModel?.Id;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var transportationTypes = await _routeService.GetAllTypes();
            transportationTypes.ForEach(model =>
                _transportationTypes.Add(new KeyValuePair<string, string>(model.Name, GetDisplayName(model.Name))));


            MovementTypePicker.ItemsSource = _transportationTypes;
            MovementTypePicker.ItemDisplayBinding = new Binding("Value");

            if (Route != null)
            {
                StartEntry.Text = Route.Start;
                EndEntry.Text = Route.End;
                LengthEntry.Text = Route.LengthKm.ToString();
                

                foreach (KeyValuePair<string, string> kvp in MovementTypePicker.ItemsSource)
                {
                    if (kvp.Key == Route.MovementType)
                    {
                        MovementTypePicker.SelectedItem = kvp;
                        break;
                    }
                }
            }
        }

        private string GetDisplayName(string key)
        {
            return App.GetDisplayName(key);
        }
        

        // Speichern der Route
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Route.Start = StartEntry.Text;
            Route.End = EndEntry.Text;
            Route.LengthKm = Convert.ToDouble(LengthEntry.Text);
            Route.MovementType = ((KeyValuePair<string, string>)MovementTypePicker.SelectedItem).Key;
            if (RouteId != null)
            {
                await _routeService.EditRoute((long)RouteId, Route);
            }
            else
            {
                await _routeService.AddRoute(Route);
            }


            // Nach dem Speichern könntest du zurück navigieren
            await Navigation.PopAsync();
        }

        // Navigiere zur UserPage
        private async void OnUserPageClicked(object sender, EventArgs e)
        {
            var userPage = App.Current.Services.GetRequiredService<UserPage>();
            await Navigation.PushAsync(userPage);
        }

        // Navigiere zur FriendsPage
        private async void OnFriendsPageClicked(object sender, EventArgs e)
        {
            var friendsPage = App.Current.Services.GetRequiredService<FriendsPage>();
            await Navigation.PushAsync(friendsPage);
        }

        // Navigiere zur RoutePage (zum Beispiel eine Übersicht)
        private async void OnRoutePageClicked(object sender, EventArgs e)
        {
            var routePage = App.Current.Services.GetRequiredService<RoutePage>();
            await Navigation.PushAsync(routePage);
        }

        // Navigiere zur LeaderboardPage
        private async void OnLeaderboardPageClicked(object sender, EventArgs e)
        {
            var leaderboardPage = App.Current.Services.GetRequiredService<LeaderboardPage>();
            await Navigation.PushAsync(leaderboardPage);
        }

        // Navigiere zur FriendRequestPage
        private async void OnFriendRequestPageClicked(object sender, EventArgs e)
        {
            var friendRequestPage = App.Current.Services.GetRequiredService<FriendRequestPage>();
            await Navigation.PushAsync(friendRequestPage);
        }
    }
}