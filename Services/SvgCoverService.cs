using System;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace mu_marketplaceV0.Services
{
    public static class SvgCoverService
    {
        public static string MakeCoverDataUri(string title, string artist, int size = 1000)
        {
            var words = title?.Trim()
                             .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                         ?? Array.Empty<string>();
            var n = words.Length;
            var live = size * 0.8;
            var border = (size - live) / 2;
            var fSize = n > 0 ? live / n : live;
            var y0 = size / 2 - ((n - 1) / 2) * fSize;

            var sb = new StringBuilder();

            for (int i = 0; i < n; i++)
            {
                var w = words[i];
                var y = y0 + i * fSize;
                sb.Append($@"<text x=""{size/2}"" y=""{y}"" text-anchor=""middle"" dominant-baseline=""middle"" font-family=""sans-serif"" font-size=""{fSize}"" textLength=""{live}"" lengthAdjust=""spacingAndGlyphs"" fill=""#000"">{w}</text>");
            }

            sb.Append($@"<text x=""{size/2}"" y=""{size - border/2}"" text-anchor=""middle"" dominant-baseline=""middle"" font-family=""sans-serif"" font-size=""{border * 0.5}"" fill=""#444"">{artist}</text>");

            var svg = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""{size}"" height=""{size}"" viewBox=""0 0 {size} {size}"">
  <rect width=""100%"" height=""100%"" fill=""#fff""/>
  {sb}
</svg>";

            var dataUri = "data:image/svg+xml;charset=utf-8," + Uri.EscapeDataString(svg);
            return dataUri;
        }
    }
}
