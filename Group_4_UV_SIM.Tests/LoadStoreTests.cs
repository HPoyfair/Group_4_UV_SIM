using Xunit;
using Group_4_UV_SIM;

namespace Group_4_UV_SIM.Tests
{
    public class LoadStoreTests
    {
        [Fact]
        public void STORE_ValidMemory_UpdatesMemory()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Accumulator = 5678;
            cpu.Memory[15] = 0;
            cpu.InstructionPointer = 0;

            LoadStore.Store(0, 15, cpu);

            Assert.Equal(5678, cpu.Memory[15]);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void STORE_InvalidMemoryAddress_DoesNotStore_Legacy()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Accumulator = 1234;
            cpu.Memory[99] = 7777;
            cpu.InstructionPointer = 0;

            LoadStore.Store(0, 100, cpu);

            Assert.Equal(7777, cpu.Memory[99]);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void STORE_NegativeMemoryAddress_DoesNotStore()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Accumulator = 1234;
            cpu.InstructionPointer = 0;

            LoadStore.Store(0, -1, cpu);

            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void STORE_ValidExtendedMemory_UpdatesMemory()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Extended6Digit;
            cpu.Accumulator = 123456;
            cpu.Memory[150] = 0;
            cpu.InstructionPointer = 0;

            LoadStore.Store(0, 150, cpu);

            Assert.Equal(123456, cpu.Memory[150]);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void STORE_InvalidMemoryAddress_DoesNotStore_Extended()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Extended6Digit;
            cpu.Accumulator = 123456;
            cpu.Memory[249] = 888888;
            cpu.InstructionPointer = 0;

            LoadStore.Store(0, 250, cpu);

            Assert.Equal(888888, cpu.Memory[249]);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void LOAD_ValidMemory_UpdatesAccumulator()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Memory[10] = 1234;
            cpu.Accumulator = 0;
            cpu.InstructionPointer = 0;

            LoadStore.Load(0, 10, cpu);

            Assert.Equal(1234, cpu.Accumulator);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void LOAD_InvalidMemoryAddress_DoesNotChangeAccumulator_Legacy()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Accumulator = 4321;
            cpu.InstructionPointer = 0;

            LoadStore.Load(0, 100, cpu);

            Assert.Equal(4321, cpu.Accumulator);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void LOAD_NegativeMemoryAddress_DoesNotChangeAccumulator()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Legacy4Digit;
            cpu.Accumulator = 4321;
            cpu.InstructionPointer = 0;

            LoadStore.Load(0, -1, cpu);

            Assert.Equal(4321, cpu.Accumulator);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void LOAD_ValidExtendedMemory_UpdatesAccumulator()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Extended6Digit;
            cpu.Memory[150] = 654321;
            cpu.Accumulator = 0;
            cpu.InstructionPointer = 0;

            LoadStore.Load(0, 150, cpu);

            Assert.Equal(654321, cpu.Accumulator);
            Assert.Equal(1, cpu.InstructionPointer);
        }

        [Fact]
        public void LOAD_InvalidMemoryAddress_DoesNotChangeAccumulator_Extended()
        {
            var cpu = new CpuState();
            cpu.Format = ProgramFormat.Extended6Digit;
            cpu.Accumulator = 111111;
            cpu.InstructionPointer = 0;

            LoadStore.Load(0, 250, cpu);

            Assert.Equal(111111, cpu.Accumulator);
            Assert.Equal(1, cpu.InstructionPointer);
        }
    }
}