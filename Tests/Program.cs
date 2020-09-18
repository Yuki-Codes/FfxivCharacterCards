using System;
using System.Threading.Tasks;

namespace Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			Task.Run(Main).Wait();
		}

		private static async Task Main()
		{
			// The lodestone character Id, can be seen at the end of lodestone URLs
			// such as: https://na.finalfantasyxiv.com/lodestone/character/1/
			uint characterId = 1;

			// Your Xiv API key, obtainable from https://xivapi.com/account
			string xivApiKey = "Your XIV Api Key";

			string path = await FfxivCharacterCards.Card.Generate(characterId, xivApiKey);
			Console.WriteLine(path);
		}
	}
}
