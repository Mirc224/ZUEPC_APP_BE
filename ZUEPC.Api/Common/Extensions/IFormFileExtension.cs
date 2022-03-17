using System.Text;

namespace ZUEPC.Api.Common.Extensions;

public static class IFormFileExtension
{
	public static async Task<string> ReadAsStringAsync(this IFormFile file)
	{
		StringBuilder result = new();
		using (StreamReader reader = new(file.OpenReadStream()))
		{
			while (reader.Peek() >= 0)
				result.AppendLine(await reader.ReadLineAsync());
		}
		return result.ToString();
	}
}
