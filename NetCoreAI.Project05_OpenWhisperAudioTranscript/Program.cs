using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text;
using AssemblyAI;
using AssemblyAI.Transcripts;

class Program
{
	static readonly HttpClient httpClient = new HttpClient();
	static async Task Main(string[] args)
	{
		string apiKey = "apikey";
		string audioFilePath = "audio1.mp3";

		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			var form = new MultipartFormDataContent();

			var audioContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
			audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
			form.Add(audioContent, "file", Path.GetFileName(audioFilePath));
			form.Add(new StringContent("whisper-1"), "model");

			Console.WriteLine("Ses Dosyası İşleniyor, Lütfen Bekleyiniz......");

			var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				Console.WriteLine("Transkript: ");
				Console.WriteLine(result);
				Console.ReadLine();
			}
			else
			{
				Console.WriteLine($"Hata: {response.StatusCode}");
				Console.WriteLine(await response.Content.ReadAsStringAsync());
				Console.ReadLine();
			}
		}
	}
}

