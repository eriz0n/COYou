using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Services
{
    public class UserService
    {
        private readonly DummyDataService _dummyDataService;
        private readonly ApiService _apiService;

        public UserService(DummyDataService dummyDataService, ApiService apiService)
        {
            _dummyDataService = dummyDataService;
            _apiService = apiService;
        }


        public async Task<UserModel?> GetUser()
        {
            if (Constants.UseDummyData)
                return _dummyDataService._user;
            
            return await _apiService.GetUserAsync();
        }        
        public async Task<UserModel?> EditUser(string? firstName, string? lastName, string? userName)
        {
            if (Constants.UseDummyData)
                return _dummyDataService.EditUser(firstName, lastName, userName);
            
            return await _apiService.EditUserAsync(new UserModel(userName, firstName, lastName, null));
        }

    }
}