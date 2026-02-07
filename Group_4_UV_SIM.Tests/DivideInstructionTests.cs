using Xunit;
using Group_4_UV_SIM;

public class DivideTests
{
    [Fact]
    public void Divide_ValidOperand_DividesAccumulator()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 20;
        cpu.InstructionPointer = 5;

        Arithmetic.Divide(32, 4, cpu);

        Assert.Equal(5, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Divide_ByZero_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 20;
        cpu.InstructionPointer = 5;

        Arithmetic.Divide(32, 0, cpu);

        
        Assert.Equal(20, cpu.Accumulator);

        
        Assert.Equal(6, cpu.InstructionPointer);
    }
}
