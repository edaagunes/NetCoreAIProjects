using Newtonsoft.Json; // JSON verilerini dönüştürmek için kullanılan kütüphane (serileştirme ve ayrıştırma)
using System.Text; // Metin verileriyle çalışmak için gerekli (örneğin UTF8 encoding)

class Program
{
	private static async Task Main(string[] args) // Ana program girişi (async olduğu için API çağrısı beklenebilir)
	{
		// Kullanıcıdan çevirmek istediği cümleyi alıyoruz
		Console.Write("Çevirmek istediğiniz cümleyi giriniz: ");
		string inputText = Console.ReadLine(); // Konsoldan kullanıcı girişi

		// OpenAI API anahtarı (gerçek projelerde gizli tutulmalıdır!)
		string apiKey = "apikey";

		// Çeviri yapan metodu çağırıyoruz ve sonucu alıyoruz
		string translatedText = await TranslateTextToEnglish(inputText, apiKey);

		// Eğer çeviri başarılıysa sonucu yazdır
		if (!string.IsNullOrEmpty(translatedText))
		{
			Console.WriteLine();
			Console.Write($"Çeviri (İngilizce): {translatedText}");
			Console.WriteLine();
		}
		else
		{
			// Eğer boş ya da null geldiyse hata mesajı göster
			Console.Write("Beklenmeyen bir hata oluştu");
		}

		Console.ReadLine(); // Program hemen kapanmasın diye kullanıcıdan tuş bekleniyor
	}

	// Verilen metni İngilizce'ye çevirmek için OpenAI API'ye istek gönderen metot
	private static async Task<string> TranslateTextToEnglish(string text, string apiKey)
	{
		// HTTP isteği yapmak için HttpClient nesnesi oluşturuluyor
		using (HttpClient client = new HttpClient())
		{
			// API çağrısında yetkilendirme için Authorization başlığı ekleniyor
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			// API’ye gönderilecek veri yapısı hazırlanıyor
			var requestBody = new
			{
				model = "gpt-3.5-turbo", // Kullanılacak yapay zeka modeli
				messages = new[] // Chat tarzında mesaj listesi
				{
					new { role = "system", content = "You are a helpful translator" }, // Modelin nasıl davranacağı belirleniyor
					new { role = "user", content = $"Please translate this text to English: {text}" } // Kullanıcının çeviri isteği
				}
			};

			// C# nesnesi JSON formatına çevriliyor
			string jsonBody = JsonConvert.SerializeObject(requestBody);

			// JSON içeriği, HTTP POST isteğine uygun şekilde hazırlanıyor
			var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

			try
			{
				// OpenAI API’ye HTTP POST isteği gönderiliyor
				HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

				// API’den gelen cevap string olarak alınıyor
				string responseString = await response.Content.ReadAsStringAsync();

				// JSON yanıtı dinamik bir nesneye dönüştürülüyor
				dynamic responseObject = JsonConvert.DeserializeObject(responseString);

				// Yanıt içindeki çeviri metni alınıyor
				string translation = responseObject.choices[0].message.content;

				// Çeviri döndürülüyor
				return translation;
			}
			catch (Exception ex)
			{
				// Hata durumunda mesaj yazdırılıyor ve null dönülüyor
				Console.WriteLine($"Bir hata oluştu: {ex.Message}");
				return null;
			}
		}
	}
}
