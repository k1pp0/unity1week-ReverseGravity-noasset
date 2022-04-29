using System.Collections;
using UnityEngine;

namespace Core.Scripts.View
{
    public class SpawnBelts : MonoBehaviour
    {
        [SerializeField] private float width;
        [SerializeField] private float height;
        [SerializeField] private float force;

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            foreach (Transform child in transform)
            {
                child.transform.localPosition = new Vector3(Random.Range(-width / 2, width / 2),
                    Random.Range(-height / 2, height / 2), 0);
                var rb = child.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(Random.Range(-force, force),
                    Random.Range(-force, force), 0), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-force, force),
                    Random.Range(-force, force), 0), ForceMode.Impulse);
                yield return null;
            }
        }
    }
}