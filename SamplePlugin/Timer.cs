using System.Diagnostics;
namespace SamplePlugin;


public class Timer
{
    private readonly Stopwatch _stopwatch = new();

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _stopwatch.Reset();
    }

    public string GetElapsedTime()
    {
        var elapsed = _stopwatch.Elapsed;
        return $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}";
    }

    public bool IsRunning => _stopwatch.IsRunning;
}
