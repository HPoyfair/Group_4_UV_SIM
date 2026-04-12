using Xunit;
using Group_4_UV_SIM;

public class AddInstructionTests
{
    [Fact]
    public void Add_ValidOperand_AddsCorrectly()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5;
        cpu.Memory[10] = 3;

        Arithmetic.Add(30, 10, cpu);

        Assert.Equal(8, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_InvalidOperand_DoesNotThrow_AndAdvancesInstructionPointer()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5;
        cpu.InstructionPointer = 0;

        var exception = Record.Exception(() => Arithmetic.Add(30, 101, cpu));

        Assert.Null(exception);
        Assert.Equal(5, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }
}
