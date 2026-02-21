using Xunit;
using Group_4_UV_SIM;
using MyControl = Group_4_UV_SIM.Control;

public class ControlTests
{
    // ================= Branch (always jump) =================

    [Fact]
    public void Branch_Always_JumpsToOperand()
    {
        var cpu = new CpuState();
        cpu.InstructionPointer = 5;

        MyControl.Branch(40, 17, cpu);

        Assert.Equal(17, cpu.InstructionPointer);
    }

    // ================= BranchZero =================

    [Fact]
    public void BranchZero_WhenAccumulatorZero_Jumps()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 0;
        cpu.InstructionPointer = 5;

        MyControl.BranchZero(42, 17, cpu);

        Assert.Equal(17, cpu.InstructionPointer);
    }

    [Fact]
    public void BranchZero_WhenAccumulatorNotZero_Advances()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 10;
        cpu.InstructionPointer = 5;

        MyControl.BranchZero(42, 17, cpu);

        Assert.Equal(6, cpu.InstructionPointer);
    }

    // ================= BranchNeg =================

    [Fact]
    public void BranchNeg_WhenAccumulatorNegative_Jumps()
    {
        var cpu = new CpuState();
        cpu.Accumulator = -1;
        cpu.InstructionPointer = 5;

        MyControl.BranchNeg(41, 17, cpu);

        Assert.Equal(17, cpu.InstructionPointer);
    }

    [Fact]
    public void BranchNeg_WhenAccumulatorPositive_Advances()
    {
        var cpu = new CpuState();
        cpu.Accumulator = 1;
        cpu.InstructionPointer = 5;

        MyControl.BranchNeg(41, 17, cpu);

        Assert.Equal(6, cpu.InstructionPointer);
    }
}
