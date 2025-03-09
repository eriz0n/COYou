using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

using System.Threading.Tasks;
using coyou.Model;
using System.Net;

namespace coyou.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string? _bearerToken;
    private string? _refreshToken;

    public ApiService()
    {

        //var uri = new Uri(DeviceInfo.Platform == DevicePlatform.Android
        //    ? "http://10.0.2.2:8080"
        //    : "http://localhost:8080");

        var uri = new Uri("INSERT_SERVER_URI");

        _httpClient = new HttpClient
        {
            BaseAddress = uri,
            //MaxResponseContentBufferSize = long.MaxValue,
        };
        _bearerToken = SecureStorage.GetAsync("bearer_token").Result;
        _refreshToken = SecureStorage.GetAsync("refresh_token").Result;
    }

    public void SetBearerToken(string token)
    {
        _bearerToken = token;
        SecureStorage.SetAsync("bearer_token", token);

    }
    public void SetRefreshToken(string token)
    {
        _refreshToken = token;
        SecureStorage.SetAsync("refresh_token", token);
    }

    public bool IsUserLoggedIn()
    {
        return _bearerToken != null && _refreshToken != null;
    }

    public void LogOut()
    {
        _bearerToken = null;
        _refreshToken = null;
        SecureStorage.Remove("refresh_token");
        SecureStorage.Remove("bearer_token");

    }

    private bool IsTokenExpired()
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(_bearerToken))
            return true; // Ungültiger Token ist automatisch "abgelaufen"

        var jwtToken = handler.ReadJwtToken(_bearerToken);
        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");

        if (expClaim == null)
            return true; // Kein Ablaufdatum gefunden → sicherheitshalber als abgelaufen betrachten

        var expUnix = long.Parse(expClaim.Value);
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

        return expDateTime < DateTime.UtcNow;
    }

    private async Task AddAuthorizationHeader()
    {
        if (IsTokenExpired())
        {
            var client = new HttpClient();
            var model = new TokenRefreshModel(_refreshToken);
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.Google.TokenUri, content);
            var handled = await HandleResponse<LoginResponse>(response);
            if (handled?.IdToken != null)
            {
                _bearerToken = handled.IdToken;
            }
            else
            {
                return;
            }
            
            
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            !string.IsNullOrEmpty(_bearerToken) ? new AuthenticationHeaderValue("Bearer", _bearerToken) : null;
    }

    /// <summary>
    /// Sends a POST request with a JSON body.
    /// </summary>
    private async Task<T?> PostAsync<T>(string endpoint, object? requestData = null)
    {
        await AddAuthorizationHeader();
        HttpResponseMessage? response;
        if (requestData != null)
        {
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync(endpoint, content);

        }
        else
        {
            response = await _httpClient.PostAsync(endpoint, null);

        }


        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// Sends a GET request.
    /// </summary>
    private async Task<T?> GetAsync<T>(string endpoint, object? requestData = null)
    {
        await AddAuthorizationHeader();
        if (requestData != null)
        {
            var json = JsonConvert.SerializeObject(requestData);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                
            };
            request.Method = new HttpMethod("GET");

            var response1 = await _httpClient.SendAsync(request);
                //.ConfigureAwait(false);
            //response1.EnsureSuccessStatusCode();
            return await HandleResponse<T>(response1);
        }


        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// Handles API responses and deserialization.
    /// </summary>
    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
            {
                return (T?)(object)jsonString; // Explizites Casting zu object und dann zu T
            }
            if (typeof(T) == typeof(UserModel))
            {
                var index = jsonString.IndexOf(",\"receiver\":");
                if (index != -1)
                    jsonString= jsonString.Substring(0, index) + "}";

            }
            //Console.WriteLine(jsonString);
            //Console.WriteLine(typeof(T));
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        return default;
    }

    /// <summary>
    /// Sends a PUT request with a JSON body.
    /// </summary>
    private async Task<T?> PutAsync<T>(string endpoint, object? requestData = null)
    {
        await AddAuthorizationHeader();
        requestData ??= new object();
        var json = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        Console.WriteLine(json);
        Console.WriteLine(content.ReadAsStringAsync().Result);
        var response = await _httpClient.PutAsync(endpoint, content);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// Sends a DELETE request.
    /// </summary>
    private async Task<bool> DeleteAsync(string endpoint)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    // ----- USER CONTROLLER -----
    public async Task<UserModel?> GetUserAsync() => await PostAsync<UserModel>("/user", null);
    public async Task<UserModel?> EditUserAsync(UserModel user) => await PutAsync<UserModel>("/user", user);

    // ----- ROUTE CONTROLLER -----
    public async Task<double?> EditRouteAsync(long id, RouteModel route) =>
        await PutAsync<double>($"/api/route/{id}", route);

    public async Task<bool?> DeleteRouteAsync(long id) => await DeleteAsync($"/api/route/{id}");
    public async Task<List<FullRouteModel>?> GetUserRoutesAsync() => await GetAsync<List<FullRouteModel>>("/api/route");
    public async Task<double?> AddRouteAsync(RouteModel route) => await PostAsync<double>("/api/route", route);

    public async Task<List<TypeModel>?> GetAllTypesAsync() =>
        await GetAsync<List<TypeModel>>("/api/route/types");

    public async Task<List<HistoryModel>?> GetHistoryAsync(HistoryRequestModel request) =>
        await GetAsync<List<HistoryModel>>("/api/route/history", request);

    // ----- FRIEND CONTROLLER -----
    public async Task<string?> SendFriendRequestAsync(string username) =>
        await PostAsync<string>($"/friends/send/{username}");

    public async Task<string?> AcceptFriendRequestAsync(string username) =>
        await PostAsync<string>($"/friends/accept/{username}");

    public async Task<List<FriendModel>?> GetAllFriendsAsync() =>
        await GetAsync<List<FriendModel>>( "/friends");

    public async Task<List<FriendRequestModel>?> GetAllReceivedFriendRequestsAsync() =>
        await GetAsync<List<FriendRequestModel>>( "/friends/incoming-requests");

    public async Task<bool?> EndFriendshipAsync(string username) =>
        await DeleteAsync($"/friends/{username}");

    // ----- EMISSIONS CONTROLLER -----
    public async Task<double?> CalculateEmissionsAsync(EmissionsModel model) =>
        await PostAsync<double>("/api/emissions", model);

    public async Task<double?> GetEmissionsOfWeekAsync(string week) =>
        await GetAsync<double>( $"/api/emissions/{week}");

    public async Task<List<WasteModel>?> GetAllWasteTypesAsync() =>
        await GetAsync<List<WasteModel>>( "/api/emissions/waste");

    public async Task<List<LeaderboardModel>?> GetWeeklyLeaderboardAsync() =>
        await GetAsync<List<LeaderboardModel>>( "/api/emissions/leaderboard");

    public async Task<List<EmissionsHistoryModel>?> GetEmissionHistoryAsync() =>
        await GetAsync<List<EmissionsHistoryModel>>( "/api/emissions/history");

    public async Task<List<EnergyConsumptionModel>?> GetAllEnergyConsumptionTypesAsync() =>
        await GetAsync<List<EnergyConsumptionModel>>( "/api/emissions/energy");

    public async Task<List<DietModel>?> GetAllDietsAsync() =>
        await GetAsync<List<DietModel>>( "/api/emissions/diet");

    public async Task<double?> GetEmissionsOfCurrentWeekAsync() =>
        await GetAsync<double>( "/api/emissions/current");
}