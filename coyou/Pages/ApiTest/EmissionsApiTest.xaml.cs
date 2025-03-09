using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Services;

namespace coyou.Pages.ApiTest;

public partial class EmissionsApiTest : ContentPage
{
    private readonly EmissionsService _emissionsService;

    public EmissionsApiTest(EmissionsService emissionsService)
    {
        InitializeComponent();
        _emissionsService = emissionsService;
    }

    private async void CalculateEmissions(object? sender, EventArgs e)
    {

        var waste = new Dictionary<string, double>
        {
            { "bio", 10 },
            { "general", 5 },
        };
        var energy = new Dictionary<string, double>
        {
            { "oil", 15 },
            { "hydro", 7 },
        };
        var emissionsModel = new EmissionsModel(null, "meat_based", waste, energy);
        var result = await _emissionsService.CalculateEmissions(emissionsModel);
        CalculateEmissionsLabel.Text = result?.ToString() ?? string.Empty;
    }

    private async void GetEmissionsOfWeek(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetEmissionsOfWeek(Convert.ToInt32(YearEntry.Text),
            Convert.ToInt32(WeekNumberEntry.Text));
        GetEmissionsOfWeekLabel.Text = result.ToString() ?? string.Empty;
    }

    private async void GetAllWasteTypes(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetAllWasteTypes();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllWasteTypesLabel.Text = s ?? string.Empty;
    }

    private async void GetWeeklyLeaderboard(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetWeeklyLeaderboard();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetWeeklyLeaderboardLabel.Text = s ?? string.Empty;
    }

    private async void GetEmissionHistory(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetEmissionHistory();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetEmissionHistoryLabel.Text = s ?? string.Empty;
    }

    private async void GetAllEnergyConsumptionTypes(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetAllEnergyConsumptionTypes();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllEnergyConsumptionTypesLabel.Text = s ?? string.Empty;
    }

    private async void GetAllDiets(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetAllDiets();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllDietsLabel.Text = s ?? string.Empty;
    }

    private async void GetEmissionsOfCurrentWeek(object? sender, EventArgs e)
    {
        var result = await _emissionsService.GetEmissionsOfCurrentWeek();
        GetEmissionsOfCurrentWeekLabel.Text = result.ToString() ?? string.Empty;
    }
}