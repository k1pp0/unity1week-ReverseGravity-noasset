using UnityEngine;
using Zenject;

namespace GameJamUtility.Scripts.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private TransitionStyleManager transitionStyleManagerPrefab;
        [SerializeField] private BGMManager bgmManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<TransitionStyleManager>()
                .FromComponentInNewPrefab(transitionStyleManagerPrefab)
                .AsSingle();
            
            Container.Bind<BGMManager>()
                .FromComponentInNewPrefab(bgmManagerPrefab)
                .AsSingle();
        
            Container.BindInterfacesTo<SceneTransitionManager>().AsSingle();
            Container.BindInterfacesTo<GameInputHandler>().AsSingle();
        }
    }
}