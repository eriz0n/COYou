using coyou;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using coyou.Services;

namespace coyou
{
    public partial class StreckenuebersichtPage : ContentPage
    {
        // Der Konstruktor nimmt die Liste der Strecken als Parameter
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly EmissionsService _emissionsService;
        private readonly RouteService _routeService;

        public StreckenuebersichtPage(List<Strecke> streckenList, UserService userService, FriendService friendService, EmissionsService emissionsService, RouteService routeService)
        {
            InitializeComponent();
            _userService = userService;
            _friendService = friendService;
            _emissionsService = emissionsService;
            _routeService = routeService;
            streckenCollectionView.ItemsSource = streckenList; // Bindet die Liste der Strecken an die CollectionView

        }

        // Event-Handler für den Button "Zurück zur Aufzeichnung"
        private async void OnZurueckZurStreckeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Zurück zur Strecken-Aufzeichnungs-Seite
        }
    }
}
