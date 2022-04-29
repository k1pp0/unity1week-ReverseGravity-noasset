using System;
using UniRx;

namespace GameJamUtility
{
    public interface IGameInputHandler
    {
        IObservable<Unit> OnSpacePressed { get; }
    }
}
