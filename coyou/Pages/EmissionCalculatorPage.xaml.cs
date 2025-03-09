using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Services;
using System.Collections.ObjectModel;

namespace coyou.Pages;

public partial class EmissionCalculatorPage : ContentPage
{
    private readonly EmissionsService _emissionsService;
    private readonly RouteService _routeService;
    private readonly ObservableCollection<KeyValuePair<string, string>> _diets = [];
    private readonly ObservableCollection<KeyValuePair<string, string>> _energyTypes = [];
    private readonly ObservableCollection<KeyValuePair<string, string>> _wasteTypes = [];
    private readonly ObservableCollection<KeyValuePair<string, string>> _transportationTypes = [];


    public EmissionCalculatorPage(EmissionsService emissionsService, RouteService routeService)
    {
        InitializeComponent();
        _emissionsService = emissionsService;
        _routeService = routeService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var diets = await _emissionsService.GetAllDiets();
        var energyTypes = await _emissionsService.GetAllEnergyConsumptionTypes();
        var wasteTypes = await _emissionsService.GetAllWasteTypes();
        var transportationTypes = await _routeService.GetAllTypes();

        diets.ForEach(model =>
            _diets.Add(new KeyValuePair<string, string>(model.DietType, GetDisplayName(model.DietType))));
        wasteTypes.ForEach(model =>
            _wasteTypes.Add(new KeyValuePair<string, string>(model.Type, GetDisplayName(model.Type))));
        energyTypes.ForEach(model =>
            _energyTypes.Add(new KeyValuePair<string, string>(model.Type, GetDisplayName(model.Type))));
        transportationTypes.ForEach(model =>
            _transportationTypes.Add(new KeyValuePair<string, string>(model.Name, GetDisplayName(model.Name))));

        EnergyPicker.ItemsSource = _energyTypes;
        EnergyPicker.ItemDisplayBinding = new Binding("Value");
        WastePicker.ItemsSource = _wasteTypes;
        WastePicker.ItemDisplayBinding = new Binding("Value");
        DietPicker.ItemsSource = _diets;
        DietPicker.ItemDisplayBinding = new Binding("Value");
        TransportationPicker.ItemsSource = _transportationTypes;
        TransportationPicker.ItemDisplayBinding = new Binding("Value");

        var diet = await _emissionsService.ReadDiet();
        DietPicker.SelectedItem = new KeyValuePair<string, string>(diet, GetDisplayName(diet));
        var energy = await _emissionsService.ReadEnergy();
        foreach (var type in energy)
        {
            EnergyPicker.ItemsSource.Remove(new KeyValuePair<string, string>(type.Key, GetDisplayName(type.Key)));
            var entryLayout = await CreateEntryRow("KWh", EnergyContainer, EnergyPicker, type);
            EnergyContainer.Children.Add(entryLayout);
        }

        var waste = await _emissionsService.ReadWaste();
        foreach (var type in waste)
        {
            WastePicker.ItemsSource.Remove(new KeyValuePair<string, string>(type.Key, GetDisplayName(type.Key)));
            var entryLayout = await CreateEntryRow("KG", WasteContainer, WastePicker, type);
            WasteContainer.Children.Add(entryLayout);
        }


        if (!await _emissionsService.IsEmissionDataDone())
        {
            Transportation.IsEnabled = true;
            Transportation.IsVisible = true;
        }
    }

    // Methode zum Hinzufügen eines Müll-Eintrags
    private async void OnAddWasteClicked(object sender, EventArgs e)
    {
        if (WastePicker.SelectedItem == null)
            return;
        var entryLayout = await CreateEntryRow("KG", WasteContainer, WastePicker);
        WasteContainer.Children.Add(entryLayout);
    }

    // Methode zum Hinzufügen eines Strom-Eintrags
    private async void OnAddEnergyClicked(object sender, EventArgs e)
    {
        if (EnergyPicker.SelectedItem == null)
            return;
        var entryLayout = await CreateEntryRow("KWh", EnergyContainer, EnergyPicker);
        EnergyContainer.Children.Add(entryLayout);
    }

    // Methode zum Erstellen einer neuen Zeile mit Picker, Entry und Buttons
    private async Task<View> CreateEntryRow(string unit, VerticalStackLayout parentContainer, Picker picker,
        KeyValuePair<string, double>? kvp = null)
    {
        KeyValuePair<string, string> labelKvp;
        if (kvp != null)
        {
            labelKvp = new KeyValuePair<string, string>(kvp.Value.Key, GetDisplayName(kvp.Value.Key));
        }
        else
        {
            labelKvp = (KeyValuePair<string, string>)picker.SelectedItem;
        }

        var label = new Label
        {
            BindingContext = labelKvp,
            VerticalOptions = LayoutOptions.Center,
        };
        label.SetBinding(Label.TextProperty, new Binding("Value"));

        var entry = new Entry
        {
            VerticalOptions = LayoutOptions.Center,
            Text = kvp == null ? "0" : kvp.Value.Value.ToString(),
            Keyboard = Keyboard.Numeric,
            WidthRequest = 50
        };

        var removeButton = new Button
        {
            Text = "-",
            VerticalOptions = LayoutOptions.Center,
        };

        var rowLayout = new HorizontalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Spacing = 10,
            Children = { label, entry, new Label { Text = unit, VerticalOptions = LayoutOptions.Center }, removeButton }
        };
        picker.ItemsSource.Remove(picker.SelectedItem);

