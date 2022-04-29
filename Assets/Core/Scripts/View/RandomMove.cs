using UnityEngine;

namespace Core.Scripts.View
{
    public class RandomMove : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Update()
        {
            transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0) * speed;
        }
    }
}
