using System;
using System.IO;




public class Simulator
{

    private int[] memory = new int[100];
    private int accumulator = 0;
    private int instructionPointer = 0;
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

        for (int i = 0; i < lines.Length && i < memory.Length; i++)
        {
            //stop reading if empty line exists
            if(lines[i].Trim() == "") break;
            //convert string to number and store in memory at the
            memory[i] = int.Parse(lines[i]);
        }

    }


    public void LogMemory()
    {
        for (int i = 0; i < memory.Length; i++)
        {
            Console.WriteLine($"Memory[{i}]: {memory[i]}");
        }
    }


    
    private void ExecuteInstruction(int opcode, int operand)
    {
        switch (opcode)
{
    case 10:
    case 11:
        // Input/Output operations
        //ex. InputOutput.Execute(opcode, operand);
        break;

    case 20:
    case 21:
        // Load/Store operations
        //ex. LoadStore.Execute(opcode, operand);
        break;

    case 30:
    case 31:
    case 32:
    case 33:
        // Arithmetic operations
        //ex. Arithmetic.Execute(opcode, operand);
        break;

    case 40:
    case 41:
    case 42:
    case 43:
        // Control operations
        //ex. Control.Execute(opcode, operand);
        break;

    default:
        throw new Exception($"Invalid opcode: {opcode}");
}

    }
}