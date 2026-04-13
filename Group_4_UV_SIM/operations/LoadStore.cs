namespace Group_4_UV_SIM;

public class LoadStore
{

    private static bool IsSafe(int value, CpuState cpu)
    {
        int max = FormatRules.GetMaxWordValue(cpu.Format);
        int min = FormatRules.GetMinWordValue(cpu.Format);

        return value >= min && value <= max;
    }

    public static void Load(int opcode, int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            cpu.InstructionPointer++;
            return;
        }

        // Load a word from memory location (operand) into the accumulator
        cpu.Accumulator = cpu.Memory[operand];

        // increment instruction pointer
        cpu.InstructionPointer++;
    }

    public static void Store(int opcode, int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            cpu.InstructionPointer++;
            return;
        }

        if (!IsSafe(cpu.Accumulator, cpu))
        {
            Console.WriteLine("Error: Accumulator value is outside the valid word range.");
            cpu.InstructionPointer++;
            return;
        }

        // Store the accumulator value into memory location (operand)
        cpu.Memory[operand] = cpu.Accumulator;

        // increment instruction pointer
        cpu.InstructionPointer++;
    }
}