using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coyou.Services;

namespace coyou.Pages.ApiTest;

public partial class UserApiTest : ContentPage
{
    private readonly UserService _userService;

    public UserApiTest(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void EditUser(object? sender, EventArgs e)
    {
        var user = await _userService.EditUser(FirstNameEntry.Text, LastNameEntry.Text, UserNameEntry.Text);
        EditUserLabel.Text = user?.ToString() ?? string.Empty;
    }

    private async void GetUser(object? sender, EventArgs e)
    {
        var user = await _userService.GetUser();
        GetUserLabel.Text = user?.ToString() ?? string.Empty;
        ProfilePictureView.Source = user?.GetProfileImageSource();
    }

    private async void SelectProfilePicture(object? sender, EventArgs e)
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null) return;
        var stream = await result.OpenReadAsync();
        ProfilePicturePreview.Source = ImageSource.FromStream(() => stream);
    }
}