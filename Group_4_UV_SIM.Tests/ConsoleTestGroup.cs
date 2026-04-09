using Xunit;
using System;
using System.IO;
using Group_4_UV_SIM;

[CollectionDefinition("Console Tests", DisableParallelization = true)]
public class ConsoleTests
// Tests Input/Output and any other instructions that interact with the console.
// in this case, subtract is included as it uses the console for checking exception generated.
{
    [Fact]
    public void Subtract_InvalidAddress_HandlesFailure()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
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
                threw = true;
            }

            string text = output.ToString().ToLower();

            Assert.True(threw || text.Contains("error"));

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
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 5;

        cpu.OnRequestInput = prompt => 42;
        cpu.OnOutputMessage = message => { };

        InputOutput.Read(10, 10, cpu);

        Assert.Equal(42, cpu.Memory[10]);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Read_StoresFirstInputValue_AndAdvancesInstructionPointer_Extended()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.InstructionPointer = 5;

        int callCount = 0;

        cpu.OnRequestInput = prompt =>
        {
            callCount++;
            return 123456;
        };

        InputOutput.Read(10, 10, cpu);

        Assert.Equal(123456, cpu.Memory[10]);
        Assert.Equal(6, cpu.InstructionPointer);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void Read_LegacyOutOfRangeInput_DoesNotStoreValue()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 5;
        cpu.Memory[10] = 0;

        cpu.OnRequestInput = prompt => 123456;

        InputOutput.Read(10, 10, cpu);

        Assert.Equal(0, cpu.Memory[10]);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Write_OutputsMemoryValue_AndAdvancesIP_Legacy()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 5;
        cpu.Memory[20] = 99;

        string capturedOutput = string.Empty;
        cpu.OnOutputMessage = message => capturedOutput = message;

        InputOutput.Write(11, 20, cpu);

        Assert.False(string.IsNullOrWhiteSpace(capturedOutput));
        Assert.Contains("99", capturedOutput);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Write_OutputsExtendedFormattedValue_AndAdvancesIP()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.InstructionPointer = 5;
        cpu.Memory[20] = 123456;

        string capturedOutput = string.Empty;
        cpu.OnOutputMessage = message => capturedOutput = message;

        InputOutput.Write(11, 20, cpu);

        Assert.False(string.IsNullOrWhiteSpace(capturedOutput));
        Assert.Contains("123456", capturedOutput);
        Assert.Equal(6, cpu.InstructionPointer);
    }
}