using System.Net.Http.Json;

// Used by components that need information about all possible allergens

public class AllergenService
{
    private readonly HttpClient _httpClient;
    private List<AllergenDto>? _allergens;

    // HttpClient is passed to this constructor automatically
    public AllergenService(HttpClient httpClient)
    {
        Console.WriteLine("allergen service created");
        _httpClient = httpClient;
    }

    // Gets the allergens from api only if not loaded already
    public async Task<List<AllergenDto>> GetAllergenDtosAsync()
    {
        if (_allergens == null)
        {
            Console.WriteLine("using the allergen service for the first time");
            _allergens = await _httpClient.GetFromJsonAsync<List<AllergenDto>>("api/allergens/all");
        }
        Console.WriteLine("using the allergen service again");
        return _allergens!;
    }
}