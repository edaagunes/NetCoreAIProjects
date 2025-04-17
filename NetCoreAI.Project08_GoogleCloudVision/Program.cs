using Google.Cloud.Vision.V1; 

class Program
{
	static void Main(string[] args)
	{
		Console.Write("Resim yolunu giriniz"); 
		string imagePath = Console.ReadLine(); 
		Console.WriteLine();

		string credentialPath = @"C:\Users\EdaGunes\OneDrive\Masaüstü\tastyapi-447622-d6ab8b4e3062.json"; // Google Cloud projesine ait servis hesabı JSON dosyasının yolu (kimlik doğrulama için gerekli)

		// JSON dosyası üzerinden kimlik doğrulama yapabilmek için ortam değişkeni ayarlanır
		Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath); // Google Vision API'ye bu kimlik ile başvurulur

		try
		{
			var client = ImageAnnotatorClient.Create(); // Google Vision istemcisi oluşturulur

			var image = Image.FromFile(imagePath); // Belirtilen yoldaki resim dosyası yüklenir
			var response = client.DetectText(image); // Vision API ile resimdeki metinler tespit edilir (OCR işlemi yapılır)

			Console.WriteLine("Resimdeki metin: ");
			Console.WriteLine();

			foreach (var annotation in response) // Her bir bulunan metin bilgisi üzerinde döngü yapılır
			{
				if (!string.IsNullOrEmpty(annotation.Description)) // Eğer metin boş değilse
				{
					Console.WriteLine(annotation.Description); // Metin ekrana yazdırılır
				}
			}
		}
		catch (Exception ex) 
		{
			Console.WriteLine($"Bir hata oluştu", ex.Message); 
		}
	}
}
