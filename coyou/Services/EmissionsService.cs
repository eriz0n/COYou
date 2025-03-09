namespace coyou.Services;

public class EmissionsService
{
    private readonly DummyDataService _dummyDataService;
    private readonly ApiService _apiService;
    private readonly UserService _userService;

    public EmissionsService(DummyDataService dummyDataService, ApiService apiService, UserService userService)
    {
        _dummyDataService = dummyDataService;
        _apiService = apiService;
        _userService = userService;
    }

    public async Task<bool> WriteDiet(string diet)
    {
        var user = await _userService.GetUser();
        var diets = await GetAllDiets();
        foreach (var model in diets)
        {
            if (model.DietType == diet)
            {
                Preferences.Set(user.Username + "_diet_type", diet);
                return true;
            }
        }

        return false;
    }
    public async Task<string> ReadDiet()
    {
        var user = await _userService.GetUser();
        return Preferences.Get(user.Username + "_diet_type", "");
    }    
    public async Task SetEmissionDataDone()
    {
        var user = await _userService.GetUser();
        Preferences.Set(user.Username + "_emission_data_done", true);
    }
    public async Task<bool> IsEmissionDataDone()
    {
        var user = await _userService.GetUser();
        if (user == null)
            return false;
        return Preferences.Get(user.Username + "_emission_data_done", false);
    }
    public async Task<bool> WriteWaste(string wasteType, double kg)
    {
        var user = await _userService.GetUser();

        if (kg <= 0)
        {
            Preferences.Remove(user.Username + "_waste_" + wasteType);
            return true;
        }
        var wasteTypes = await GetAllWasteTypes();
        foreach (var model in wasteTypes)
        {
            if (model.Type == wasteType)
            {
                Preferences.Set(user.Username + "_waste_" + wasteType, kg);
                return true;
            }
        }

        return false;
    }
    public async Task<Dictionary<string, double>> ReadWaste()
    {
        var user = await _userService.GetUser();

        var wasteDict = new Dictionary<string, double>();
        var wasteTypes = await GetAllWasteTypes();
        foreach (var model in wasteTypes)
        {
            var kwh = Preferences.Get(user.Username + "_waste_" + model.Type, -1d);
            if (kwh <= 0)
                continue;
            wasteDict.Add(model.Type, kwh);
        }
        return wasteDict;
    }

    public async Task<bool> WriteEnergy(string energyType, double kwh)
    {
        var user = await _userService.GetUser();

        if (kwh <= 0)
        {
            Preferences.Remove(user.Username + "_energy_" + energyType);
            return true;
        }
        var energyTypes = await GetAllEnergyConsumptionTypes();
        foreach (var model in energyTypes)
        {
            if (model.Type == energyType)
            {
                Preferences.Set(user.Username + "_energy_" + energyType, kwh);
                return true;
            }
        }

        return false;
    }

    public async Task<Dictionary<string, double>> ReadEnergy()
    {
        var user = await _userService.GetUser();

        var energyDict = new Dictionary<string, double>();
        var energyTypes = await GetAllEnergyConsumptionTypes();
        foreach (var model in energyTypes)
        {
            var kwh = Preferences.Get(user.Username + "_energy_" + model.Type, -1d);
            if (kwh <= 0)
                continue;
            energyDict.Add(model.Type, kwh);
        }
        return energyDict;
    }



    public async Task<bool> IsEmissionCalculationAvailable()
    {
        var weekAlreadyCalculated = await GetEmissionsOfCurrentWeek() > 0;
        var emissionDataDone = await IsEmissionDataDone();
        
        return !weekAlreadyCalculated && emissionDataDone;
    }
    public async Task<EmissionsModel> GenerateEmissionsModel(string? mainTransportationType = null, int? weeklyKm = null)
    {
        double? routeEmissions = null;
        var diet = await ReadDiet();
        var waste = await ReadWaste();
        var energy = await ReadEnergy();

        if (mainTransportationType != null && weeklyKm != null)
        {
            var types = await _apiService.GetAllTypesAsync();
            foreach (var type in types)
            {
                if (type.Name != mainTransportationType) continue;
                routeEmissions = (double)type.GramsPerKilometer! * (double) weeklyKm;
                break;

            }
        }

        var model = new EmissionsModel(routeEmissions, diet, waste, energy);
        return model;
    }

    public async Task<double?> CalculateEmissions(EmissionsModel emissions)
    {
        if (!await IsEmissionCalculationAvailable())
            return null;
        if (Constants.UseDummyData)
        {
            return _dummyDataService.CalculateEmissions(emissions);
        }

        return await _apiService.CalculateEmissionsAsync(emissions);
    }

    public async Task<double?> GetEmissionsOfWeek(int year, int weekNumber)
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetEmissionsOfWeek();
        }

        return await _apiService.GetEmissionsOfWeekAsync(string.Concat(year, "-W", weekNumber));
    }

    public async Task<List<WasteModel>?> GetAllWasteTypes()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetAllWasteTypes();
        }

        return await _apiService.GetAllWasteTypesAsync();
    }

    public async Task<List<LeaderboardModel>?> GetWeeklyLeaderboard()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetWeeklyLeaderboard();
        }

        return await _apiService.GetWeeklyLeaderboardAsync();
    }

    public async Task<List<EmissionsHistoryModel>?> GetEmissionHistory()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetEmissionHistory();
        }

        return await _apiService.GetEmissionHistoryAsync();
    }

    public async Task<List<EnergyConsumptionModel>?> GetAllEnergyConsumptionTypes()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetAllEnergyConsumptionTypes();
        }

        return await _apiService.GetAllEnergyConsumptionTypesAsync();
    }

    public async Task<List<DietModel>?> GetAllDiets()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetAllDiets();
        }

        return await _apiService.GetAllDietsAsync();
    }

    public async Task<double?> GetEmissionsOfCurrentWeek()
    {
        if (Constants.UseDummyData)
        {
            return _dummyDataService.GetEmissionsOfCurrentWeek();
        }

        return await _apiService.GetEmissionsOfCurrentWeekAsync();
    }
}