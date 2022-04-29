using Core.Scripts.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Scripts.Presenter
{
    public class LinePresenter : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform goal;
        [SerializeField] private LineRenderer lineRenderer;
        void Start()
        {
            lineRenderer.positionCount = 2;
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    lineRenderer.SetPosition(0, player.CenterPosition);
                    lineRenderer.SetPosition(1, goal.position);
                }).AddTo(this);
        }
    }
}
