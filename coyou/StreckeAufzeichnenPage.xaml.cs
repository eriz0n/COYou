using coyou;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using coyou.Services;

namespace coyou
{
    public partial class StreckeAufzeichnenPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly EmissionsService _emissionsService;
        private readonly RouteService _routeService;
        // Die Streckenliste aus der App-Klasse
        private ObservableCollection<Strecke> StreckenList;

        // Konstruktor der StreckeAufzeichnenPage


        public StreckeAufzeichnenPage(UserService userService, FriendService friendService, EmissionsService emissionsService, RouteService routeService)
        {
            InitializeComponent();
            StreckenList = (Application.Current as App).StreckenList; // Zugriff auf die ObservableCollection aus der App-Klasse

            _userService = userService;
            _friendService = friendService;
            _emissionsService = emissionsService;
            _routeService = routeService;
        }

        // Event-Handler f�r den Button "Strecke Hinzuf�gen"
        private void OnStreckeHinzufuegen(object sender, EventArgs e)
        {
            // Die eingegebenen Werte aus den Textfeldern holen
            string startpunkt = startpunktEntry.Text;
            string endpunkt = endpunktEntry.Text;
            string fortbewegungsmittel = fortbewegungsmittelPicker.SelectedItem?.ToString();

            // Validierung, ob alle Felder ausgef�llt sind
            if (string.IsNullOrWhiteSpace(startpunkt) || string.IsNullOrWhiteSpace(endpunkt) || string.IsNullOrWhiteSpace(fortbewegungsmittel))
            {
                DisplayAlert("Fehler", "Bitte alle Felder ausf�llen!", "OK");
                return;
            }

            // Neue Strecke erstellen und zur Liste hinzuf�gen
            StreckenList.Add(new Strecke(startpunkt, endpunkt, fortbewegungsmittel));

            // Nach dem Hinzuf�gen zur Strecken�bersicht navigieren
            Navigation.PushAsync(new StreckenuebersichtPage(StreckenList.ToList(), _userService, _friendService, _emissionsService, _routeService));
        }
    }
}
