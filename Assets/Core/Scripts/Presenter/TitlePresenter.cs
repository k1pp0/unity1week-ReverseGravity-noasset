using Core.Scripts.View;
using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Presenter
{
    public class TitlePresenter : MonoBehaviour
    {
        [Inject] private ISceneTransitionManager sceneTransitionManager;
        [Inject] private IGameInputHandler gameInputHandler;
        [Inject] private BGMManager bgmManager;
    
        [SerializeField] private Player player;
        [SerializeField] private Planet earth;
    
        [SerializeField] private MainCameraPostEffect mainCameraPostEffect;
        [SerializeField] private SoundManager soundManager;
    
        private bool _reverseGravity = true;
        private int _timer = 300;

        private void Start()
        {
            gameInputHandler.OnSpacePressed
                .Take(1)
                .Subscribe(_ =>
                {
                    soundManager.PlayNext();
                    sceneTransitionManager.Run("Main", 6);
                })
                .AddTo(this);
        }

        private void Update()
        {
            player.AddForce(player.Exert(earth, _reverseGravity));
            if (_timer == 0)
            {
                mainCameraPostEffect.SetReverseGravityDirection(_reverseGravity);
                _timer = _reverseGravity ? 100 : 300;
                _reverseGravity = !_reverseGravity;
            }
            _timer--;
        }
    }
}
