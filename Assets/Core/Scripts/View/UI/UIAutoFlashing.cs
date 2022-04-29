using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.View.UI
{
    [RequireComponent(typeof(Text))]
    public class UIAutoFlashing : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        private int _timer = 0;
        public float speed;
        public float min;

        private void Update()
        {
            canvasGroup.alpha = (Mathf.Sin(Mathf.Deg2Rad * _timer * speed) + 1) * (1 - min) / 2 + min;
            _timer++;
        }
    }
}
