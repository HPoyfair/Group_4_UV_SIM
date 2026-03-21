namespace UVGUI
{
    public static class SaveFileService
    {
        public static void SaveMemoryToFile(int[] memory, string filePath)
        {
            if (memory == null)
                throw new ArgumentNullException(nameof(memory));

            if (memory.Length != 100)
                throw new ArgumentException("Memory must have exactly 100 entries.");

            // Find last non-zero instruction
            int lastNonZero = -1;
            for (int i = 99; i >= 0; i--)
            {
                if (memory[i] != 0)
                {
                    lastNonZero = i;
                    break;
                }
            }

            List<string> lines = new List<string>();

            if (lastNonZero == -1)
            {
                // If everything is zero, still save one line
                lines.Add(FormatValue(0));
            }
            else
            {
                for (int i = 0; i <= lastNonZero; i++)
                {
                    lines.Add(FormatValue(memory[i]));
                }
            }

            File.WriteAllLines(filePath, lines);
        }

        private static string FormatValue(int value)
        {
            return value.ToString("+0000;-0000;0000");
        }
    }
}