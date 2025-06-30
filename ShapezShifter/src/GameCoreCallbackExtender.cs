using System;
using Core.Events;
using ShapezShifter;

public class GameCoreCallbackExtender : IDisposable
{
    public IEvent OnPreGameStart => _OnPreGameStart;
    private readonly MultiRegisterEvent _OnPreGameStart = new();

    public IEvent OnPostGameStart => _OnPostGameStart;
    private readonly MultiRegisterEvent _OnPostGameStart = new();

    public GameCoreCallbackExtender()
    {
        _OnPreGameStart.Invoke();
        GameHelper.Core.OnGameInitialized.Register(_OnPostGameStart.Invoke);
    }

    public void Dispose()
    {
        GameHelper.Core.OnGameInitialized.Unregister(_OnPostGameStart.Invoke);
        _OnPreGameStart.Dispose();
        _OnPostGameStart.Dispose();
    }
}