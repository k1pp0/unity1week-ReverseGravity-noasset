using UnityEngine;

namespace Core.Scripts.View
{
    [RequireComponent(typeof(Transform))]
    public class CameraTracker : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Transform _stabilizer;
        private Transform _target;

        [SerializeField] private float distance;
        [SerializeField] private float speed;

        private void Awake()
        {
            _cameraTransform = GetComponent<Transform>();
            _stabilizer = new GameObject("Stabilizer").transform;
            _target = new GameObject("Target").transform;
        }

        public void SetTargetPosition(Vector3 target)
        {
            _target.position = Vector3.Lerp(_target.position, target, speed * Time.deltaTime);
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                _target.position + Vector3.back * distance, speed * Time.deltaTime);
            _stabilizer.position =
                Vector3.Lerp(_stabilizer.position, _cameraTransform.position, speed * Time.deltaTime);
            _stabilizer.LookAt(_target);
            _cameraTransform.rotation =
                Quaternion.Lerp(_cameraTransform.rotation, _stabilizer.rotation, speed * Time.deltaTime);
        }
    }
}