using System;
using System.IO;

namespace Group_4_UV_SIM;



public class Simulator
{

    private readonly CpuState cpu;
    public Simulator(CpuState sharedState)
    {
        cpu = sharedState;
    }


    public void Run(string passedPath = null)
    {
        string path = passedPath;

        // If no path was passed (Console mode), ask the user in the console
        if (string.IsNullOrEmpty(path))
        {
            Console.Write("Enter program file path: ");
            path = Console.ReadLine();
        }

        // Guard clause: if still empty, exit
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("No file path provided. Exiting.");
            return;
        }

        // Now load and execute
        ReadFile(path);
        LogMemory();
    }
        public void ExecuteNext()
        {
            if (cpu.Halted || cpu.InstructionPointer < 0 || cpu.InstructionPointer >= cpu.Memory.Length)
            {
                cpu.Halted = true;
                return;
            }

            int instruction = cpu.Memory[cpu.InstructionPointer];
            int opcode = instruction / 100;
            int operand = instruction % 100;

            ExecuteInstruction(opcode, operand);


            cpu.NotifyStateChanged();
        }
    public void ReadFile(string path)
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
{//============================= Input/Output operations =========================
    case 10:
        InputOutput.Read(opcode, operand, cpu);
        break;
    case 11:
        InputOutput.Write(opcode, operand, cpu);
        break;
 // ===========================  Load/Store operations =========================
      
    case 20:
        LoadStore.Load(opcode, operand, cpu);
        break;
    case 21:
       LoadStore.Store(opcode, operand, cpu);
        break;
// =============================  Arithmetic operations =========================
    case 30:
        Arithmetic.Add(opcode, operand, cpu);
        break;
    case 31:
        Arithmetic.Subtract(opcode, operand, cpu);
        break;
    case 32:
        Arithmetic.Divide(opcode, operand, cpu);
        break;
    case 33:
        Arithmetic.Multiply(opcode, operand, cpu);
        break;
 //============================== Control operations ===========================
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