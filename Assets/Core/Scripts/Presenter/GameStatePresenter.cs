using Core.Scripts.Model;
using Core.Scripts.View;
using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.Scripts.Presenter
{
    public class GameStatePresenter : MonoBehaviour
    {
        [Inject] private IGameInputHandler gameInputHandler;
        [Inject] private IGameStateProvider gameStateProvider;
        [Inject] private IGameTimerProvider gameTimerProvider;
        [Inject] private ISceneTransitionManager sceneTransitionManager;

        [SerializeField] private GameUIView gameUiView;
        [SerializeField] private SoundManager soundManager;

        private float _time;

        private const string Url = "https://unityroom.com/games/reverse-gravity";

        private void Start()
        {
            gameStateProvider.SetState(GameState.GameReady);

            // Ready -> Play
            gameInputHandler.OnSpacePressed
                .Where(_ => gameStateProvider.IsGameReady())
                .Take(1)
                .Subscribe(_ =>
                {
                    gameStateProvider.SetState(GameState.GamePlay);
                    gameUiView.HideDescription();
                    gameTimerProvider.StartTimer();
                    soundManager.PlayNext();
                }).AddTo(this);

            // Over -> Title
            gameInputHandler.OnSpacePressed
                .Where(_ => gameStateProvider.IsGameOver())
                .Take(1)
                .Subscribe(async _ =>
                {
                    soundManager.PlayNext();
                    sceneTransitionManager.Run("Title", 6);
                }).AddTo(this);

            // Clear -> Ranking
            gameInputHandler.OnSpacePressed
                .Where(_ => gameStateProvider.IsGameClear())
                .Take(1)
                .Subscribe(async _ =>
                {
                    soundManager.PlayNext();
                    gameUiView.HideGameClear();
                    gameUiView.ShowRanking(_time);
                }).AddTo(this);

            // Ranking -> Finish
            this.UpdateAsObservable()
                .Select(_ => SceneManager.sceneCount)
                .Select(_ => _ == 2)
                .Pairwise()
                .Where(_ => _.Previous && !_.Current)
                .Subscribe(_ =>
                {
                    gameStateProvider.SetState(GameState.GameFinish);
                    gameUiView.ShowGameClear();
                    gameUiView.HideRanking();
                }).AddTo(this);

            // Finish -> Title
            gameInputHandler.OnSpacePressed
                .Where(_ => gameStateProvider.IsFinish())
                .Take(1)
                .Subscribe(async _ =>
                {
                    soundManager.PlayNext();
                    sceneTransitionManager.Run("Title", 6);
                }).AddTo(this);

            // Play -> Over
            gameStateProvider.CurrentState
                .Where(state => state == GameState.GameOver)
                .Take(1)
                .Subscribe(_ => { gameUiView.ShowGameOver(); }).AddTo(this);

            // Play -> Clear
            gameStateProvider.CurrentState
                .Where(state => state == GameState.GameClear)
                .Take(1)
                .Subscribe(_ => { gameUiView.ShowGameClear(); }).AddTo(this);

            // Elapsed Time
            gameTimerProvider.ElapsedTime
                .Where(_ => gameStateProvider.IsGamePlay())
                .Select(ConvertTimeFormat)
                .Subscribe(gameUiView.SetGameTime)
                .AddTo(this);

            // Clear time
            gameTimerProvider.ElapsedTime
                .Where(_ => gameStateProvider.IsGameClear())
                .Take(1)
                .Do(_ => _time = _)
                .Select(ConvertTimeFormat)
                .Subscribe(gameUiView.SetClearTime)
                .AddTo(this);

            // Tweet
            gameUiView.OnGameClearTweetButtonClicked
                .Subscribe(_ =>
                {
                    var format = ConvertTimeFormat(_time);
                    var text =
                        $"*ロボット* ハ {format.Minute:D2}:{format.Second:D2}:{format.MillSecond:D2} デ チキュウニ キカンシタ " +
                        "https://unityroom.com/games/reverse-gravity";
                    StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot(text));
                }).AddTo(this);

            // Tweet
            gameUiView.OnGameOverTweetButtonClicked
                .Subscribe(_ =>
                {
                    var text = "*ロボット* ハ タイヨウケイカラ ハナレスギテ シマッタ... " +
                               "*ロボット* ニ ノコサレタノハ ウチュウクウカンヲ タダヨイツヅケル コトノミ... " +
                               "--ソノウチ *ロボット* ハ カンガエルノヲ ヤメタ... " +
                               "https://unityroom.com/games/reverse-gravity";
                    StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot(text));
                }).AddTo(this);
        }

        private TimeFormat ConvertTimeFormat(float t)
        {
            var m = Mathf.FloorToInt(t) / 60;
            var s = Mathf.FloorToInt(t) % 60;
            var ms = Mathf.FloorToInt((t - Mathf.FloorToInt(t)) * 100);
            return new TimeFormat {Minute = m, Second = s, MillSecond = ms};
        }
    }
}