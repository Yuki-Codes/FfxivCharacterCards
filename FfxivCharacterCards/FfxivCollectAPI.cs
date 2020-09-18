// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using System.Threading.Tasks;

	internal static class FfxivCollectAPI
	{
		public static async Task<Character?> Get(uint id)
		{
			try
			{
				return await JsonWebRequest.Send<Character>("https://ffxivcollect.com/api/characters/" + id);
			}
			catch (Exception)
			{
				return null;
			}
		}

		[Serializable]
		public class Character
		{
			public uint Id { get; set; }
			public Data? Achievements { get; set; }
			public Data? Mounts { get; set; }
			public Data? Minions { get; set; }
			public Data? Orchestrions { get; set; }
			public Data? Emotes { get; set; }
			public Data? Bardings { get; set; }
			public Data? Hairstyles { get; set; }
			public Data? Armoires { get; set; }
			public Data? Triad { get; set; }
		}

		[Serializable]
		public class Data
		{
			public int Count { get; set; }
			public int Total { get; set; }
		}
	}
}
