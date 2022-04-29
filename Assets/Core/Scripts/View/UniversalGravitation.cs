using UnityEngine;

namespace Core.Scripts.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class UniversalGravitationBehaviour : MonoBehaviour
    {
        private const float G = 3.0f;

        protected Rigidbody UgRigidBody;
        protected Transform UgTransform;
        public float Mass => UgRigidBody.mass;

        private void Awake()
        {
            UgRigidBody = GetComponent<Rigidbody>();
            UgTransform = GetComponent<Transform>();
        }

        public Vector3 Exert(UniversalGravitationBehaviour target, bool isReverse)
        {
            var p1 = this.UgTransform.position;
            var p2 = target.UgTransform.position;
            var r = Vector3.Distance(p1, p2);
            var m1 = this.Mass;
            var m2 = target.Mass;
            var d = p1 - p2;
            var v = G * (m1 * m2) / (r * r);
            if (isReverse) v *= -1;
            return v * d;
        }
    }
}