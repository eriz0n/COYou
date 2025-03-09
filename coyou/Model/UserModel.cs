using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace coyou;

public class UserModel
{
    public UserModel()
    {
    }

    public UserModel(string? username, string? firstName, string? lastName, string? profilePictureHash)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        ProfilePictureHash = profilePictureHash;
    }

    [JsonProperty("username")] public string? Username { get; set; }
    [JsonProperty("firstName")] public string? FirstName { get; set; }
    [JsonProperty("lastName")] public string? LastName { get; set; }
    [JsonProperty("profilePictureHash")] public string? ProfilePictureHash { get; set; }

    protected bool Equals(UserModel other)
    {
        return Username == other.Username && FirstName == other.FirstName && LastName == other.LastName && ProfilePictureHash == other.ProfilePictureHash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UserModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Username, FirstName, LastName, ProfilePictureHash);
    }

    public ImageSource GetProfileImageSource()
    {
        try
        {
            if (ProfilePictureHash != null)
            {
                byte[] imageBytes = Convert.FromBase64String(ProfilePictureHash);
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting Base64 to ImageSource: {ex.Message}");
        }
        return ImageSource.FromFile("dotnet_bot.png");

    }

    public Image GetProfileImage()
    {
        return new Image
        {
            Source = GetProfileImageSource(),
            WidthRequest = 96,
            HeightRequest = 96
        };
    }
    public override string? ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

