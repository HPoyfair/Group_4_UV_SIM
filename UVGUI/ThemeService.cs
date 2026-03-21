namespace UVGUI
{
    public static class ThemeService
    {
        private static readonly Color DefaultPrimary = Color.FromArgb(76, 114, 29);
        private static readonly Color DefaultOff = Color.White;

        public static Theme LoadTheme(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length >= 2)
                    {
                        Color primary = ColorTranslator.FromHtml(lines[0].Trim());
                        Color off = ColorTranslator.FromHtml(lines[1].Trim());
                        return new Theme { PrimaryColor = primary, OffColor = off };
                    }
                }
                catch
                {
                    // File is malformed — fall through to defaults
                }
            }

            return DefaultTheme();
        }

        public static void SaveTheme(Theme theme, string filePath)
        {
            string[] lines =
            {
                ColorTranslator.ToHtml(theme.PrimaryColor),
                ColorTranslator.ToHtml(theme.OffColor)
            };
            File.WriteAllLines(filePath, lines);
        }

        public static Theme DefaultTheme() =>
            new Theme { PrimaryColor = DefaultPrimary, OffColor = DefaultOff };
    }
}
