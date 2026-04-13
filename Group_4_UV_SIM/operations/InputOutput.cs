namespace Group_4_UV_SIM;

public class InputOutput
{
    public static void Read(int opcode, int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            cpu.InstructionPointer++;
            return;
        }

        int value = 0;

        if (cpu.OnRequestInput != null)
        {
            value = cpu.OnRequestInput($"Enter value for address {operand}:");
        }

        if (!IsSafe(value, cpu))
        {
            Console.WriteLine("Error: Input value is outside the valid word range.");
            cpu.InstructionPointer++;
            return;
        }

        cpu.Memory[operand] = value;
        cpu.InstructionPointer++;
    }

    public static void Write(int opcode, int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            cpu.InstructionPointer++;
            return;
        }

        int value = cpu.Memory[operand];
        string message = $"> {FormatWord(value, cpu)}";

        if (cpu.OnOutputMessage != null)
        {
            cpu.OnOutputMessage(message);
        }
        else
        {
            Console.WriteLine(message);
        }

        cpu.InstructionPointer++;
    }


    private static bool IsSafe(int value, CpuState cpu)
    {
        int max = FormatRules.GetMaxWordValue(cpu.Format);
        int min = FormatRules.GetMinWordValue(cpu.Format);

        return value >= min && value <= max;
    }

    private static string FormatWord(int value, CpuState cpu)
    {
        char sign = value < 0 ? '-' : '+';
        int absVal = Math.Abs(value);

        if (cpu.Format == ProgramFormat.Legacy4Digit)
        {
            return $"{sign}{absVal:0000}";
        }

        return $"{sign}{absVal:000000}";
    }
}