using UnityEngine;

namespace Core.Scripts.View
{
    public class SkyBoxRotater : MonoBehaviour
    {
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        [Range(0.01f, 0.1f)] public float rotateSpeed;
        [SerializeField] private Material sky;
        private float _rotationRepeatValue;

        void Update()
        {
            _rotationRepeatValue = Mathf.Repeat(sky.GetFloat(Rotation) + rotateSpeed, 360f);
            sky.SetFloat(Rotation, _rotationRepeatValue);
        }
    }
}