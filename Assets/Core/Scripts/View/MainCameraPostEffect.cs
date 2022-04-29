using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Core.Scripts.View
{
    [ExecuteInEditMode]
    public class MainCameraPostEffect : MonoBehaviour {

        [SerializeField] private Material invertMaterial;
        [SerializeField] private Material normalMaterial;
        
        private static readonly int Rate = Shader.PropertyToID("_Rate");
        private const float TransitionTime = 0.5f;

        private bool _reverseGravity = false;

        private void OnRenderImage(RenderTexture source, RenderTexture dest){
            if (_reverseGravity)
            {
                Graphics.Blit(source, dest, invertMaterial);
            }
            else
            {
                Graphics.Blit(source, dest, normalMaterial);
            }
        }

        public void SetReverseGravityDirection(bool isReverse)
        {
            _reverseGravity = isReverse;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            var t = 0f;
            var material = _reverseGravity ? invertMaterial : normalMaterial;
            while (t < TransitionTime)
            {
                t += Time.deltaTime;
                var rate = t / TransitionTime;
                material.SetFloat(Rate, Mathf.Clamp01(rate));
                yield return null;
            }
        }
    }
}