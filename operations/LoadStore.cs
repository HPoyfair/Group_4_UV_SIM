public class LoadStore
{
    public static void Load(int opcode, int operand, CpuState cpu)
    {
        if (operand < 0 || operand >= cpu.Memory.Length)
        {
            Console.WriteLine($"Error: Invalid memory address {operand} in LOAD operation.");
            cpu.InstructionPointer++;
            return;
        }

        // Load a word from memory location (operand) into the accumulator
        cpu.Accumulator = cpu.Memory[operand];
        //increment instruction pointer
        cpu.InstructionPointer++;
        
    }

    public static void Store(int opcode, int operand, CpuState cpu)
    {
        if (operand < 0 || operand >= cpu.Memory.Length)
        {
            Console.WriteLine($"Error: Invalid memory address {operand} in STORE operation.");
            cpu.InstructionPointer++;
            return;
        }

        // Store the accumulator value into memory location (operand)
        cpu.Memory[operand] = cpu.Accumulator;

        //increment instruction pointer
        cpu.InstructionPointer++;
    }
}