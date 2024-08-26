namespace Shared;

public class WeatherForecast
{
    public WeatherForecast(DateOnly date, int temp, string? summary)
    {
        this.Date = date;
        this.TemperatureC = temp;
        this.Summary = summary;
    }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
