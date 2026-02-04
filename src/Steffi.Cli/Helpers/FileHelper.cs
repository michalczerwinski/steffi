namespace Steffi.Cli.Helpers;

public static class FileHelper
{
	public static void TryDeleteFileIfExists(string path)
	{
		if (File.Exists(path))
		{
			try
			{
				File.Delete(path);
			}
			catch { }
		}
	}
}