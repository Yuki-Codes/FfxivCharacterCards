// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using SixLabors.Fonts;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Drawing.Processing;
	using SixLabors.ImageSharp.Processing;

	internal static class IImageProcessingContextExtensions
	{
		public static void DrawText(this IImageProcessingContext context, DrawingOptions op, string? text, Font font, Color color, Rectangle bounds)
		{
			if (text == null)
				return;

			if (string.IsNullOrEmpty(text))
				return;

			op.TextOptions.WrapTextWidth = bounds.Width;

			RendererOptions rOp = new RendererOptions(font);
			rOp.WrappingWidth = bounds.Width;

			bool fits = false;
			while (!fits)
			{
				FontRectangle size = TextMeasurer.Measure(text, rOp);
				fits = size.Height <= bounds.Height && size.Width <= bounds.Width;

				if (!fits)
				{
					text = Truncate(text, text.Length - 5);
				}
			}

			context.DrawText(op, text, font, color, new Point(bounds.X, bounds.Y));
		}

		public static void DrawTextAnySize(this IImageProcessingContext context, DrawingOptions op, string? text, FontFamily font, Color color, Rectangle bounds)
		{
			if (string.IsNullOrEmpty(text))
				return;

			int fontSize = 64;
			bool fits = false;
			Font currentFont = font.CreateFont(fontSize);
			while (!fits)
			{
				currentFont = font.CreateFont(fontSize);
				FontRectangle size = TextMeasurer.Measure(text!, new RendererOptions(currentFont));
				fits = size.Height <= bounds.Height && size.Width <= bounds.Width;

				if (!fits)
				{
					fontSize -= 2;
				}

				if (fontSize <= 2)
				{
					return;
				}
			}

			context.DrawText(op, text, currentFont, color, new Point(bounds.X, bounds.Y));
		}

		private static string Truncate(string value, int maxChars)
		{
			return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
		}
	}
}
