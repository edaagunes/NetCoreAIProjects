using System.Speech.Synthesis;

class Program
{
	static void Main(string[] args)
	{
		SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

		speechSynthesizer.Volume = 50;
		speechSynthesizer.Rate = -4;

		Console.Write("Metni giriniz: ");
		string input=Console.ReadLine();

		if (!string.IsNullOrEmpty(input))
		{
			speechSynthesizer.Speak(input);
		}

		Console.ReadLine();
	}
}