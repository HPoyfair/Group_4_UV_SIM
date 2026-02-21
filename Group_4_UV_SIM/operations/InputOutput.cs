
namespace Group_4_UV_SIM;

public class InputOutput
{
    private const int WORD_LIMIT = 9999;

    public static void Read(int opcode, int operand, CpuState cpu)
    {
        int value = 0;
        if (cpu.OnRequestInput != null)
        {
            value = cpu.OnRequestInput($"Enter value for address {operand}:");
        }

        cpu.Memory[operand] = value;
        cpu.InstructionPointer++;
    }





    public static void Write(int opcode, int operand, CpuState cpu)
    {
        int value = cpu.Memory[operand];
        string message = $"> {value:0000}";

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

    private static bool IsValidAddress(int operand, CpuState cpu) => operand >= 0 && operand < cpu.Memory.Length; //helper to check if operand is within the bounds of the memory array

    private static string FormatWord(int value){ //another helper converts normal ints into machiny word display format


        // Produces +0000 to +9999, -0001 to -9999
        char sign = value < 0 ? '-' : '+';
        int absVal = Math.Abs(value);
        return $"{sign}{absVal:0000}";
    }
}