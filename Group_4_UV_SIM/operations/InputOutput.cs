
namespace Group_4_UV_SIM;

public class InputOutput
{
    private const int WORD_LIMIT = 9999;
    
        public static void Read(int opcode, int operand, CpuState cpu){

        if (!IsValidAddress(operand, cpu)){

            Console.WriteLine($"Error: Invalid memory address {operand} in READ operation.");
            cpu.InstructionPointer++; // incrementing even in an error prevents infinate loops
            return;
        }

        while (true){ 

            Console.Write($"Enter an integer word (-9999 to 9999) for memory[{operand:D2}]: "); // adds a nice 00 to make it format good like memory
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= -WORD_LIMIT && value <= WORD_LIMIT)
            {
                cpu.Memory[operand] = value;
                break;
            }

            Console.WriteLine("Invalid input. Please enter a signed integer between -9999 and 9999.");
        }

        cpu.InstructionPointer++;
    }


       
    

     public static void Write(int opcode, int operand, CpuState cpu){


        if (!IsValidAddress(operand, cpu)) //validate address again
        {
            Console.WriteLine($"Error: Invalid memory address {operand} in WRITE operation.");
            cpu.InstructionPointer++; 
            return;
        }

        int value = cpu.Memory[operand]; // load memory

        Console.WriteLine($"memory[{operand:D2}] = {FormatWord(value)}"); //write it from memory

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