using System;
using System.IO;
using Xunit;
using Group_4_UV_SIM;


namespace Group_4_UV_SIM.Tests;

public class InputOutputReadTests
{
    [Fact]
    public void READ_valid_input()
    {
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        var originalIn = Console.In;
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetIn(new StringReader("123\n"));
            Console.SetOut(output);

            InputOutput.Read(10, 7, cpu);

            Assert.Equal(123, cpu.Memory[7]);
            Assert.Equal(1, cpu.InstructionPointer);
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void READ_invalid_input()
    {
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        var originalIn = Console.In;
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            // invalid first, then valid
            Console.SetIn(new StringReader("hello\n5\n"));
            Console.SetOut(output);

            InputOutput.Read(10, 7, cpu);

            Assert.Equal(5, cpu.Memory[7]);
            Assert.Equal(1, cpu.InstructionPointer);
            Assert.Contains("Invalid input", output.ToString());
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }
}
