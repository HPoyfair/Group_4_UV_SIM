namespace Group_4_UV_SIM;

public class Arithmetic
{
    // Helper method to check for overflow/underflow based on loaded format
    private static bool IsSafe(int value, CpuState cpu)
    {
        int max = FormatRules.GetMaxWordValue(cpu.Format);
        int min = FormatRules.GetMinWordValue(cpu.Format);

        if (value > max || value < min)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Helper method to validate memory address
    private static bool HasValidAddress(int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            Console.WriteLine($"Error: Invalid memory address {operand}.");
            return false;
        }

        return true;
    }

    // ADD operation, adds value from memory to accumulator value if safe
    public static void Add(int opcode, int operand, CpuState cpu)
    {
        if (!HasValidAddress(operand, cpu))
        {
            cpu.InstructionPointer++;
            return;
        }

        int result = cpu.Accumulator + cpu.Memory[operand];

        if (IsSafe(result, cpu))
        {
            cpu.Accumulator = result;
        }
        else
        {
            Console.WriteLine("Error: Overflow in ADD operation.");
        }

        cpu.InstructionPointer++;
    }

    // SUBTRACT operation, subtracts memory value from accumulator value if safe
    public static void Subtract(int opcode, int operand, CpuState cpu)
    {
        if (!HasValidAddress(operand, cpu))
        {
            cpu.InstructionPointer++;
            return;
        }

        int result = cpu.Accumulator - cpu.Memory[operand];

        if (IsSafe(result, cpu))
        {
            cpu.Accumulator = result;
        }
        else
        {
            Console.WriteLine("Error: Overflow in SUBTRACT operation.");
        }

        cpu.InstructionPointer++;
    }

    // DIVIDE operation, divides accumulator by operand if number is not 0
    public static void Divide(int opcode, int operand, CpuState cpu)
    {
        if (!HasValidAddress(operand, cpu))
        {
            cpu.InstructionPointer++;
            return;
        }

        if (cpu.Memory[operand] == 0)
        {
            Console.WriteLine("Error: Cannot divide by 0");
        }
        else
        {
            int result = cpu.Accumulator / cpu.Memory[operand];

            if (IsSafe(result, cpu))
            {
                cpu.Accumulator = result;
            }
            else
            {
                Console.WriteLine("Error: Overflow in DIVIDE operation.");
            }
        }

        cpu.InstructionPointer++;
    }

    // MULTIPLY operation, multiplies accumulator and operand if safe, result stays in accumulator
    public static void Multiply(int opcode, int operand, CpuState cpu)
    {
        if (!HasValidAddress(operand, cpu))
        {
            cpu.InstructionPointer++;
            return;
        }

        int result = cpu.Accumulator * cpu.Memory[operand];

        if (IsSafe(result, cpu))
        {
            cpu.Accumulator = result;
        }
        else
        {
            Console.WriteLine("Error: Overflow in MULTIPLY operation.");
        }

        cpu.InstructionPointer++;
    }
}