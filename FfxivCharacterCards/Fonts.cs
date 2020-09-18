// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using SixLabors.Fonts;

	internal class Fonts
	{
		public static FontCollection Collection = new FontCollection();

		public FontFamily AxisRegular;
		public FontFamily OptimuSemiBold;
		public FontFamily JupiterPro;
		public FontFamily Eorzea;

		public Fonts(string assetDir)
		{
			this.AxisRegular = Collection.Install(assetDir + "/Axis-Regular.ttf");
			this.OptimuSemiBold = Collection.Install(assetDir + "/OptimusPrincepsSemiBold.ttf");
			this.JupiterPro = Collection.Install(assetDir + "/JupiterProFixed.ttf");
			this.Eorzea = Collection.Install(assetDir + "/Eorzea.ttf");
		}
	}
}
