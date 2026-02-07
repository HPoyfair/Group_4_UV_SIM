using System;
using System.IO;
using Xunit;
using Group_4_UV_SIM;

namespace Group_4_UV_SIM.Tests;

public class SubtractInstructionTests
{
    [Fact]
    public void Subtract_Valid_SubtractsMemoryFromAccumulator()
    {
        // UC-09 success: accumulator = accumulator - memory[operand]
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        int operand = 3;
        cpu.Accumulator = 50;
        cpu.Memory[operand] = 20;

        // Act
        // opcode 31 = SUBTRACT
        Arithmetic.Subtract(31, operand, cpu);

        // Assert
        Assert.Equal(30, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

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
}
