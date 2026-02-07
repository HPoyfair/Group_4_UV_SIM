using Xunit;
using Group_4_UV_SIM;
using System;

namespace Group_4_UV_SIM.Tests
{
	public class MemoryLoadTests
	{
		   [Fact]
		   public void LOAD_ValidMemory_UpdatesAccumulator()
		   {
			   var cpu = new CpuState();
			   cpu.Memory[10] = 1234;
			   cpu.Accumulator = 0;
			   LoadStore.Load(0, 10, cpu);
			   Assert.Equal(1234, cpu.Accumulator);
		   }

		   [Fact]
		   public void LOAD_InvalidMemoryAddress_ThrowsException()
		   {
			   var cpu = new CpuState();
			   Assert.Throws<IndexOutOfRangeException>(() => LoadStore.Load(0, 100, cpu));
			   Assert.Throws<IndexOutOfRangeException>(() => LoadStore.Load(0, -1, cpu));
		   }
	}
}
