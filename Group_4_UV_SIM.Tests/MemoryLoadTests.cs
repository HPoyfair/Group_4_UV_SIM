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
			Operations.LoadStore.ExecuteLoad(cpu, 10);
			Assert.Equal(1234, cpu.Accumulator);
		}

		[Fact]
		public void LOAD_InvalidMemoryAddress_ThrowsException()
		{
			var cpu = new CpuState();
			Assert.Throws<IndexOutOfRangeException>(() => Operations.LoadStore.ExecuteLoad(cpu, 100));
			Assert.Throws<IndexOutOfRangeException>(() => Operations.LoadStore.ExecuteLoad(cpu, -1));
		}
	}
}
