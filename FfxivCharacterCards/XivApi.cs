// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	internal static class XivApi
	{
		/// <summary>
		/// Get Character data, this is parsed straight from Lodestone in real-time. The more data you request the slower the entire request will be.
		/// </summary>
		public static async Task<GetResponse> GetCharacter(uint? id, string xivApiKey)
		{
			string route = "https://xivapi.com/character/" + id + "?data=FC,CJ&extended=true&private_key=" + xivApiKey;
			return await JsonWebRequest.Send<GetResponse>(route);
		}

		[Serializable]
		public class GetResponse
		{
			public Character? Character { get; set; }
			public FreeCompany? FreeCompany { get; set; }
		}

		[Serializable]
		public class Character
		{
			public ClassJob? ActiveClassJob { get; set; }
			public string Avatar { get; set; } = string.Empty;
			public string Bio { get; set; } = string.Empty;
			public List<ClassJob>? ClassJobs { get; set; }
			public string DC { get; set; } = string.Empty;
			public string FreeCompanyId { get; set; } = string.Empty;
			public uint Gender { get; set; }
			public GrandCompany? GrandCompany { get; set; }
			public Data? GuardianDeity { get; set; }
			public uint ID { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Nameday { get; set; } = string.Empty;
			public uint ParseDate { get; set; }
			public string Portrait { get; set; } = string.Empty;
			public Data? Race { get; set; }
			public string Server { get; set; } = string.Empty;
			public Data? Title { get; set; }
			public bool TitleTop { get; set; }
			public Data? Town { get; set; }
			public Data? Tribe { get; set; }

			public ClassJob? GetClassJob(Jobs id)
			{
				if (this.ClassJobs == null)
					return null;

				foreach (ClassJob job in this.ClassJobs)
				{
					if (job.Job == null)
						return null;

					if (job.Job.ID != (uint)id)
						continue;

					return job;
				}

				return null;
			}
		}

		[Serializable]
		public class ClassJob
		{
			public Class? Class { get; set; }
			public ulong ExpLevel { get; set; } = 0;
			public ulong ExpLevelMax { get; set; } = 0;
			public ulong ExpLevelTogo { get; set; } = 0;
			public bool IsSpecialised { get; set; } = false;
			public Class? Job { get; set; }
			public int Level { get; set; } = 0;
			public string Name { get; set; } = string.Empty;
		}

		[Serializable]
		public class FreeCompany
		{
			public uint ActiveMemberCount { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Tag { get; set; } = string.Empty;
			public string Slogan { get; set; } = string.Empty;
			public List<string> Crest { get; set; } = new List<string>();
		}

		[Serializable]
		public class Class
		{
			public string Abbreviation { get; set; } = string.Empty;
			public uint ID { get; set; }
			public string Icon { get; set; } = string.Empty;
			public string Name { get; set; } = string.Empty;
			public string Url { get; set; } = string.Empty;
		}

		[Serializable]
		public class GrandCompany
		{
			public Data? Company { get; set; }
			public Data? Rank { get; set; }
		}

		[Serializable]
		public class Data
		{
			public int ID { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Icon { get; set; } = string.Empty;
			public string Url { get; set; } = string.Empty;
		}
	}
}
