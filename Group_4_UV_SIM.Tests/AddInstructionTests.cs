using Xunit;
using Group_4_UV_SIM;

public class AddInstructionTests
{
    [Fact]
    public void Add_ValidOperand_AddsCorrectly()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 5;
        cpu.Memory[10] = 3;

        Arithmetic.Add(30, 10, cpu);

        Assert.Equal(8, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_InvalidOperand_Throw_Exception()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 5;
        cpu.InstructionPointer = 0;

        Assert.Throws<IndexOutOfRangeException>(() =>
            Arithmetic.Add(30, 101, cpu)
        );
    }

    
}