using Xunit;
using Group_4_UV_SIM;

public class ArithmeticTests
// This class contains tests for arithmetic instructions ADD, SUBTRACT, MULTIPLY, AND DIVIDE. 
//Note: subtract has a test in console tests.
{
    //=================== ADD TESTS =================
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
    //=================== SUBTRACT TESTS =================
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

    //=================== MULTIPLY TESTS =================
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

    //=================== DIVIDE TESTS =================
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
