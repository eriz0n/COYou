using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Services
{
    public class RouteService
    {
        private readonly DummyDataService _dummyDataService;
        private readonly ApiService _apiService;

        public RouteService(DummyDataService dummyDataService, ApiService apiService)
        {
            _dummyDataService = dummyDataService;
            _apiService = apiService;
        }

        public async Task<double?> EditRoute(long id, RouteModel route)
        {
            if (Constants.UseDummyData)
                return _dummyDataService.EditRoute(id, route);

            return await _apiService.EditRouteAsync(id, route);

        }
        public async Task<bool?> DeleteRoute(long id)
        {
            if (Constants.UseDummyData)
                return _dummyDataService.DeleteRoute(id);

            return await _apiService.DeleteRouteAsync(id);

        }
        public async Task<List<FullRouteModel>?> GetUserRoutes()
        {
            if (Constants.UseDummyData)
                return _dummyDataService.GetUserRoutes();

            return await _apiService.GetUserRoutesAsync();

        }
        public async Task<double?> AddRoute(RouteModel route)
        {
            if (Constants.UseDummyData)
                return _dummyDataService.AddRoute(route);
            

            return await _apiService.AddRouteAsync(route);

        }

        public async Task<List<TypeModel>?> GetAllTypes()
        {
            if (Constants.UseDummyData)
                return _dummyDataService.GetAllTypes();

            return await _apiService.GetAllTypesAsync();

        }
        public async Task<List<HistoryModel>?> GetHistory(DateOnly startDate, DateOnly endDate)
        {
            if (Constants.UseDummyData)
                return _dummyDataService.GetHistory();

            return await _apiService.GetHistoryAsync(new HistoryRequestModel(startDate, endDate));

        }

    }
}
