using System;


namespace Public.Common.Lib.Extensions
{
    public static class PathExtensions
    {
		public static string GetRelativePath(string fromPath, string toPath)
        {
            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
            return relativePath;
        }
    }
}
