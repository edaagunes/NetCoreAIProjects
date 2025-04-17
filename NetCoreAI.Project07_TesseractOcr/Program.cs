using Tesseract; // Tesseract OCR kütüphanesini projeye dahil eder

class Program
{
	static void Main(string[] args)
	{
		Console.Write("Karakter okuması yapılacak resim yolu:"); 
		string imagePath = Console.ReadLine(); 
		Console.WriteLine();

		string tessDataPath = @"C:\tessdata"; // Tesseract dil dosyalarının bulunduğu klasörün yolu

		try
		{
			// Tesseract motoru başlatılır (İngilizce dilinde ve varsayılan modda)
			using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
			{
				// Girilen resim yolu kullanılarak görsel yüklenir
				using (var img = Pix.LoadFromFile(imagePath))
				{
					// Görsel OCR işlemine sokulur
					using (var page = engine.Process(img))
					{
						// Görselden çıkarılan metin alınır
						string text = page.GetText();

						// Çıkarılan metin ekrana yazdırılır
						Console.WriteLine("Resimden Okunan Metin...");
						Console.WriteLine(text);
					}
				}
			}
		}
		catch (Exception ex) 
		{
			Console.WriteLine($"Bir hata oluştu: {ex.Message}"); 
		}

		Console.ReadLine(); 
	}
}