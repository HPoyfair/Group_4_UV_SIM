public class LoadStore
{
    public static void Load(int opcode, int operand, CpuState cpu)
    {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
        
    }

    public static void Store(int opcode, int operand, CpuState cpu)
    {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
    }
}