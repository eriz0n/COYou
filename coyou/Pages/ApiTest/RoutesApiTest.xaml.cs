using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Services;

namespace coyou.Pages.ApiTest;

public partial class RoutesApiTest : ContentPage
{
    private readonly RouteService _routeService;
    public RoutesApiTest(RouteService routeService)
    {
        InitializeComponent();
        _routeService = routeService;
    }

    private async void EditRoute(object? sender, EventArgs e)
    {
        var route = new RouteModel("Leoben", "Scheifling", "petrol car", 70);
        var result = await _routeService.EditRoute(Convert.ToInt64(EditRouteEntry.Text), route);
        EditRouteLabel.Text = result.ToString() ?? string.Empty;
    }

    private async void DeleteRoute(object? sender, EventArgs e)
    {
        var result = await _routeService.DeleteRoute(Convert.ToInt64(EditRouteEntry.Text));
        DeleteRouteLabel.Text = result.ToString() ?? string.Empty;
    }

    private async void GetUserRoutes(object? sender, EventArgs e)
    {
        var result = await _routeService.GetUserRoutes();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetUserRoutesLabel.Text = s ?? string.Empty;
    }

    private async void AddRoute(object? sender, EventArgs e)
    {
        var route = new RouteModel("Leoben", "Oberzeiring", "diesel car", 60);
        var result = await _routeService.AddRoute(route);
        AddRouteLabel.Text = result.ToString() ?? string.Empty;
    }

    private async void GetAllTypes(object? sender, EventArgs e)
    {
        var result = await _routeService.GetAllTypes();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllTypesLabel.Text = s ?? string.Empty;
    }

    private async void GetHistory(object? sender, EventArgs e)
    {
        var result = await _routeService.GetHistory(DateOnly.FromDateTime(StartDatePicker.Date), DateOnly.FromDateTime(EndDatePicker.Date));
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetHistoryLabel.Text = s ?? string.Empty;
    }
}