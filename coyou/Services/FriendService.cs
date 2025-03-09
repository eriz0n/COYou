using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Services
{
    public class FriendService
    {
        private readonly DummyDataService _dummyDataService;
        private readonly ApiService _apiService;

        public FriendService(DummyDataService dummyDataService, ApiService apiService)
        {
            _dummyDataService = dummyDataService;
            _apiService = apiService;
        }

        public async Task<string?> SendFriendRequest(string username)
        {
            if (Constants.UseDummyData)
            {
                return _dummyDataService.SendFriendRequest(username);
            }

            return await _apiService.SendFriendRequestAsync(username);
        }

        public async Task<string?> AcceptFriendRequest(string username)
        {
            if (Constants.UseDummyData)
            {
                return _dummyDataService.AcceptFriendRequest(username);
            }

            return await _apiService.AcceptFriendRequestAsync(username);
        }

        public async Task<List<FriendModel>?> GetAllFriends()
        {
            if (Constants.UseDummyData)
            {
                return _dummyDataService.GetAllFriends();
            }

            return await _apiService.GetAllFriendsAsync();
        }

        public async Task<List<FriendRequestModel>?> GetAllReceivedFriendRequests()
        {
            if (Constants.UseDummyData)
            {
                return _dummyDataService.GetAllReceivedFriendRequests();
            }

            return await _apiService.GetAllReceivedFriendRequestsAsync();
        }

        public async Task<bool?> EndFriendShip(string username)
        {
            if (Constants.UseDummyData)
            {
                return _dummyDataService.EndFriendShip(username);
            }

            return await _apiService.EndFriendshipAsync(username);
        }
    }
}