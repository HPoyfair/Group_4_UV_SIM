using Xunit;

using System;
using System.IO;
using Group_4_UV_SIM; 


[CollectionDefinition("Console Tests", DisableParallelization = true)]
public class ConsoleTests
// Tests Input/Output and any other instructions that interact with the console.
//in this case, subtract is included as it uses the console for checking exception generated.
{
    [Fact]
    public void Subtract_InvalidAddress_HandlesFailure()
    {
        // UC-09 failure/edge: invalid memory address
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        int badOperand = 150;

        var originalOut = Console.Out;
        var output = new StringWriter();
        bool threw = false;

        try
        {
            Console.SetOut(output);

            try
            {
                Arithmetic.Subtract(31, badOperand, cpu);
            }
            catch (Exception)
            {
                // Some implementations throw for invalid addresses.
                threw = true;
            }

            string text = output.ToString().ToLower();

            // Pass condition: either it threw OR it printed an error message.
            Assert.True(threw || text.Contains("error"));

            // If it did NOT throw, we expect it to behave like your I/O ops:
            // increment IP and keep running (prevents infinite loops).
            if (!threw)
            {
                Assert.Equal(1, cpu.InstructionPointer);
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

     [Fact]
public void Input_ReadsValueIntoMemory()
{
    var cpu = new CpuState();
    cpu.InstructionPointer = 5;

    var originalIn = Console.In;
    var originalOut = Console.Out;

    try
    {
        Console.SetIn(new StringReader("42\n"));
        Console.SetOut(new StringWriter()); // suppress prompt spam

        InputOutput.Read(10, 10, cpu);

        Assert.Equal(42, cpu.Memory[10]);
        Assert.Equal(6, cpu.InstructionPointer);
    }
    finally
    {
        Console.SetIn(originalIn);
        Console.SetOut(originalOut);
    }
}


 [Fact]
public void Read_InvalidThenValidInput_RepromptsAndStoresValidValue()
{
    var cpu = new CpuState();
    cpu.InstructionPointer = 5;

    var originalIn = Console.In;
    var originalOut = Console.Out;

    try
    {
        // first line invalid (too big), second line valid
        Console.SetIn(new StringReader("123456\n42\n"));
        Console.SetOut(new StringWriter()); // capture/suppress spam

        InputOutput.Read(10, 10, cpu);

        Assert.Equal(42, cpu.Memory[10]);
        Assert.Equal(6, cpu.InstructionPointer);
    }
    finally
    {
        Console.SetIn(originalIn);
        Console.SetOut(originalOut);
    }
}

    [Fact]
public void Write_PrintsMemoryValue_AndAdvancesIP()
{
    var cpu = new CpuState();
    cpu.InstructionPointer = 5;

    // ensure address is valid
    Assert.True(cpu.Memory.Length > 20);

    cpu.Memory[20] = 99;

    var originalOut = Console.Out;

    try
    {
        var sw = new StringWriter();
        Console.SetOut(sw);

        InputOutput.Write(11, 20, cpu);

        var output = sw.ToString();

        // check formatted output
        Assert.Contains("memory[20] = +0099", output);

        // check IP increment
        Assert.Equal(6, cpu.InstructionPointer);
    }
    finally
    {
        Console.SetOut(originalOut);
    }
}
}