using GameJamUtility;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameStateProvider>().AsSingle();
        Container.BindInterfacesTo<GameTimerProvider>().AsSingle();
    }
}