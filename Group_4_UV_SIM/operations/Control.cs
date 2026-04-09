
namespace Group_4_UV_SIM;

public class Control
{
    private static bool HasValidAddress(int operand, CpuState cpu)
    {
        if (!FormatRules.IsValidAddress(operand, cpu.Format))
        {
            Console.WriteLine($"Error: Invalid branch address {operand}.");
            return false;
        }

        return true;
    }

    public static void Branch(int opcode, int operand, CpuState cpu)
    {
        // set operand as the instruction pointer if valid
        if (HasValidAddress(operand, cpu))
        {
            cpu.InstructionPointer = operand;
        }
        else
        {
            cpu.InstructionPointer++;
        }
    }

    public static void Halt(int opcode, int operand, CpuState cpu)
    {
        // set halt flag to true
        cpu.Halted = true;
    }

    public static void BranchZero(int opcode, int operand, CpuState cpu)
    {
        // check if accumulator is zero, if so set instruction pointer to operand
        if (cpu.Accumulator == 0)
        {
            if (HasValidAddress(operand, cpu))
            {
                cpu.InstructionPointer = operand;
            }
            else
            {
                cpu.InstructionPointer++;
            }
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
            if (HasValidAddress(operand, cpu))
            {
                cpu.InstructionPointer = operand;
            }
            else
            {
                cpu.InstructionPointer++;
            }
        }
        else
        {
            cpu.InstructionPointer++;
        }
    }
}