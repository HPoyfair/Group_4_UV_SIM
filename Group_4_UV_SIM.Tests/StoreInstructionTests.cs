using Xunit;
using Group_4_UV_SIM;
using System;

namespace Group_4_UV_SIM.Tests
{
	public class StoreInstructionTests
	{
		[Fact]
		public void STORE_ValidMemory_UpdatesMemory()
		{
			var cpu = new CpuState();
			cpu.Accumulator = 5678;
			cpu.Memory[15] = 0;
			Operations.LoadStore.ExecuteStore(cpu, 15);
			Assert.Equal(5678, cpu.Memory[15]);
		}

		[Fact]
		public void STORE_InvalidMemoryAddress_ThrowsException()
		{
			var cpu = new CpuState();
			cpu.Accumulator = 1234;
			Assert.Throws<IndexOutOfRangeException>(() => Operations.LoadStore.ExecuteStore(cpu, 100));
			Assert.Throws<IndexOutOfRangeException>(() => Operations.LoadStore.ExecuteStore(cpu, -1));
		}
	}
}
