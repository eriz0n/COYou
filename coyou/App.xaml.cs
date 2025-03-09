using coyou.Services;
using System.Collections.ObjectModel;

namespace coyou
{
    public partial class App : Application
    {    
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }
        public ObservableCollection<Strecke> StreckenList { get; set; }
        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;
           
            MainPage = new NavigationPage(App.Current.Services.GetRequiredService<LoginPage>());
        }
        
        public static string GetDisplayName(string key)
        {
            return key switch
            {
                "bicycle" => "Fahrrad",
                "glass" => "Glas",
                "meat_based" => "Fleischbasiert",
                "vegan" => "Vegan",
                "vegetarian" => "Vegetarisch",
                "coal" => "Kohle",
                "hydro" => "Wasserenergie",
                "natural_gas" => "Erdgas",
                "nuclear" => "Kernenergie",
                "oil" => "Öl",
                "solar" => "Solarenergie",
                "wind" => "Windenergie",
                "diesel bus" => "Dieselbus",
                "diesel car" => "Dieselauto",
                "diesel train" => "Diesellok",
                "domestic flight plane" => "Inlandsflugzeug",
                "e-bike" => "E-Bike",
                "electric bus" => "Elektrobus",
                "electric car" => "Elektroauto",
                "electric train" => "Elektrozug",
                "hybrid car" => "Hybridauto",
                "long-haul flight plane" => "Langstreckenflugzeug",
                "motorcycle" => "Motorrad",
                "petrol car" => "Benzinauto",
                "walking" => "zu Fuß",
                "bio" => "Bio",
                "general" => "Allgemein",
                "metal" => "Metall",
                "paper" => "Papier",
                "plastic" => "Plastik",

                _ => key
            };
        }
    }
}