        // Entfernen des Elements
        removeButton.Clicked += async (s, e) =>
        {
            picker.ItemsSource.Add(label.BindingContext);
            if (picker == EnergyPicker)
            {
                await _emissionsService.WriteEnergy(((KeyValuePair<string, string>)label.BindingContext).Value, -1);
            }
            else
            {
                await _emissionsService.WriteWaste(((KeyValuePair<string, string>)label.BindingContext).Value, -1);
            }

            parentContainer.Children.Remove(rowLayout);
        };

        return rowLayout;
    }

    private string GetDisplayName(string key)
    {
        return App.GetDisplayName(key);
    }

    // Methode zum Absenden der Daten (Hier kannst du die gesammelten Werte verarbeiten)
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (!Validate())
        {
            await DisplayAlert("Fehler", "Eingaben unvollständig oder Fehlerhaft", "OK");
            return;
        }

        await _emissionsService.WriteDiet(((KeyValuePair<string, string>)DietPicker.SelectedItem).Key);
        foreach (var view in EnergyContainer.Children)
        {
            var row = (HorizontalStackLayout)view;
            var key = "";
            var value = 0d;
            foreach (var child in row.Children)
            {
                if (child.GetType() == typeof(Entry))
                {
                    var entry = (Entry)child;
                    value = Convert.ToDouble(entry.Text);
                }

                if (string.IsNullOrEmpty(key) && child.GetType() == typeof(Label))
                {
                    var label = (Label)child;
                    key = ((KeyValuePair<string, string>)label.BindingContext).Key;
                }
            }

            await _emissionsService.WriteEnergy(key, value);
        }

        foreach (var view in WasteContainer.Children)
        {
            var row = (HorizontalStackLayout)view;
            var key = "";
            var value = 0d;
            foreach (var child in row.Children)
            {
                if (child.GetType() == typeof(Entry))
                {
                    var entry = (Entry)child;
                    value = Convert.ToDouble(entry.Text);
                }

                if (string.IsNullOrEmpty(key) && child.GetType() == typeof(Label))
                {
                    var label = (Label)child;
                    key = ((KeyValuePair<string, string>)label.BindingContext).Key;
                }
            }

            await _emissionsService.WriteWaste(key, value);
        }

        await _emissionsService.WriteDiet(((KeyValuePair<string, string>)DietPicker.SelectedItem).Value);


        if (await _emissionsService.IsEmissionDataDone())
        {
            await DisplayAlert("Erfolg", "Emissionen erfolgreich aktualisiert", "OK");
            Navigation.RemovePage(this);
            return;
        }

        var emissionsModel = await _emissionsService.GenerateEmissionsModel(
            ((KeyValuePair<string, string>)TransportationPicker.SelectedItem).Key
            , Convert.ToInt32(TransportationEntry.Text)
        );
        await _emissionsService.SetEmissionDataDone();

        var emissions = await _emissionsService.CalculateEmissions(emissionsModel);

        if (emissions == null)
        {
            await DisplayAlert("Erfolg", "Emissionen erfolgreich aktualisiert", "OK");
            var page = App.Current.Services.GetRequiredService<MainPage>();
            await Navigation.PushAsync(page);
            Navigation.RemovePage(this);
            return;
        }

        await DisplayAlert("Wöchentliche Emissionen", $"{emissions.ToString()}g CO2", "OK");

        var mainPage = App.Current.Services.GetRequiredService<MainPage>();
        await Navigation.PushAsync(mainPage);

        Navigation.RemovePage(this);
    }

    private bool Validate()
    {
        if (Transportation.IsEnabled &&
            (TransportationPicker.SelectedItem == null ||
             string.IsNullOrEmpty(TransportationEntry.Text) ||
             Convert.ToDouble(TransportationEntry.Text) < 0))
            return false;
        foreach (var view in WasteContainer.Children)
        {
            var row = (HorizontalStackLayout)view;
            var value = 0d;
            foreach (var child in row.Children)
            {
                if (child.GetType() != typeof(Entry)) continue;
                var entry = (Entry)child;
                value = Convert.ToDouble(entry.Text);
                if (value < 0)
                    return false;
            }
        }        
        foreach (var view in EnergyContainer.Children)
        {
            var row = (HorizontalStackLayout)view;
            var value = 0d;
            foreach (var child in row.Children)
            {
                if (child.GetType() != typeof(Entry)) continue;

                var entry = (Entry)child;
                value = Convert.ToDouble(entry.Text);
                if (value < 0)
                    return false;
            }
        }
        return true;
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
}