public class InputOutput
{
    public static void Read(int opcode, int operand, CpuState cpu) {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
    }

    public static void Write(int opcode, int operand, CpuState cpu) {
        //======= function ======

        //increment instruction pointer
        cpu.InstructionPointer++;
    }
}