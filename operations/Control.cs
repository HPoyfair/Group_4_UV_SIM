

using System.Dynamic;
namespace Group_4_UV_SIM;

public class Control
{
    public static void Branch(int opcode, int operand, CpuState cpu)
    {
        // set operand as the instruction pointer
        cpu.InstructionPointer = operand;

    }

    public static void Halt(int opcode, int operand, CpuState cpu)
    {
        //set halt flag to true
        cpu.Halted = true;
    }
    public static void BranchZero(int opcode, int operand, CpuState cpu)
    {
        // check if accumulator is zero, if so set instruction pointer to operand
        if (cpu.Accumulator == 0)
        {
            cpu.InstructionPointer = operand;
        }
        else
        {
            cpu.InstructionPointer++;
        }
    }

    public static void BranchNeg(int opcode, int operand, CpuState cpu)
    {
        // check if accumulator is negative, if so set instruction pointer to operand
        if (cpu.Accumulator < 0)
        {
            cpu.InstructionPointer = operand;
        }

        else
        {
            cpu.InstructionPointer++;
        }
    }
   
}