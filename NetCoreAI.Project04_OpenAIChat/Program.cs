using System.Text; 
using System.Text.Json; 

class Program
{
	static async Task Main(string[] args)
	{
		// API anahtarını tanımlıyoruz (gerçek projelerde gizli tutulmalı)
		var apiKey = "apikey";

		// Kullanıcıdan bir soru alıyoruz
		Console.WriteLine("Lütfen sorunuzu yazınız : (örneğin: 'Merhaba, bugün hava Ankara'da kaç derece')");
		var prompt = Console.ReadLine(); 

		// HTTP istemcisi oluşturuluyor
		using var httpClient = new HttpClient();

		// API istekleri için gerekli olan başlıklar ayarlanıyor
		httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}"); // Yetkilendirme
		httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://openrouter.ai/"); // Referans sayfa (zorunlu olabilir)
		httpClient.DefaultRequestHeaders.Add("X-Title", "OpenAIChat"); // İsteğe bağlı bir başlık

		// API'ye gönderilecek olan istek verisi (JSON formatında)
		var requestBody = new
		{
			model = "openai/gpt-3.5-turbo", // Kullanılacak model
			messages = new[]
			{
				new {role="system", content = "You are a helpful assistant."}, // Sistemin rolü ve ön tanımlı davranışı
				new {role="user", content = prompt ?? ""} // Kullanıcının sorusu
			},
			max_tokens = 500 // Cevap uzunluğu için maksimum token sayısı
		};

		// JSON formatına çevirip içerik olarak oluşturuyoruz
		var json = JsonSerializer.Serialize(requestBody);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		try
		{
			// API'ye POST isteği gönderiliyor
			var response = await httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

			// Dönen cevabın içeriğini string olarak al
			var responseString = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				// JSON cevabını çözümle
				var result = JsonSerializer.Deserialize<JsonElement>(responseString);

				// İçinden "content" alanını al (Chatbot'un cevabı)
				var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

				// Cevabı ekrana yazdır
				Console.WriteLine("Open AI Cevabı: ");
				Console.WriteLine(answer);
				Console.ReadLine(); // Kullanıcının görmesi için bekle
			}
			else
			{
				// Hata oluştuysa hata mesajını yazdır
				Console.WriteLine($"Bir hata oluştu: {response.StatusCode}");
				Console.WriteLine(responseString);
				Console.ReadLine();
			}
		}
		catch (Exception ex)
		{
			// Herhangi bir beklenmeyen hata oluşursa burada yakalanır
			Console.WriteLine($"Bir hata oluştu: {ex.Message}");
			Console.ReadLine();
		}
	}
}
