using UnityEngine;

namespace Core.Scripts.View
{
    public class Satellite : MonoBehaviour
    {
        [SerializeField] private Transform planet;
        [SerializeField] private float speed;
        [SerializeField] private float radius;
        [SerializeField] private float angle;

        private void Update()
        {
            var r = (angle + speed * Time.frameCount) * Mathf.Deg2Rad;
            transform.position = planet.position + new Vector3(radius * Mathf.Cos(r), radius * Mathf.Sin(r), 0);
        }
    }
}
