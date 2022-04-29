using System.Collections;
using Core.Scripts.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.View
{
    public class GameUIView : MonoBehaviour
    {
        [Header("GamePlay")]
        [SerializeField] private CanvasGroup description;
        [SerializeField] private Text distance;
        [SerializeField] private Text gameTime0;
        [SerializeField] private Text gameTime1;
        
        [Header("GameOver")]
        [SerializeField] private CanvasGroup gameOver;
        [SerializeField] private Button overTweetButton;

        
        [Header("GameClear")]
        [SerializeField] private CanvasGroup gameClear;
        [SerializeField] private Text clearTime0;
        [SerializeField] private Text clearTime1;
        [SerializeField] private Text clearNext;
        [SerializeField] private Text clearText;
        [SerializeField] private Button clearTweetButton;

        private const float FadeTime = 0.5f;
        
        public Subject<Unit> OnGameClearTweetButtonClicked = new Subject<Unit>();
        public Subject<Unit> OnGameOverTweetButtonClicked = new Subject<Unit>();

        private void Start()
        {
            ShowDescription();

            clearTweetButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    OnGameClearTweetButtonClicked.OnNext(Unit.Default);
                }).AddTo(this);
            
            overTweetButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    OnGameOverTweetButtonClicked.OnNext(Unit.Default);
                }).AddTo(this);
        }
        
        public void ShowDescription()
        {
            StartCoroutine(FadeIn(description));
        }

        public void HideDescription()
        {
            StartCoroutine(FadeOut(description));
        }
        
        public void ShowGameOver()
        {
            overTweetButton.interactable = true;
            StartCoroutine(FadeIn(gameOver));
        }

        public void HideGameOver()
        {
            overTweetButton.interactable = false;
            StartCoroutine(FadeOut(gameOver));
        }
        
        public void ShowGameClear()
        {
            clearTweetButton.interactable = true;
            StartCoroutine(FadeIn(gameClear));
        }

        public void HideGameClear()
        {
            clearTweetButton.interactable = false;
            StartCoroutine(FadeOut(gameClear));
        }

        public void ShowRanking(float t)
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking (Mathf.FloorToInt(t * 1000));
        }

        public void HideRanking()
        {
            clearNext.text = "Press Space To Title";
        }

        public void SetDistance(float d)
        {
            distance.text = $"チキュウマデ:{Mathf.FloorToInt(d * 1000000):D9}{Random.Range(0, 9)}km";
        }
        
        public void SetGameTime(TimeFormat format)
        {
            gameTime0.text = $"{format.Minute:D2}:{format.Second:D2}";
            gameTime1.text = $":{format.MillSecond:D2}";
        }
        
        public void SetClearTime(TimeFormat format)
        {
            clearTime0.text = $"{format.Minute:D2}:{format.Second:D2}";
            clearTime1.text = $":{format.MillSecond:D2}";
        }

        public void SetClearText(ScoreInfo info)
        {
            var infos = 0;
            var text = "*ロボット* ハ ";
            
            // Gravity Flip
            if (info.IsFewFlips || info.IsManyFlips)
            {
                text += info.IsFewFlips ? "ウチュウノ インリョクニ ミチビカレ\n" : "ウチュウノ インリョクヲ ネジマゲテ\n";
                infos++;
            }

            // Solar System Distance
            if (info.IsNearDistance || info.IsLongDistance)
            {
                text += info.IsNearDistance ? "タイヨウケイカラ ハナレルコトナク" : "タイヨウケカラ ハナレナガラモ";
                infos++;
                text += infos % 2 == 1 ? "\n" : " ";
            }
            
            // Planets
            if (info.IsAllPlanets || info.IsNonePlanets)
            {
                text += info.IsAllPlanets ? "スベテノ ワクセイヲ ケイユシテ" : "ワクセイト ショウトツセズニ";
                infos++;
                text += infos % 2 == 1 ? "\n" : " ";
            }
            
            // Velocity
            if (info.IsHighSpeed || info.IsLowSpeed)
            {
                text += info.IsHighSpeed ? "コウソクデ " : "ユックリト ";
            }

            // Collision
            if (!info.IsManyCollisions && !info.IsSunCollision)
            {
                text += "ブジ ";
            }

            text += "チキュウニ キカンシタ";

            if (info.IsManyCollisions || info.IsSunCollision)
            {
                text += "\n";
            }
            
            if (info.IsManyCollisions)
            {
                text += "ショウトツデ ボロボロニナリ ";
            }
            if (info.IsSunCollision)
            {
                text += "タイヨウノ ホノオニ ツツマレ ";
            }

            if (info.IsManyCollisions || info.IsSunCollision)
            {
                text += info.IsManyCollisions && info.IsSunCollision ? "テ..." : "ナガラ...";
            }
            clearText.text = text;
        }
        
        public static IEnumerator FadeIn(CanvasGroup canvasGroup)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            var t = 0f;
            while (t < FadeTime)
            {
                t += Time.deltaTime;
                var rate = t / FadeTime;
                canvasGroup.alpha = Mathf.Clamp01(rate);
                yield return null;
            }
        }

        public static IEnumerator FadeOut(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            var t = 0f;
            while (t < FadeTime)
            {
                t += Time.deltaTime;
                var rate = t / FadeTime;
                canvasGroup.alpha = 1f - Mathf.Clamp01(rate);
                yield return null;
            }
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
