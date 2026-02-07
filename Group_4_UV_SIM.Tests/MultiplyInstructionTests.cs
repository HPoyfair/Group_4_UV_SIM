using Xunit;
using Group_4_UV_SIM; // <- your namespace
public class MultiplyTests
{
    [Fact]
    public void Multiply_ValidOperand_MultipliesAccumulator()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 5;
        cpu.Memory[4] = 4;
        cpu.InstructionPointer = 5;

        Arithmetic.Multiply(31, 4, cpu);

        Assert.Equal(20, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
     }

    [Fact]
    public void Multiply_ByZero_SetsAccumulatorToZero()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 5;
        cpu.Memory[4] = 0;
        cpu.InstructionPointer = 5;

        Arithmetic.Multiply(31, 4, cpu);

        Assert.Equal(0, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
     }
}