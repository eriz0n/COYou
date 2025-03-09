using coyou.Pages;
using coyou.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;


namespace coyou.Services
{
    public class LoginService
    {
        private readonly ApiService _apiService;
        private readonly UserService _userService;
        private readonly HttpClient _httpClient;
        public LoginService(ApiService apiService, UserService userService)
        {
            _apiService = apiService;
            _userService = userService;
            _httpClient = new HttpClient();
        }

         

        public async Task<UserModel?> StartLogin()
        {
            if (_apiService.IsUserLoggedIn())
                return await _userService.GetUser();
            try
            {

                var authUrl = $"{Constants.Google.AuthUri}?response_type=code" +
                              $"&redirect_uri={Constants.CallBackUrl}" +
                              $"&client_id={Constants.Google.ClientId}" +
                              $"&scope=openid email profile" +
                              $"&include_granted_scopes=true" +
                              $"&state=state_parameter_passthrough_value";
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri(authUrl),
                    new Uri(Constants.CallBackUrl));

                var code = authResult.Properties["code"];

                var requestUri = "https://oauth2.googleapis.com/token";

                var parameters = new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", Constants.Google.ClientId },
                    { "redirect_uri", Constants.CallBackUrl },
                    { "grant_type", "authorization_code" }
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var login = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                    if (login?.IdToken != null) _apiService.SetBearerToken(login.IdToken);
                    if (login?.RefreshToken != null) _apiService.SetRefreshToken(login.RefreshToken);

                    var user = await _userService.GetUser();
                    return user;

                    ; // Contains access_token, refresh_token, etc.
                }
                else
                {
                    //return $"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}";
                    string error = $"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}";
                    Console.WriteLine(error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}