using UnityEngine;

namespace Core.Scripts.View
{
    public class Planet : UniversalGravitationBehaviour
    {
        [Range(0.01f, 1f)] public float rotateSpeed;

        private void Update()
        {
            UgTransform.Rotate(0, rotateSpeed, 0);
        }
    }
}
