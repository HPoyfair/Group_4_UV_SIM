namespace Group_4_UV_SIM;

public class CpuState
{
    // this breaks the ui currently!!!!! see form 1 to fix it and adjust size of actual gui in designer.cs
    public int[] Memory { get; } = new int[250];
    public int Accumulator { get; set; }
    public int InstructionPointer { get; set; }
    public bool Halted { get; set; }

    public ProgramFormat Format { get; set; } = ProgramFormat.Legacy4Digit;

    public event Action StateChanged;

    public Func<string, int> OnRequestInput;
    public Action<string> OnOutputMessage;

    public void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}