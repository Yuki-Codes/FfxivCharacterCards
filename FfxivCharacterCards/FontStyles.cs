// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using SixLabors.Fonts;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Drawing.Processing;

	internal static class FontStyles
	{
		public static GraphicsOptions FontGraphics = new GraphicsOptions()
		{
			Antialias = true,
		};

		public static DrawingOptions CenterText = new DrawingOptions()
		{
			GraphicsOptions = FontGraphics,
			TextOptions = new TextOptions()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			},
		};

		public static DrawingOptions LeftText = new DrawingOptions()
		{
			GraphicsOptions = FontGraphics,
			TextOptions = new TextOptions()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
			},
		};

		public static DrawingOptions RightText = new DrawingOptions()
		{
			GraphicsOptions = FontGraphics,
			TextOptions = new TextOptions()
			{
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top,
			},
		};
	}
}
