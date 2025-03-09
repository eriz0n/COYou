using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using coyou.Pages;
using coyou.Services;


namespace coyou
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginService _loginService;
        private readonly UserService _userService;
        private readonly ApiService _apiService;
        private readonly EmissionsService _emissionsService;


        public LoginPage(LoginService loginService, UserService userService, ApiService apiService,
            EmissionsService emissionsService)
        {
            InitializeComponent();
            _loginService = loginService;
            _userService = userService;
            _apiService = apiService;
            _emissionsService = emissionsService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_apiService.IsUserLoggedIn()) return;

            if (!await _emissionsService.IsEmissionDataDone()) return;


            if (testApi.IsChecked)
            {
                var apiTestPage = App.Current.Services.GetRequiredService<ApiNavigation>();
                await Navigation.PushAsync(apiTestPage);
            }
            else
            {
                var mainPage = App.Current.Services.GetRequiredService<MainPage>();
                await Navigation.PushAsync(mainPage);
            }

            Navigation.RemovePage(this);
        }

        private async void OnGoogleLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var user = await _loginService.StartLogin();


                if (user == null) return;
                if (string.IsNullOrEmpty(user?.Username))
                {
                    string? username = null;
                    while (true)
                    {
                        while (string.IsNullOrEmpty(username))
                        {
                            username = await DisplayPromptAsync("Eingabe erforderlich",
                                "Bitte geben Sie ihren Benutzernamen ein", accept: "OK",
                                placeholder: "benutzername", cancel: null);
                        }

                        await _userService.EditUser(null, null, username);
                        var newUser = await _userService.GetUser();
                        if (newUser?.Username == username)
                            break;
                        await DisplayAlert("Fehler", "Benutzername vergeben", "erneut versuchen");
                        username = string.Empty;
                    }
                }

                if (!await _emissionsService.IsEmissionDataDone())
                {
                    await Navigation.PushAsync(App.Current.Services.GetRequiredService<EmissionCalculatorPage>());
                    Navigation.RemovePage(this);
                    return;
                }

                if (testApi.IsChecked)
                {
                    var apiTestPage = App.Current.Services.GetRequiredService<ApiNavigation>();
                    await Navigation.PushAsync(apiTestPage);
                }
                else
                {
                    var mainPage = App.Current.Services.GetRequiredService<MainPage>();
                    await Navigation.PushAsync(mainPage);
                }

                Navigation.RemovePage(this);


                //await DisplayAlert("Login", "Google Login Clicked!", "OK");
                // Do something with the token
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }


        private void Skip(object? sender, EventArgs e)
        {
            if (testApi.IsChecked)
            {
                var apiTestPage = App.Current.Services.GetRequiredService<ApiNavigation>();
                Navigation.PushAsync(apiTestPage);
            }
            else
            {
                var mainPage = App.Current.Services.GetRequiredService<MainPage>();
                Navigation.PushAsync(mainPage);
            }

            Navigation.RemovePage(this);
        }
    }
}