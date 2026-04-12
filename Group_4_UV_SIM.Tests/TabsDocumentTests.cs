using System.Text;
using Xunit;
using Group_4_UV_SIM;

namespace Group_4_UV_SIM.Tests;

public class TabsStateTests
{
    [Fact]
    public void SeparateCpuStates_MaintainIndependentMemory()
    {
        var cpu1 = new CpuState();
        var cpu2 = new CpuState();

        cpu1.Memory[0] = 1111;
        cpu2.Memory[0] = 2222;

        Assert.Equal(1111, cpu1.Memory[0]);
        Assert.Equal(2222, cpu2.Memory[0]);
        Assert.NotEqual(cpu1.Memory[0], cpu2.Memory[0]);
    }

    [Fact]
    public void SeparateCpuStates_MaintainIndependentRegisters()
    {
        var cpu1 = new CpuState();
        var cpu2 = new CpuState();

        cpu1.Accumulator = 8;
        cpu1.InstructionPointer = 4;

        cpu2.Accumulator = 9;
        cpu2.InstructionPointer = 7;

        Assert.Equal(8, cpu1.Accumulator);
        Assert.Equal(4, cpu1.InstructionPointer);
        Assert.Equal(9, cpu2.Accumulator);
        Assert.Equal(7, cpu2.InstructionPointer);
    }

    [Fact]
    public void SeparateOutputLogs_RemainIndependent()
    {
        var output1 = new StringBuilder();
        var output2 = new StringBuilder();

        output1.AppendLine("> +0008");
        output2.AppendLine("> +0009");

        Assert.Contains("+0008", output1.ToString());
        Assert.DoesNotContain("+0008", output2.ToString());
        Assert.Contains("+0009", output2.ToString());
    }
}
