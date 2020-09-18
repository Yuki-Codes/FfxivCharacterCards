// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System.Threading.Tasks;

	internal class CharacterInfo
	{
		private FfxivCollectAPI.Character? ffxivCollectCharacter;
		private XivApi.Character? xivApiCharacter;
		private XivApi.FreeCompany? freeCompany;

		public uint Id { get; private set; }
		public string? Portrait => this.xivApiCharacter?.Portrait;
		public string? Name => this.xivApiCharacter?.Name;
		public string? Title => this.xivApiCharacter?.Title?.Name;
		public string? Race => this.xivApiCharacter?.Race?.Name;
		public string? Tribe => this.xivApiCharacter?.Tribe?.Name;
		public string? NameDay => this.xivApiCharacter?.Nameday;
		public XivApi.Data? GuardianDeity => this.xivApiCharacter?.GuardianDeity;
		public XivApi.GrandCompany? GrandCompany => this.xivApiCharacter?.GrandCompany;
		public XivApi.FreeCompany? FreeCompany => this.freeCompany;
		public string? Server => this.xivApiCharacter?.Server;
		public string? DataCenter => this.xivApiCharacter?.DC;
		public string? Bio => this.xivApiCharacter?.Bio;

		public bool HasMounts => this.ffxivCollectCharacter != null && this.ffxivCollectCharacter.Mounts != null && this.ffxivCollectCharacter.Mounts.Count > 0;
		public bool HasMinions => this.ffxivCollectCharacter != null && this.ffxivCollectCharacter.Minions != null && this.ffxivCollectCharacter.Minions.Count > 0;
		public bool HasAchievements => this.ffxivCollectCharacter != null && this.ffxivCollectCharacter.Achievements != null && this.ffxivCollectCharacter.Achievements.Count > 0;

		public (int Count, int Total) Mounts
		{
			get
			{
				if (!this.HasMounts)
					return (0, 0);

				return (this.ffxivCollectCharacter!.Mounts!.Count, this.ffxivCollectCharacter.Mounts.Total);
			}
		}

		public (int Count, int Total) Minions
		{
			get
			{
				if (!this.HasMinions)
					return (0, 0);

				return (this.ffxivCollectCharacter!.Minions!.Count, this.ffxivCollectCharacter.Minions.Total);
			}
		}

		public (int Count, int Total) Achievements
		{
			get
			{
				if (!this.HasAchievements)
					return (0, 0);

				return (this.ffxivCollectCharacter!.Achievements!.Count, this.ffxivCollectCharacter.Achievements.Total);
			}
		}

		public static async Task<CharacterInfo> Get(uint id, string xivApiKey)
		{
			CharacterInfo character = new CharacterInfo();
			character.Id = id;

			XivApi.GetResponse? response = await XivApi.GetCharacter(id, xivApiKey);
			character.xivApiCharacter = response.Character;
			character.freeCompany = response.FreeCompany;

			character.ffxivCollectCharacter = await FfxivCollectAPI.Get(id);

			return character;
		}

		public string GetJobLevel(Jobs job)
		{
			XivApi.ClassJob? classJob = this.xivApiCharacter?.GetClassJob(job);

			if (classJob == null)
				return string.Empty;

			return classJob.Level.ToString();
		}
	}
}
