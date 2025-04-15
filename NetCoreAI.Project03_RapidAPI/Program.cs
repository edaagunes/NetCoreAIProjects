using NetCoreAI.Project03_RapidAPI.ViewModels;
using Newtonsoft.Json;

var client = new HttpClient();
List<ApiSeriesViewModel> list = new List<ApiSeriesViewModel>();

var request = new HttpRequestMessage
{
	Method = HttpMethod.Get,
	RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/series/"),
	Headers =
	{
		{ "x-rapidapi-key", "8189ffcc04msh89957af40346776p12f8ebjsndc8eef993d4e" },
		{ "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
	},
};
using (var response = await client.SendAsync(request))
{
	response.EnsureSuccessStatusCode();
	var body = await response.Content.ReadAsStringAsync();
	list = JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body);
	foreach (var series in list)
	{
		Console.WriteLine(series.rank + "-" + series.title
						  + "-Film Puanı: " +series.rating 
						  + "-Yapım Yılı: " +series.year);
	}
}

Console.ReadLine();
