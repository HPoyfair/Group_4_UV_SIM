using Xunit;
using Group_4_UV_SIM;
using System;

namespace Group_4_UV_SIM.Tests
{
	public class LoadStoreTests
	{
		   [Fact]
		   public void STORE_ValidMemory_UpdatesMemory()
		   {
			   var cpu = new CpuState();
			   cpu.Accumulator = 5678;
			   cpu.Memory[15] = 0;
			   LoadStore.Store(0, 15, cpu);
			   Assert.Equal(5678, cpu.Memory[15]);
		   }

		   [Fact]
		   public void STORE_InvalidMemoryAddress_ThrowsException()
		   {
			   var cpu = new CpuState();
			   cpu.Accumulator = 1234;
			   Assert.Throws<IndexOutOfRangeException>(() => LoadStore.Store(0, 100, cpu));
			   Assert.Throws<IndexOutOfRangeException>(() => LoadStore.Store(0, -1, cpu));
		   }


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
