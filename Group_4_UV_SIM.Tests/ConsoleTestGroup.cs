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

    cpu.OnRequestInput = prompt => 42;
    cpu.OnOutputMessage = message => { };

    InputOutput.Read(10, 10, cpu);

    Assert.Equal(42, cpu.Memory[10]);
    Assert.Equal(6, cpu.InstructionPointer);
}



[Fact]
public void Read_StoresFirstInputValue_AndAdvancesInstructionPointer()
{
    var cpu = new CpuState();
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
public void Write_OutputsMemoryValue_AndAdvancesIP()
{
    var cpu = new CpuState();
    cpu.InstructionPointer = 5;
    cpu.Memory[20] = 99;

    string capturedOutput = string.Empty;
    cpu.OnOutputMessage = message => capturedOutput = message;

    InputOutput.Write(11, 20, cpu);

    Assert.False(string.IsNullOrWhiteSpace(capturedOutput));
    Assert.Contains("99", capturedOutput);
    Assert.Equal(6, cpu.InstructionPointer);
}
}