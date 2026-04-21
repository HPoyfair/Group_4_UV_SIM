using System.ComponentModel.DataAnnotations;

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
        bool isValid = false;
        if (format == ProgramFormat.Legacy4Digit)
        {
            isValid = address >= 0 && address <= 99;
        }
        else
        {
            isValid = address >= 0 && address <= 249;
        }

        
        if(!isValid)
        {
            Console.WriteLine($"Error: Address {address} is out of bounds for format {format}.");
        }
        return isValid;
    }

    public static (int opcode, int operand) ParseInstruction(int instruction, ProgramFormat format)
{
    int absInstruction = Math.Abs(instruction);

    if (format == ProgramFormat.Legacy4Digit)
    {
        int opcode = absInstruction / 100;
        int operand = absInstruction % 100;
        return (opcode, operand);
    }

    int opcode6 = (absInstruction / 100) % 100;
    int operand6 = absInstruction % 100;
    return (opcode6, operand6);
}
}