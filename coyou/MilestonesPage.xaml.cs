using coyou.Services;
using Microsoft.Maui.Controls;

namespace coyou
{
    public partial class MilestonesPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly EmissionsService _emissionsService;
        private readonly RouteService _routeService;

        public MilestonesPage(UserService userService, FriendService friendService, EmissionsService emissionsService, RouteService routeService)
        {
            InitializeComponent();
            _userService = userService;
            _friendService = friendService;
            _emissionsService = emissionsService;
            _routeService = routeService;
        }
    }
}
