using System;
using System.IO;




public class Simulator
{

    private readonly CpuState cpu = new CpuState();
    // other things here as needed 


    public void Run()
    {


        Console.Write("Enter program file path: ");
        string path = Console.ReadLine();

        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("No file path provided. Exiting.");
            return;
        } 

        ReadFile(path); 
        LogMemory();

        
        //iterate through memory and execute instructions

        for(int i = 0; i < cpu.Memory.Length; i++)
        {
            cpu.InstructionPointer = i;
            int instruction = cpu.Memory[i];
            int opcode = instruction / 100;
            int operand = instruction % 100;

           
        }
        
    }
    private void ReadFile(string path)
    {
        

        //check if file exists first
        if (!File.Exists(path))
    {
        Console.WriteLine($"File at path {path} not found.");
        return;
    }
        //read file and load instructions into memory 
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length && i < cpu.Memory.Length; i++)
        {
            //stop reading if empty line exists
            if(lines[i].Trim() == "") break;
            //convert string to number and store in memory at the
            cpu.Memory[i] = int.Parse(lines[i]);
        }

    }


    public void LogMemory()
    {
        for (int i = 0; i < cpu.Memory.Length; i++)
        {
            Console.WriteLine($"Memory[{i}]: {cpu.Memory[i]}");
        }
    }
    



    

    private static void ExecuteInstruction(int opcode, int operand)
    {
        switch (opcode)
{// Input/Output operations
    case 10:
    case 11:
        
        
        break;
 // Load/Store operations
      
    case 20:
    case 21:
       
        break;
// Arithmetic operations
    case 30:
    case 31:
    case 32:
    case 33:
        
        
        break;
 // Control operations
    case 40:
    case 41:
    case 42:
    case 43:
       
        
        break;

    default:
        throw new Exception($"Invalid opcode: {opcode}");
}

    }
}