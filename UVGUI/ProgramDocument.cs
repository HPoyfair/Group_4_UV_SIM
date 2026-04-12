using System.Text;
using Group_4_UV_SIM;

namespace UVGUI
{
    public class ProgramDocument
    {
        public string Title { get; set; } = "Untitled";
        public string? FilePath { get; set; }
        public CpuState Cpu { get; }
        public MemoryEditor MemoryEditor { get; }
        public bool IsDirty { get; set; }
        public StringBuilder OutputLog { get; } = new StringBuilder();

        public ProgramDocument(CpuState cpu)
        {
            Cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
            MemoryEditor = new MemoryEditor(cpu);
        }
    }
}
