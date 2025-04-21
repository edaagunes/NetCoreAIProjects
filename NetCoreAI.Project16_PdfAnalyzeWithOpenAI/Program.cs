using Newtonsoft.Json;
using System.Text;
using UglyToad.PdfPig;

class Program
{
	private static readonly string apiKey = "";

	static async Task Main(string[] args)
	{
		Console.WriteLine("PDF dosya yolunu giriniz: ");
		string pdfPath = Console.ReadLine();
		string pdfText = ExtractPatFromPdf(pdfPath);
		await AnalyzeWithAI(pdfText, "Pdf İçeriği");

		static string ExtractPatFromPdf(string filePath)
		{
			StringBuilder text = new StringBuilder();
			using (PdfDocument pdf = PdfDocument.Open(filePath))
			{
				foreach (var page in pdf.GetPages())
				{
					text.AppendLine(page.Text);
				}
			}
			return text.ToString();
		}

		static async Task AnalyzeWithAI(string text, string sourceType)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

				var requestBody = new
				{
					model = "gpt-3.5-turbo",
					messages = new[]
					{
						new { role = "system", content = "Sen bir yapay zeka asistanısın. Kullanıcının gönderdiği metni analiz eder ve Türkçe olarak özetlersin. Yanıtlarını sadece Türkçe ver" },
						new { role = "user", content = $"Analyze and summarize the following {sourceType}:\n\n {text}" }
					}
				};

				string json = JsonConvert.SerializeObject(requestBody);
				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

				string responseJson = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
					Console.WriteLine($"\n AI Analizi ({sourceType}): \n {result.choices[0].message.content}");
				}

				else
				{
					Console.WriteLine("Hata: " + responseJson);
				}
			}
		}
	}
}