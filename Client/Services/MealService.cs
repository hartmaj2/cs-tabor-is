using System.Net.Http.Json;

// Used by components that need information about all possible allergens

public class MealService
{
    private readonly HttpClient _httpClient;
    private List<string>? _mealTypes;

    // HttpClient is passed to this constructor automatically
    public MealService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Gets the allergens from api only if not loaded already
    public async Task<List<string>> GetMealTypesAsync()
    {
        if (_mealTypes == null)
        {
            _mealTypes = await _httpClient.GetFromJsonAsync<List<string>>("api/meals/meal-types");
        }
        return _mealTypes!;
    }
}