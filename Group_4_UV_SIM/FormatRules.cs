namespace Group_4_UV_SIM;

public enum ProgramFormat
{
    Legacy4Digit,
    Extended6Digit
}
public static class FormatRules
{
    public static int GetMaxMemorySize(ProgramFormat format)
    {
        return format == ProgramFormat.Legacy4Digit ? 100 : 250;
    }

    public static int GetMaxWordValue(ProgramFormat format)
    {
        return format == ProgramFormat.Legacy4Digit ? 9999 : 999999;
    }

    public static int GetMinWordValue(ProgramFormat format)
    {
        return format == ProgramFormat.Legacy4Digit ? -9999 : -999999;
    }

    public static bool IsValidAddress(int address, ProgramFormat format)
    {
        if (format == ProgramFormat.Legacy4Digit)
        {
            return address >= 0 && address <= 99;
        }

        return address >= 0 && address <= 249;
    }

    public static (int opcode, int operand) ParseInstruction(int instruction, ProgramFormat format)
    {
        if (format == ProgramFormat.Legacy4Digit)
        {
            int opcode = instruction / 100;
            int operand = instruction % 100;
            return (opcode, operand);
        }

        int opcode6 = instruction / 1000;
        int operand6 = instruction % 1000;
        return (opcode6, operand6);
    }
}