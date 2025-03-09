using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Pages.ApiTest;
using coyou.Services;

namespace coyou.Pages;

public partial class ApiNavigation : ContentPage
{
    private ApiService _apiService;

    public ApiNavigation(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void UserButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(App.Current.Services.GetRequiredService<UserApiTest>());
    }

    private async void EmissionsButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(App.Current.Services.GetRequiredService<EmissionsApiTest>());
    }

    private async void FriendsButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(App.Current.Services.GetRequiredService<FriendsApiTest>());
    }

    private async void RoutesButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(App.Current.Services.GetRequiredService<RoutesApiTest>());
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _apiService.LogOut();
        var loginPage = App.Current.Services.GetRequiredService<LoginPage>();
        await Navigation.PushAsync(loginPage);

        Navigation.RemovePage(this);

    }

    private async void CalculatorButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(App.Current.Services.GetRequiredService<EmissionCalculatorPage>());

    }
}