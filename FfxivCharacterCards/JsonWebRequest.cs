// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	using System.IO;
	using System.Net;
	using System.Text.Json;
	using System.Threading.Tasks;

	internal static class JsonWebRequest
	{
		private static JsonSerializerOptions options = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
		};

		internal static async Task<T> Send<T>(string url)
		{
			WebRequest req = WebRequest.Create(url);
			req.Timeout = 30 * 1000;
			WebResponse response = await req.GetResponseAsync();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string json = await reader.ReadToEndAsync();

			T val = JsonSerializer.Deserialize<T>(json, options);

			if (val == null)
				throw new System.Exception("Failed to deserialize json");

			return val;
		}
	}
}
