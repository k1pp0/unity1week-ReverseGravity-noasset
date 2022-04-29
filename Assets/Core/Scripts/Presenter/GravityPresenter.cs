using System;
using System.Linq;
using Core.Scripts.View;
using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Presenter
{
    public class GravityPresenter : MonoBehaviour
    {
        [Inject] private IGameInputHandler gameInputHandler;
        [Inject] private IGameStateProvider gameStateProvider;

        [SerializeField] private GameUIView gameUiView;
        [SerializeField] private SoundManager soundManager;

        [SerializeField] private Player player;
        [SerializeField] private Planet earth;
        [SerializeField] private Planet[] planets;

        [SerializeField] private MainCameraPostEffect mainCameraPostEffect;

        private readonly ReactiveProperty<bool> _reverseGravity = new ReactiveProperty<bool>(false);

        private void Start()
        {
            // 重力の切替
            gameInputHandler.OnSpacePressed
                .Where(_ => gameStateProvider.IsGamePlay())
                .Subscribe(_ =>
                {
                    _reverseGravity.Value = !_reverseGravity.Value;
                    player.SetGravityFlip();
                })
                .AddTo(this);

            // 重力の計算
            this.UpdateAsObservable()
                .Where(_ => !gameStateProvider.IsGameOver())
                .Select(_ => CalculateUniversalGravity())
                .Subscribe(player.AddForce)
                .AddTo(this);

            // 最も近い惑星との距離
            this.UpdateAsObservable()
                .Where(_ => gameStateProvider.IsGamePlay())
                .Select(_ => GetNearestPlanetDistance())
                .Do(player.SetMaxDistance)
                .Where(length => length > 120)
                .Take(1)
                .Subscribe(_ => { gameStateProvider.SetState(GameState.GameOver); })
                .AddTo(this);

            // ゲームクリア
            player.OnGameClear
                .Take(1)
                .Subscribe(_ =>
                {
                    gameUiView.SetClearText(_);
                    gameStateProvider.SetState(GameState.GameClear);
                }).AddTo(this);

            // 衝突音
            player.OnCollision
                .Where(_ => gameStateProvider.IsGamePlay())
                .ThrottleFirst(TimeSpan.FromMilliseconds(250))
                .Subscribe(_ => { soundManager.PlayCollision(); }).AddTo(this);

            // 色の反転
            _reverseGravity
                .Where(_ => gameStateProvider.IsGamePlay())
                .Subscribe(_ =>
                {
                    mainCameraPostEffect.SetReverseGravityDirection(_);
                    soundManager.PlayGravityChange(_);
                })
                .AddTo(this);

            // 地球までの距離
            this.UpdateAsObservable()
                .Select(_ => Mathf.Max(Vector3.Distance(player.CenterPosition, earth.transform.position) -
                                         earth.transform.localScale.x / 2 - 1.0f, 0.0f))
                .Subscribe(gameUiView.SetDistance)
                .AddTo(this);
        }

        private Vector3 CalculateUniversalGravity()
        {
            return planets.Aggregate(Vector3.zero, (sum, planet) => sum + planet.Exert(player, _reverseGravity.Value));
        }

        private float GetNearestPlanetDistance()
        {
            return planets
                .Select(planet => Vector3.Distance(planet.transform.position, player.transform.position))
                .Min();
        }
    }
}