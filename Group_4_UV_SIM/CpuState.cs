
namespace Group_4_UV_SIM;
public class CpuState
{
    public int[] Memory { get; } = new int[100];
    public int Accumulator { get; set; }
    public int InstructionPointer { get; set; }
    public bool Halted { get; set; }

    public event Action StateChanged;

    public Func<string, int> OnRequestInput;
    public Action<string> OnOutputMessage;


    public void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
