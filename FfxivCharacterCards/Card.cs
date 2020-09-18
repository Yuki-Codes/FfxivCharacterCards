// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using System.Threading.Tasks;

	public static class Card
	{
		public static async Task<string> Generate(uint id, string xivApiKey, string tempDir = "./Temp/", string assetDir = "./Assets/")
		{
			CharacterInfo character = await CharacterInfo.Get(id, xivApiKey);
			return await ImageGenerator.Draw(character, tempDir, assetDir);
		}
	}
}
