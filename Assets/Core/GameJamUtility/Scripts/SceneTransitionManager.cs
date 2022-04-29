using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameJamUtility
{
    public class SceneTransitionManager : ISceneTransitionManager
    {
        [Inject] private TransitionStyleManager transitionStyleManager;

        public async void Run(string name, int index)
        {
            await transitionStyleManager.Play(index);
            await SceneManager.LoadSceneAsync(name);
            transitionStyleManager.Close(index);
        }
    }
}