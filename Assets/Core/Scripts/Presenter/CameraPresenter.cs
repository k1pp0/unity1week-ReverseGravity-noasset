using System.Linq;
using Core.Scripts.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Scripts.Presenter
{
    public class CameraPresenter : MonoBehaviour
    {
        [SerializeField] private CameraTracker cameraTracker;

        [SerializeField] private Player player;
        [SerializeField] private Planet[] planets;

        private const float th = 10.0f;

        void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => GetSurroundingPlanetsCenterPosition())
                .Subscribe(cameraTracker.SetTargetPosition)
                .AddTo(this);
        }

        private Vector3 GetSurroundingPlanetsCenterPosition()
        {
            var surroundings = planets
                .Where(planet => Vector3.Distance(planet.transform.position, player.transform.position) < th)
                .Select(planet => planet.transform.position)
                .ToList();
        
            surroundings.Add(player.transform.position);

            return surroundings.Aggregate(Vector3.zero,
                (sum, planet) => sum + planet / surroundings.Count);
        }
    }
}