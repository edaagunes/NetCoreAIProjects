using Newtonsoft.Json;
using System.Text;

class Program
{
	public static async Task Main(string[] args)
	{
		string apiKey = "key";
		Console.Write("Example prompts: ");
		string prompt = Console.ReadLine();
		using (HttpClient client = new HttpClient())
		{
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
			var requestBody = new
			{
				prompt = prompt,
				n = 1,
				size = "1024x1024"
			};

			string jsonBody = JsonConvert.SerializeObject(requestBody);
			var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);
			string responseString = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseString);
			Console.ReadLine();
		}
	}
}