using Group_4_UV_SIM;

namespace UVGUI
{
    public class MemoryEditor
    {
        private readonly int[] memory;
        private readonly CpuState cpu;

        public MemoryEditor(CpuState cpu)
        {
            this.cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
            memory = cpu.Memory ?? throw new ArgumentNullException(nameof(cpu.Memory));

            if (memory.Length != 250)
                throw new ArgumentException("CPU memory must contain exactly 250 entries.");
        }

        private int MemorySize => FormatRules.GetMaxMemorySize(cpu.Format);
        private int MinValue => FormatRules.GetMinWordValue(cpu.Format);
        private int MaxValue => FormatRules.GetMaxWordValue(cpu.Format);

        public bool TryUpdateValue(int address, string rawValue, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsValidAddress(address))
            {
                errorMessage = $"Invalid memory address: {address}.";
                return false;
            }

            if (!TryParseValue(rawValue, out int parsedValue, out errorMessage))
                return false;

            memory[address] = parsedValue;
            return true;
        }

        public bool TryInsertValue(int address, int value, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsValidAddress(address))
            {
                errorMessage = $"Invalid memory address: {address}.";
                return false;
            }

            if (!IsValidValue(value))
            {
                errorMessage = $"Value must be between {MinValue} and {MaxValue}.";
                return false;
            }

            if (memory[MemorySize - 1] != 0)
            {
                errorMessage = "Cannot insert: memory is full. Delete an instruction first.";
                return false;
            }

            for (int i = MemorySize - 1; i > address; i--)
            {
                memory[i] = memory[i - 1];
            }

            memory[address] = value;
            return true;
        }

        public bool TryDeleteValue(int address, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsValidAddress(address))
            {
                errorMessage = $"Invalid memory address: {address}.";
                return false;
            }

            for (int i = address; i < MemorySize - 1; i++)
            {
                memory[i] = memory[i + 1];
            }

            memory[MemorySize - 1] = 0;
            return true;
        }

        public bool TryCopy(int startAddress, int count, out string copiedText, out string errorMessage)
        {
            copiedText = string.Empty;
            errorMessage = string.Empty;

            if (!TryValidateRange(startAddress, count, out errorMessage))
                return false;

            List<string> lines = new List<string>();
            for (int i = 0; i < count; i++)
            {
                lines.Add(FormatValue(memory[startAddress + i], cpu.Format));
            }

            copiedText = string.Join(Environment.NewLine, lines);
            return true;
        }

        public bool TryCut(int startAddress, int count, out string cutText, out string errorMessage)
        {
            cutText = string.Empty;
            errorMessage = string.Empty;

            if (!TryCopy(startAddress, count, out cutText, out errorMessage))
                return false;

            for (int i = 0; i < count; i++)
            {
                TryDeleteValue(startAddress, out _);
            }

            return true;
        }

        public bool TryPaste(int startAddress, string clipboardText, out string errorMessage)
{
    errorMessage = string.Empty;

    if (!IsValidAddress(startAddress))
    {
        errorMessage = $"Invalid memory address: {startAddress}.";
        return false;
    }

    if (string.IsNullOrWhiteSpace(clipboardText))
    {
        errorMessage = "Clipboard is empty.";
        return false;
    }

    string[] lines = clipboardText
        .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

    List<int> valuesToPaste = new List<int>();

    foreach (string line in lines)
    {
        if (!TryParseValue(line.Trim(), out int parsedValue, out errorMessage))
            return false;

        valuesToPaste.Add(parsedValue);
    }

    if (valuesToPaste.Count == 0)
    {
        errorMessage = "Clipboard does not contain any valid values.";
        return false;
    }

    if (startAddress + valuesToPaste.Count > MemorySize)
    {
        errorMessage = $"Paste would exceed the {MemorySize} memory entry limit.";
        return false;
    }

    for (int i = 0; i < valuesToPaste.Count; i++)
    {
        memory[startAddress + i] = valuesToPaste[i];
    }

    return true;
}
        public int GetUsedLength()
        {
            for (int i = MemorySize - 1; i >= 0; i--)
            {
                if (memory[i] != 0)
                    return i + 1;
            }

            return 0;
        }

        public static string FormatValue(int value, ProgramFormat format)
        {
            return format == ProgramFormat.Legacy4Digit
                ? value.ToString("+0000;-0000;0000")
                : value.ToString("+000000;-000000;000000");
        }

        public bool TryParseValue(string rawValue, out int parsedValue, out string errorMessage)
        {
            parsedValue = 0;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                errorMessage = "Value cannot be empty.";
                return false;
            }

            if (!int.TryParse(rawValue.Trim(), out parsedValue))
            {
                errorMessage = $"\"{rawValue}\" is not a valid integer.";
                return false;
            }

            if (!IsValidValue(parsedValue))
            {
                errorMessage = $"Value must be between {MinValue} and {MaxValue}.";
                return false;
            }

            return true;
        }

        private bool TryValidateRange(int startAddress, int count, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsValidAddress(startAddress))
            {
                errorMessage = $"Invalid memory address: {startAddress}.";
                return false;
            }

            if (count <= 0)
            {
                errorMessage = "Count must be greater than 0.";
                return false;
            }

            if (startAddress + count > MemorySize)
            {
                errorMessage = "Requested range exceeds memory size.";
                return false;
            }

            return true;
        }

        private bool IsValidAddress(int address)
        {
            return address >= 0 && address < MemorySize;
        }

        private bool IsValidValue(int value)
        {
            return value >= MinValue && value <= MaxValue;
        }
    }
}