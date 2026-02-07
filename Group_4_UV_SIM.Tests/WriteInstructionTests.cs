using Xunit;
using Group_4_UV_SIM;
using System;
using System.IO;

namespace Group_4_UV_SIM.Tests
{
	public class WriteInstructionTests
	{
		[Fact]
		public void WRITE_ValidMemory_WritesCorrectValue()
		{
			var cpu = new CpuState();
			cpu.Memory[22] = -4321;
			var sw = new StringWriter();
			var originalOut = Console.Out;
			Console.SetOut(sw);
			Operations.InputOutput.ExecuteWrite(cpu, 22);
			Console.SetOut(originalOut);
			var output = sw.ToString().Trim();
			Assert.Equal("-4321", output);
		}

		[Fact]
		public void WRITE_InvalidMemoryAddress_ThrowsException()
		{
			var cpu = new CpuState();
			Assert.Throws<IndexOutOfRangeException>(() => Operations.InputOutput.ExecuteWrite(cpu, 100));
			Assert.Throws<IndexOutOfRangeException>(() => Operations.InputOutput.ExecuteWrite(cpu, -1));
		}
	}
}
