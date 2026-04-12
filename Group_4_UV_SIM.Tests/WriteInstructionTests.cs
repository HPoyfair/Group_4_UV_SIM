using System;
using System.IO;
using Xunit;
using Group_4_UV_SIM;

namespace Group_4_UV_SIM.Tests;

public class WriteInstructionTests
{
    [Fact]
        public void Write_Valid_PrintsMemoryValue(){
        // UC-05 success: Display value in memory[operand] to console
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        int operand = 7;
        cpu.Memory[operand] = 42;

        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetOut(output);

            InputOutput.Write(11, operand, cpu);

            string text = output.ToString();

            // Exact output format from your implementation:
            // memory[07] = +0042
            Assert.Contains("memory[07] = +0042", text);
            Assert.Equal(1, cpu.InstructionPointer);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

   
    [Fact]
    public void Write_InvalidAddress_PrintsError(){

        // UC-05 failure/edge: invalid memory address
        var cpu = new CpuState();
        cpu.InstructionPointer = 0;

        int badOperand = 150;

        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetOut(output);

            InputOutput.Write(11, badOperand, cpu);

            string text = output.ToString();

            Assert.Contains("Error: Invalid memory address 150 in WRITE operation.", text);
            Assert.Equal(1, cpu.InstructionPointer);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
