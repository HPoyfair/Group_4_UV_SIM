using System;
using System.IO;
using Xunit;
using Group_4_UV_SIM;

namespace Group_4_UV_SIM.Tests;

public class WriteInstructionTests
{
    [Fact]
    public void Write_Valid_UsesCurrentOutputFormat()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 0;

        int operand = 7;
        cpu.Memory[operand] = 42;

        string capturedOutput = string.Empty;
        cpu.OnOutputMessage = message => capturedOutput = message;

        InputOutput.Write(11, operand, cpu);

        Assert.Equal("> +0042", capturedOutput);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Write_InvalidAddress_PrintsCurrentErrorMessage()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 0;

        int badOperand = 150;

        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetOut(output);

            InputOutput.Write(11, badOperand, cpu);

            string text = output.ToString();

            Assert.Contains("Error: Invalid memory address 150.", text);
            Assert.Equal(1, cpu.InstructionPointer);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
