using Xunit;
using Group_4_UV_SIM; // <- your namespace


public class InputOutputTests
{
    [Fact]
    public void Input_ReadsValueIntoAccumulator()
    {
            var cpu = new CpuState();
            cpu.InstructionPointer = 5;
    
            // Simulate input by directly setting the value in memory
            cpu.Memory[10] = 42; // Let's say the input instruction reads from address 10
    
            InputOutput.Read(30, 10, cpu);
    
            Assert.Equal(42, cpu.Accumulator);
            Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Output_WritesAccumulatorToMemory()
    {
            var cpu = new CpuState();
            cpu.Accumulator = 99;
            cpu.InstructionPointer = 5;
    
            InputOutput.Write(31, 20, cpu);
    
            Assert.Equal(99, cpu.Memory[20]);
            Assert.Equal(6, cpu.InstructionPointer);
       
    }
}