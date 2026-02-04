public class Arithmetic
{
    private const int WORD_LIMIT = 9999;

    // Helper method to check for overflow/underflow
    private static bool IsSafe(int value)
    {
        if (value > WORD_LIMIT || value < -WORD_LIMIT)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // ADD operation, adds value from memory to accumulator value if safe
    public static void Add(int opcode, int operand, CpuState cpu) {
        int result = cpu.Accumulator + cpu.Memory[operand];

        if (IsSafe(result))
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
    public static void Subtract(int opcode, int operand, CpuState cpu) {
        int result = cpu.Accumulator - cpu.Memory[operand];

        if (IsSafe(result))
        {
            cpu.Accumulator = result;
        }
        else
        {
            Console.WriteLine("Error: Overflow in ADD operation.");
        }

        cpu.InstructionPointer++;
    }

    public static void Divide(int opcode, int operand, CpuState cpu) {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
    }
    public static void Multiply(int opcode, int operand, CpuState cpu) {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
    }
}