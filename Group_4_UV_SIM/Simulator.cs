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

            int maxMemory = FormatRules.GetMaxMemorySize(cpu.Format);

            if (cpu.Halted || cpu.InstructionPointer < 0 || cpu.InstructionPointer >= maxMemory)
            {
                cpu.Halted = true;
                return;
            }

            int instruction = cpu.Memory[cpu.InstructionPointer];
            int opcode, operand;
            (opcode, operand) = FormatRules.ParseInstruction(instruction, cpu.Format);

            ExecuteInstruction(opcode, operand);


            cpu.NotifyStateChanged();
        }
    public void ReadFile(string path)
{
    if (!File.Exists(path))
    {
        throw new FileNotFoundException($"File at path {path} not found.");
    }

    string[] rawLines = File.ReadAllLines(path);

    List<string> lines = new List<string>();

    foreach (string line in rawLines)
    {
        if (string.IsNullOrWhiteSpace(line))
            break;

        lines.Add(line.Trim());
    }

    if (lines.Count == 0)
    {
        throw new Exception("Program file is empty.");
    }

    ProgramFormat detectedFormat = DetectFormat(lines[0]);

    foreach (string line in lines)
    {
        //may want to change how we do this, for now, just doesnt not validate if line is negative (end of program)
        if (line.TrimStart().StartsWith("-"))
        continue;
        ValidateLineFormat(line, detectedFormat);
    }

    int maxLines = FormatRules.GetMaxMemorySize(detectedFormat);

    if (lines.Count > maxLines)
    {
        throw new Exception($"File has too many lines. {detectedFormat} supports at most {maxLines} lines.");
    }

    Array.Clear(cpu.Memory, 0, cpu.Memory.Length);

    cpu.Format = detectedFormat;
    cpu.Accumulator = 0;
    cpu.InstructionPointer = 0;
    cpu.Halted = false;

    for (int i = 0; i < lines.Count; i++)
    {
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
    

//HELPERS
private ProgramFormat DetectFormat(string line)
{
    string trimmed = line.Trim();

    if (trimmed.StartsWith("+") || trimmed.StartsWith("-"))
    {
        trimmed = trimmed.Substring(1);
    }

    if (trimmed.Length == 4)
        return ProgramFormat.Legacy4Digit;

    if (trimmed.Length == 6)
        return ProgramFormat.Extended6Digit;

    throw new Exception("Invalid file format. Words must be 4 digits or 6 digits.");
}

private void ValidateLineFormat(string line, ProgramFormat format)
{
    string trimmed = line.Trim();

    if (trimmed.StartsWith("+") || trimmed.StartsWith("-"))
    {
        trimmed = trimmed.Substring(1);
    }

    if (format == ProgramFormat.Legacy4Digit && trimmed.Length != 4)
    {
        throw new Exception("Mixed formats are not allowed. Legacy files must contain only 4-digit words.");
    }

    if (format == ProgramFormat.Extended6Digit && trimmed.Length != 6)
    {
        throw new Exception("Mixed formats are not allowed. Extended files must contain only 6-digit words.");
    }

    if (!int.TryParse(line, out _))
    {
        throw new Exception($"Invalid numeric value in file: {line}");
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