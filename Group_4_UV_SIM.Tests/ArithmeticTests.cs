using Xunit;
using Group_4_UV_SIM;

public class ArithmeticTests
{
    // =================== ADD TESTS =================

    [Fact]
    public void Add_ValidOperand_AddsCorrectly_Legacy()
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
    public void Add_InvalidOperand_DoesNotChangeAccumulator_Legacy()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5;
        cpu.InstructionPointer = 0;

        Arithmetic.Add(30, 100, cpu);

        Assert.Equal(5, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_ValidExtendedAddress_AddsCorrectly_Extended()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.Accumulator = 10;
        cpu.Memory[150] = 15;

        Arithmetic.Add(30, 150, cpu);

        Assert.Equal(25, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_InvalidOperand_DoesNotChangeAccumulator_Extended()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.Accumulator = 5;
        cpu.InstructionPointer = 0;

        Arithmetic.Add(30, 250, cpu);

        Assert.Equal(5, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_LegacyOverflow_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 9999;
        cpu.Memory[1] = 1;

        Arithmetic.Add(30, 1, cpu);

        Assert.Equal(9999, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Add_ExtendedOverflow_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.Accumulator = 999999;
        cpu.Memory[1] = 1;

        Arithmetic.Add(30, 1, cpu);

        Assert.Equal(999999, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    // =================== SUBTRACT TESTS =================

    [Fact]
    public void Subtract_Valid_SubtractsMemoryFromAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.InstructionPointer = 0;
        cpu.Accumulator = 50;
        cpu.Memory[3] = 20;

        Arithmetic.Subtract(31, 3, cpu);

        Assert.Equal(30, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Subtract_InvalidOperand_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 50;
        cpu.InstructionPointer = 0;

        Arithmetic.Subtract(31, 100, cpu);

        Assert.Equal(50, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    // =================== MULTIPLY TESTS =================

    [Fact]
    public void Multiply_ValidOperand_MultipliesAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5;
        cpu.Memory[4] = 4;
        cpu.InstructionPointer = 5;

        Arithmetic.Multiply(33, 4, cpu);

        Assert.Equal(20, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Multiply_ByZero_SetsAccumulatorToZero()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5;
        cpu.Memory[4] = 0;
        cpu.InstructionPointer = 5;

        Arithmetic.Multiply(33, 4, cpu);

        Assert.Equal(0, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Multiply_LegacyOverflow_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 5000;
        cpu.Memory[4] = 3;
        cpu.InstructionPointer = 0;

        Arithmetic.Multiply(33, 4, cpu);

        Assert.Equal(5000, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    [Fact]
    public void Multiply_ExtendedOverflow_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Extended6Digit;
        cpu.Accumulator = 500000;
        cpu.Memory[4] = 3;
        cpu.InstructionPointer = 0;

        Arithmetic.Multiply(33, 4, cpu);

        Assert.Equal(500000, cpu.Accumulator);
        Assert.Equal(1, cpu.InstructionPointer);
    }

    // =================== DIVIDE TESTS =================

    [Fact]
    public void Divide_ValidOperand_DividesAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 20;
        cpu.InstructionPointer = 5;
        cpu.Memory[4] = 4;

        Arithmetic.Divide(32, 4, cpu);

        Assert.Equal(5, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Divide_ByZero_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 20;
        cpu.InstructionPointer = 5;
        cpu.Memory[0] = 0;

        Arithmetic.Divide(32, 0, cpu);

        Assert.Equal(20, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }

    [Fact]
    public void Divide_InvalidOperand_DoesNotChangeAccumulator()
    {
        var cpu = new CpuState();
        cpu.Format = ProgramFormat.Legacy4Digit;
        cpu.Accumulator = 20;
        cpu.InstructionPointer = 5;

        Arithmetic.Divide(32, 100, cpu);

        Assert.Equal(20, cpu.Accumulator);
        Assert.Equal(6, cpu.InstructionPointer);
    }
}