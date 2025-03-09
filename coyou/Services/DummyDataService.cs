using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Services
{
    public class DummyDataService
    {
        internal UserModel _user { get; set; }
        internal Dictionary<long, RouteModel> _routes { get; }
        internal List<HistoryModel> _historyModels { get; }
        internal List<FriendModel> _friends { get; }
        internal List<FriendRequestModel> _friendRequests { get; }

        public DummyDataService()
        {
            _user = new UserModel("usermax", "Max", "Mustermann", "");
            _user.ProfilePictureHash =
                "iVBORw0KGgoAAAANSUhEUgAAAGAAAABgCAIAAABt+uBvAAAAA3NCSVQICAjb4U/gAAAABmJLR0QAaACfADiOTMCgAAABsElEQVR4nO3csUrDUBhA4SYpvoCu2lEncXQRBWffwFFBwQdwdHHppE/Q2QdwcBLdBRcFcVB0cCjUoYNtaRt3IT1Nm+aqnG+/9/4ccoeU0OigsV5Rtjj0AL+dgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgUC1tJP2t07XapvT79PufB5f7Ey/z5jKCxRHcRwlRexTwCY5jivzsL+ovCfoh7SSpulwgoXDdFD4MCMEC/TafKxf7oU6fXxeMWAgYCBgIGAgYCBgIGAgYCBgIBDsVSOJq6uLG7mWtDutl+bDjObJEizQ0vzy4XY915Knj7uzq6MZzZPFKwYMBIJdsf6g1xt0cy3p9r9mNMwIwQK9t579Peg/MBAwEDAQMBAwEDAQMBAwEDAQMBAwEAj2slpbWDnfvZ5sbeP25P7tpth5sgQLFEfJXHXCT6GSuMTvvko76Y8yEIj8i67RfIKAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIGAgYCBgIHAN4RNNQiDPaN4AAAAAElFTkSuQmCC";
            _routes = new Dictionary<long, RouteModel>
            {
                { 1, new RouteModel("Scheifling", "Leoben", "petrol car", 69) },
                { 2, new RouteModel("Leoben", "Scheifling", "diesel car", 69) },
                { 3, new RouteModel("Leoben", "Scheifling", "walking", 69) }
            };

            _historyModels = new List<HistoryModel>();
            _historyModels.Add(new HistoryModel(new DateTime(2025, 1, 1), 30));
            _historyModels.Add(new HistoryModel(new DateTime(2025, 1, 7), 20));
            _historyModels.Add(new HistoryModel(new DateTime(2025, 1, 14), 50));
            _historyModels.Add(new HistoryModel(new DateTime(2025, 1, 21), 10));

            _friends = new List<FriendModel>();
            _friends.Add(new FriendModel("user2", DateTime.Today));
            _friends.Add(new FriendModel("user3", DateTime.Today));
            _friends.Add(new FriendModel("testuser", new DateTime(2025, 1, 10)));

            _friendRequests =
            [
                new FriendRequestModel("hans456", DateTime.Today),
                new FriendRequestModel("franz123", DateTime.Today),
            ];
        }

        internal double? CalculateEmissions(EmissionsModel emissions)
        {
            return new Random().Next(200);
        }

        internal double? GetEmissionsOfWeek()
        {
            return new Random().Next(200);
        }

        internal List<WasteModel>? GetAllWasteTypes()
        {
            List<WasteModel> wasteTypes =
            [
                new("bio", 0.3),
                new("general", 0.6),
                new("glass", 0.55),
                new("metal", 8.2),
                new("paper", 1.5),
                new("plastic", 6.5),
            ];
            return wasteTypes;
        }

        internal List<LeaderboardModel>? GetWeeklyLeaderboard()
        {
            List<LeaderboardModel> leaderboard = new List<LeaderboardModel>();
            _friends.ForEach(model =>
            {
                LeaderboardModel leaderboardModel = new LeaderboardModel();
                leaderboardModel.Username = model.Username;
                leaderboardModel.TotalEmissions = new Random().Next(100);
                leaderboard.Add(leaderboardModel);
            });
            return leaderboard;
        }

        internal List<EmissionsHistoryModel>? GetEmissionHistory()
        {
            List<EmissionsHistoryModel> emissionsHistory = new List<EmissionsHistoryModel>();
            for (int i = 1; i < 10; i++)
            {
                var model = new EmissionsHistoryModel
                {
                    CalendarWeek = "2025-W" + i,
                    Emissions = new Random().Next(50, 150)
                };
                emissionsHistory.Add(model);
            }

            return emissionsHistory;
        }

        internal List<EnergyConsumptionModel>? GetAllEnergyConsumptionTypes()
        {
            List<EnergyConsumptionModel> consumptionTypes =
            [
                new("coal", 0.98),
                new("hydro", 0),
                new("natural_gas", 0.43),
                new("nuclear", 0.006),
                new("oil", 0.27),
                new("solar", 0.02),
                new("wind", 0.01),
            ];
            return consumptionTypes;
        }

        internal List<DietModel>? GetAllDiets()
        {
            List<DietModel> diets =
            [
                new("meat_based", 19.55),
                new("vegan", 4.61),
                new("vegetarian", 11.32),
            ];
            return diets;
        }

        internal double? GetEmissionsOfCurrentWeek()
        {
            return new Random().Next(200);
            ;
        }

        internal string? SendFriendRequest(string username)
        {
            return "Friend request sent!";
        }

        internal string? AcceptFriendRequest(string username)
        {
            foreach (var friend in _friendRequests)
            {
                if (friend.User == username)
                {
                    _friends.Add(new FriendModel(username, DateTime.Today));
                    _friendRequests.Remove(friend);
                    return "Friend request accepted!";
                }
            }

            return "Friend request not found";
        }

        internal List<FriendModel>? GetAllFriends()
        {
            return _friends;
        }

        internal List<FriendRequestModel>? GetAllReceivedFriendRequests()
        {
            return _friendRequests;
        }

        internal bool? EndFriendShip(string username)
        {
            foreach (var friend in _friends)
            {
                if (friend.Username == username)
                {
                    return _friends.Remove(friend);
                }
            }

            return false;
        }

        internal UserModel EditUser(string? firstName, string? lastName, string? userName)
        {
            if (firstName != null) _user.FirstName = firstName;
            if (lastName != null) _user.LastName = lastName;
            if (userName != null) _user.Username = userName;
            return _user;
        }

        internal bool DeleteRoute(long id)
        {

            return _routes.Remove(id);
        }

        internal double AddRoute(RouteModel route)
        {
            _routes.Add(_routes.Count + 1, route);
            return GetRouteEmissions(route);
        }

        internal List<TypeModel> GetAllTypes()
        {
            List<TypeModel> types =
            [
                new("bicycle", 21.0),
                new("diesel bus", 85.0),
                new("diesel car", 170.0),
                new("diesel train", 90.0),
                new("domestic flight plane", 255.0),
                new("e - bike", 12.0),
                new("electric bus", 20.0),
                new("electric car", 50.0),
                new("electric train", 27.0),
                new("hybrid car", 90.0),
                new("long-haul flight plane", 195.0),
                new("motorcycle", 103.0),
                new("petrol car", 192.0),
                new("walking", 0.0),
            ];
            return types;
        }

        internal List<HistoryModel> GetHistory()
        {
            List<HistoryModel> history = new List<HistoryModel>();
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                history.Add(new HistoryModel(new DateTime(2025, 1, i), random.Next(10, 10000)));
            }

            return history;
        }

        internal double EditRoute(long id, RouteModel updatedRoute)
        {
            foreach (var route in _routes)
            {
                if (route.Key == id)
                {
                    route.Value.LengthKm = updatedRoute.LengthKm;
                    route.Value.MovementType = updatedRoute.MovementType;
                    route.Value.Start = updatedRoute.Start;
                    route.Value.End = updatedRoute.End;
                }
            }

            return GetRouteEmissions(updatedRoute);
        }

        private double GetRouteEmissions(RouteModel route)
        {
            foreach (var type in GetAllTypes())
            {
                if (route.MovementType == type.Name)
                    return (double)(route.LengthKm * type.GramsPerKilometer)!;
            }
            return 0;
            
        }

        internal List<FullRouteModel> GetUserRoutes()
        {
            List<FullRouteModel> routes = [];
            foreach (var kvp in _routes)
            {
                FullRouteModel fullRoute = new FullRouteModel();
                fullRoute.Id = kvp.Key;
                fullRoute.LengthKm = kvp.Value.LengthKm;
                fullRoute.Emissions = GetRouteEmissions(kvp.Value);
                fullRoute.MovementType = kvp.Value.MovementType;
                fullRoute.Start = kvp.Value.Start;
                fullRoute.End = kvp.Value.End;
                routes.Add(fullRoute);

            }
            return routes;
        }
    }
}