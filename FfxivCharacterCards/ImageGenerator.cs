// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using SixLabors.Fonts;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Drawing.Processing;
	using SixLabors.ImageSharp.PixelFormats;
	using SixLabors.ImageSharp.Processing;

	internal static class ImageGenerator
	{
		private static Fonts? fonts;

		internal static async Task<string> Draw(CharacterInfo character, string tempDirectory, string assetDirectory)
		{
			if (fonts == null)
				fonts = new Fonts(assetDirectory);

			if (character.Portrait == null)
				throw new Exception("Character has no portrait");

			string portraitPath = tempDirectory + "/" + character.Id + ".jpg";
			await FileDownloader.Download(character.Portrait, portraitPath);

			Image<Rgba32> backgroundImg = Image.Load<Rgba32>(assetDirectory + "/CharacterCardBackground.png");

			Image<Rgba32> charImg = Image.Load<Rgba32>(portraitPath);
			charImg.Mutate(x => x.Resize(375, 512));

			Image<Rgba32> overlayImg = Image.Load<Rgba32>(assetDirectory + "/CharacterCardOverlay.png");

			Image<Rgba32> finalImg = new Image<Rgba32>(1024, 512);
			finalImg.Mutate(x => x.DrawImage(backgroundImg, 1.0f));
			finalImg.Mutate(x => x.DrawImage(charImg, 1.0f));
			finalImg.Mutate(x => x.DrawImage(overlayImg, 1.0f));

			// Grand Company
			if (character.GrandCompany != null && character.GrandCompany.Company != null)
			{
				Image<Rgba32> gcImg = Image.Load<Rgba32>(assetDirectory + "/GrandCompanies/" + character.GrandCompany.Company.ID + ".png");
				finalImg.Mutate(x => x.DrawImage(gcImg, 1.0f));
				gcImg.Dispose();

				if (character.GrandCompany.Rank != null)
				{
					Image<Rgba32> rankImg = Image.Load<Rgba32>(assetDirectory + "/GrandCompanies/Ranks/" + character.GrandCompany.Rank.Icon.Replace("/i/083000/", string.Empty));
					finalImg.Mutate(x => x.DrawImage(rankImg, new Point(370, 152), 1.0f));
					rankImg.Dispose();
					finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, character.GrandCompany?.Rank?.Name, fonts.AxisRegular.CreateFont(18), Color.White, new Point(412, 161)));
				}
			}

			// Server
			finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, character.Server + " - " + character.DataCenter, fonts.AxisRegular.CreateFont(18), Color.White, new Point(412, 194)));

			// Free Company
			if (character.FreeCompany != null)
			{
				Image<Rgba32> crestFinal = new Image<Rgba32>(128, 128);
				foreach (string crestPart in character.FreeCompany.Crest)
				{
					string name = Path.GetFileName(crestPart);
					string crestPath = assetDirectory + "/Crests/" + name;

					if (!File.Exists(crestPath))
						await FileDownloader.Download(crestPart, crestPath);

					Image<Rgba32> crestImg = Image.Load<Rgba32>(crestPath);
					crestFinal.Mutate(x => x.DrawImage(crestImg, 1.0f));
					crestImg.Dispose();
				}

				for (int y = 0; y < crestFinal.Height; y++)
				{
					for (int x = 0; x < crestFinal.Width; x++)
					{
						Rgba32 pixel = crestFinal[x, y];

						if (pixel.R == 64 && pixel.G == 64 && pixel.B == 64)
							pixel.A = 0;

						crestFinal[x, y] = pixel;
					}
				}

				crestFinal.Mutate(x => x.Resize(64, 64));
				finalImg.Mutate(x => x.DrawImage(crestFinal, new Point(364, 270), 1.0f));
				crestFinal.Dispose();

				finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, "<" + character.FreeCompany.Tag + ">", fonts.AxisRegular.CreateFont(24), Color.White, new Point(431, 300)));
				finalImg.Mutate(x => x.DrawTextAnySize(FontStyles.LeftText, character.FreeCompany.Name, fonts.AxisRegular, Color.White, new Rectangle(431, 280, 158, 22)));
			}

			// Name
			finalImg.Mutate(x => x.DrawTextAnySize(FontStyles.CenterText, character.Name, fonts.OptimuSemiBold, Color.White, new Rectangle(680, 70, 660, 55)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.Title, fonts.AxisRegular.CreateFont(22), Color.White, new PointF(680, 35)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.Race + " (" + character.Tribe + ")", fonts.AxisRegular.CreateFont(20), Color.White, new PointF(680, 110)));

			// Birthday (1st Sun of the 1st Astral Moon)
			if (character.NameDay != null)
			{
				Image<Rgba32> moonImg;
				if (character.NameDay.Contains("Astral"))
				{
					moonImg = Image.Load<Rgba32>(assetDirectory + "/Moons/Astral.png");
				}
				else
				{
					moonImg = Image.Load<Rgba32>(assetDirectory + "/Moons/Umbral.png");
				}

				finalImg.Mutate(x => x.DrawImage(moonImg, new Point(907, 122), 1.0f));
				moonImg.Dispose();
			}

			Image<Rgba32> dietyImage = Image.Load<Rgba32>(assetDirectory + character.GuardianDeity?.Icon.Replace("/i/061000/", "/Twelve/"));
			finalImg.Mutate(x => x.DrawImage(dietyImage, new Point(907, 122), 1.0f));
			dietyImage.Dispose();

			finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, character.NameDay, fonts.AxisRegular.CreateFont(16), Color.White, new Point(700, 191)));
			finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, character.GuardianDeity?.Name, fonts.AxisRegular.CreateFont(20), Color.White, new Point(700, 164)));

			// Jobs
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Paladin), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(631, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Warrior), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(690, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Darkknight), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(747, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Gunbreaker), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(808, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Whitemage), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(865, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Scholar), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(925, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Astrologian), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(983, 330)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Dragoon), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(395, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Monk), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(454, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Ninja), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(513, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Samurai), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(572, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Bard), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(631, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Machinist), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(690, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Dancer), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(747, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Blackmage), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(808, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Summoner), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(865, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Redmage), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(925, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Bluemage), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(983, 406)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Botanist), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(395, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Fisher), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(454, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Miner), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(513, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Alchemist), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(572, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Armorer), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(631, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Blacksmith), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(690, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Carpenter), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(747, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Culinarian), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(808, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Goldsmith), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(865, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Leatherworker), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(925, 480)));
			finalImg.Mutate(x => x.DrawText(FontStyles.CenterText, character.GetJobLevel(Jobs.Weaver), fonts.AxisRegular.CreateFont(20), Color.White, new PointF(983, 480)));

			// Progress
			if (character.HasMounts)
			{
				string mountsStr = character.Mounts.Count + " / " + character.Mounts.Total;
				float p = (float)character.Mounts.Count / (float)character.Mounts.Total;

				Image<Rgba32> barImg = Image.Load<Rgba32>(assetDirectory + "/Bar.png");
				float width = p * barImg.Width;
				barImg.Mutate(x => x.Resize(new Size((int)width, barImg.Height)));
				finalImg.Mutate(x => x.DrawImage(barImg, new Point(404, 234), 1.0f));
				barImg.Dispose();
				finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, mountsStr, fonts.AxisRegular.CreateFont(16), Color.White, new Point(408, 234)));
			}

			if (character.HasMinions)
			{
				string minionsStr = character.Minions.Count + " / " + character.Minions.Total;
				float p = (float)character.Minions.Count / (float)character.Minions.Total;

				Image<Rgba32> barImg = Image.Load<Rgba32>(assetDirectory + "/Bar.png");
				float width = p * barImg.Width;
				barImg.Mutate(x => x.Resize(new Size((int)width, barImg.Height)));
				finalImg.Mutate(x => x.DrawImage(barImg, new Point(616, 234), 1.0f));
				barImg.Dispose();
				finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, minionsStr, fonts.AxisRegular.CreateFont(16), Color.White, new Point(620, 234)));
			}

			if (character.HasAchievements)
			{
				string achieveStr = character.Achievements.Count + " / " + character.Achievements.Total;
				float p = (float)character.Achievements.Count / (float)character.Achievements.Total;

				Image<Rgba32> barImg = Image.Load<Rgba32>(assetDirectory + "/Bar.png");
				float width = p * barImg.Width;
				barImg.Mutate(x => x.Resize(new Size((int)width, barImg.Height)));
				finalImg.Mutate(x => x.DrawImage(barImg, new Point(838, 234), 1.0f));
				barImg.Dispose();
				finalImg.Mutate(x => x.DrawText(FontStyles.LeftText, achieveStr, fonts.AxisRegular.CreateFont(16), Color.White, new Point(842, 234)));
			}

			// Save
			string outputPath = tempDirectory + character.Id + "_render.png";
			finalImg.Save(outputPath);

			charImg.Dispose();
			overlayImg.Dispose();
			finalImg.Dispose();

			return outputPath;
		}
	}
}
