using System.Linq;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace GameJamUtility
{
    public class TransitionStyle : MonoBehaviour
    {
        private const string PanelFadeIn = "Panel Open";
        private const string PanelFadeOut = "Panel Close";
        private const string StyleExpand = "Expand";
        private const string StyleClose = "Close";

        private bool isContainClose = true;

        [Header("PANEL ANIMATOR")] [SerializeField]
        private Animator panel;

        [Header("STYLE ANIMATOR")] [SerializeField]
        private Animator style;

        private void Awake()
        {
            // var controller = style.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            // if (controller != null)
            // {
            //     isContainClose = controller.layers[0].stateMachine.states
            //         .Select(state => state.state.name)
            //         .Contains(StyleClose);
            // }
        }

        public UniTask Play()
        {
            panel.Play(PanelFadeIn, 0, 0);
            style.Play(StyleExpand, 0, 0);

            if (isContainClose)
            {
                return style.ObserveEveryValueChanged(_ => style.GetCurrentAnimatorStateInfo(0))
                    .Skip(1)
                    .Select(state => state.IsName(StyleExpand))
                    .Pairwise()
                    .Where(pair => pair.Previous && !pair.Current)
                    .Take(1)
                    .Do(_ => style.speed = 0.1f)
                    .ToUniTask();
            }
            else
            {
                return style.ObserveEveryValueChanged(_ => style.GetCurrentAnimatorStateInfo(0))
                    .Skip(1)
                    .Select(state => state.normalizedTime)
                    .Where(t => t > 0.5f)
                    .Take(1)
                    .Do(_ => style.speed = 0.1f)
                    .ToUniTask();
            }
        }

        public void Close()
        {
            style.speed = 1.0f;
        }
    }
}