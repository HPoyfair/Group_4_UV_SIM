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

        //================== RUN TIME ===============
        //iterate through memory and execute instructions

        cpu.InstructionPointer = 0;
        // run the program when instructions are ABOVE 0, HALT FLAG IS FALSE, and IP IS WITHIN MEMORY BOUNDS
        while(!cpu.Halted && cpu.InstructionPointer < cpu.Memory.Length && cpu.InstructionPointer >= 0) {  
            int instruction = cpu.Memory[cpu.InstructionPointer];
            int opcode = instruction / 100; // first two digits
            int operand = instruction % 100; // last two digits

            ExecuteInstruction(opcode, operand);

    
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
    



    
//IMPORTANT: You all need to increment instruction pointer on each case or handle that inside your operations class (you can look at control)




    private void ExecuteInstruction(int opcode, int operand)
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
        Control.Branch(opcode, operand, cpu);
        break;
    case 41:
        Control.BranchNeg(opcode, operand, cpu);
        break;
    case 42:
        Control.BranchZero(opcode, operand, cpu);
        break;
    case 43:
        Control.Halt(opcode, operand, cpu);
       
        
        break;

    default:
        throw new Exception($"Invalid opcode: {opcode}");
}

    }
}