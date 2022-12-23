using ProcessManager.ManageServices;
using System.Text.Json;

var weatherForecast = new WeatherForecast
{
    Date = DateTime.Parse("2019-08-01"),
    TemperatureCelsius = 25,
    Summary = "Hot"
};
string jsonString = JsonSerializer.Serialize(weatherForecast);
for (int i = 0; i < 5; i++)
{
    //Console.WriteLine(@$"{i}Hello, World!");
    Console.WriteLine(jsonString);
    Task.Delay(500).Wait();
}