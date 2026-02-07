using Xunit;
using System;
using System.IO;
using Group_4_UV_SIM; // <- your namespace


public class InputOutputTests
{
    [Fact]
    public void Input_ReadsValueIntoMemory()
    {
            var cpu = new CpuState();
            cpu.InstructionPointer = 5;
    
            // Simulate input by directly setting the value in memory
           
            Console.SetIn(new StringReader("42")); 
            InputOutput.Read(30, 10, cpu);
    
            Assert.Equal(42, cpu.Memory[10]);
            Assert.Equal(6, cpu.InstructionPointer);
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