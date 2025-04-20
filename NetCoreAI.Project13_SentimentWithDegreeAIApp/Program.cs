using Newtonsoft.Json;
using System.Text;

class Program
{
	private static readonly string apiKey = "";

	public static async Task Main(string[] args)
	{
		Console.Write("Bir metin giriniz: ");
		string input = Console.ReadLine();
		Console.WriteLine();

		if (!string.IsNullOrEmpty(input))
		{
			Console.WriteLine("Gelişmiş duygu analizi yapılıyor...");
			string sentiment = await AdvancedSentimentalAnalysis(input);
			Console.WriteLine();
			Console.WriteLine($"\n Sonuç: \n{sentiment}");
		}

		static async Task<string> AdvancedSentimentalAnalysis(string text)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

				var requestBody = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new {role="system", content="You are an advanced AI that analyzes emotions in text. Your response must be in JSON format. Identify the sentiment scores (0-100%) for the following emotions: Joy, Sadness, Anger, Fear, Surprise and Neutral."},
						new {role="user", content=$"Analyze this text: \"{text}\" and return a JSON object with percentages for each emotions"},

					}

				};
				string json = JsonConvert.SerializeObject(requestBody);
				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
				string responseJson = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
					string analyzsis = result.choices[0].message.content.ToString();
					return analyzsis;
				}
				else
				{
					Console.WriteLine($"Bir hata oluştu: {responseJson}");
					return "Hata";
				}
			}
		}
	}
}