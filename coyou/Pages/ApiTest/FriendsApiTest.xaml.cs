using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Services;

namespace coyou.Pages.ApiTest;

public partial class FriendsApiTest : ContentPage
{
    private readonly FriendService _friendService;


    public FriendsApiTest(FriendService friendService)
    {
        InitializeComponent();
        _friendService = friendService;

    }

    private async void SendFriendRequest(object? sender, EventArgs e)
    {
        var result = await _friendService.SendFriendRequest(SendFriendRequestEntry.Text);
        SendFriendRequestLabel.Text = result ?? string.Empty;
    }

    private async void AcceptFriendRequest(object? sender, EventArgs e)
    {
        var result = await _friendService.AcceptFriendRequest(AcceptFriendRequestEntry.Text);
        AcceptFriendRequestLabel.Text = result ?? string.Empty;
    }

    private async void GetAllFriends(object? sender, EventArgs e)
    {
        var result = await _friendService.GetAllFriends();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllFriendsLabel.Text = s ?? string.Empty;
    }

    private async void GetAllReceivedFriendRequests(object? sender, EventArgs e)
    {
        var result = await _friendService.GetAllReceivedFriendRequests();
        var s = "";
        result?.ForEach(model => s += model + "\n");
        GetAllReceivedFriendRequestsLabel.Text = s ?? string.Empty;
    }

    private async void EndFriendship(object? sender, EventArgs e)
    {
        var result = await _friendService.EndFriendShip(EndFriendshipEntry.Text);
        EndFriendshipLabel.Text = result.ToString() ?? string.Empty;
    }
}