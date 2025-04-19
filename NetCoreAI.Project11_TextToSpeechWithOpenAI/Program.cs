// Gerekli kütüphaneler projeye dahil ediliyor
using Newtonsoft.Json; // JSON formatında veri oluşturmak ve çözümlemek için kullanılır
using System.Text; // Metin kodlama işlemleri için kullanılır

class Program
{
	// OpenAI API anahtarını saklamak için kullanılan sabit alan (gizli tutulmalı)
	private static readonly string apiKey = "";

	// Programın ana çalıştırılabilir metodu (asenkron olarak çalışır)
	public static async Task Main(string[] args)
	{
		Console.Write("Metni giriniz: "); // Kullanıcıdan metin girişi istenir
		string input = Console.ReadLine(); // Girilen metin input değişkenine atanır

		// Eğer kullanıcı boş bir metin girmezse işlemler başlatılır
		if (!string.IsNullOrEmpty(input))
		{
			Console.WriteLine("Ses dosyası oluşturuluyor...");
			await GenerateSpeech(input); // Metni sese çeviren metod çağrılır
			Console.Write("Ses dosyası 'output.mp3' olarak kaydedildi");

			// Oluşturulan ses dosyasını sistemde açar
			System.Diagnostics.Process.Start("explorer.exe", "output.mp3");
		}
	}

	// Metni sese çeviren ve mp3 dosyası olarak kaydeden asenkron metot
	static async Task GenerateSpeech(string text)
	{
		// HTTP istekleri yapmak için HttpClient nesnesi kullanılır
		using (HttpClient client = new HttpClient())
		{
			// API'ye yetki sağlamak için istek başlığına API anahtarı eklenir
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			// API'ye gönderilecek JSON verisi oluşturulur
			var requestBody = new
			{
				model = "tts-1", // Kullanılacak metinden konuşmaya (text-to-speech) modeli
				input = text,    // Kullanıcının girdiği metin
				voice = "alloy"  // Kullanılacak ses türü (OpenAI'nin sağladığı seslerden biri)
			};

			// requestBody nesnesi JSON formatına dönüştürülür
			string json = JsonConvert.SerializeObject(requestBody);

			// JSON verisi UTF-8 kodlamasıyla içeriğe dönüştürülür ve Content-Type belirtilir
			HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

			// OpenAI API'ye POST isteği gönderilir (text-to-speech isteği)
			HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

			// İstek başarılıysa
			if (response.IsSuccessStatusCode)
			{
				// Dönen ses dosyası byte dizisi olarak alınır
				byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();

				// Dosya sistemine mp3 dosyası olarak yazılır
				await File.WriteAllBytesAsync("output.mp3", audioBytes);
			}
			else
			{
				// Hata durumunda kullanıcı bilgilendirilir
				Console.WriteLine("Bir hata oluştu");
			}
		}
	}
}
