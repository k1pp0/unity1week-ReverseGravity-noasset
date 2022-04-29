using System;
using GameJamUtility;
using UniRx;
using UnityEngine;
using Zenject;

public class GameInputHandler : IGameInputHandler, IInitializable, IDisposable, ITickable
{
    private Subject<Unit> onSpacePressed = new Subject<Unit>();
    IObservable<Unit> IGameInputHandler.OnSpacePressed => onSpacePressed;
    
    private readonly CompositeDisposable disposable = new CompositeDisposable();

    void IInitializable.Initialize()
    {
        
    }

    void IDisposable.Dispose()
    {
        disposable.Dispose();
    }

    void ITickable.Tick()
    {
        
#if UNITY_IOS
        if (Input.GetMouseButtonDown(0))
#else
        if (Input.GetKeyDown(KeyCode.Space))
#endif
        {        
            onSpacePressed.OnNext(Unit.Default);
        }
    }
}
