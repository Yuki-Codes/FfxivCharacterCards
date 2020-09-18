// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System;
	using System.IO;
	using System.Net;
	using System.Threading.Tasks;

	internal static class FileDownloader
	{
		public static Task Download(string url, string path)
		{
			string? dir = Path.GetDirectoryName(path);

			if (dir is null)
				throw new Exception("Failed to get director at path: \"" + path + "\"");

			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using (WebClient client = new WebClient())
			{
				client.DownloadFile(url, path);
			}

			return Task.CompletedTask;
		}
	}
}
