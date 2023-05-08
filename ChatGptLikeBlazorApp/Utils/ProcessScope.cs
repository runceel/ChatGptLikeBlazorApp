namespace ChatGptLikeBlazorApp.Utils;

public class ProcessScope
{
    public bool IsRunning { get; private set; }
    public bool IsNotRunning => !IsRunning;

    public IDisposable Start()
    {
        if (IsRunning) { return NullProcess.Instance; }

        IsRunning = true;
        return new Process(this, state => state.IsRunning = false);
    }
}

file class NullProcess : IDisposable
{
    public static IDisposable Instance { get; } = new NullProcess();
    public void Dispose()
    {
    }
}

class Process : IDisposable
{
    private readonly ProcessScope _state;
    private readonly Action<ProcessScope> _dispose;

    public Process(ProcessScope state, Action<ProcessScope> dispose)
    {
        _state = state;
        _dispose = dispose;
    }

    public void Dispose() => _dispose(_state);
}
