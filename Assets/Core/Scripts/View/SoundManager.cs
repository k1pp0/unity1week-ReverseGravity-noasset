using System;
using UniRx.Async;
using UnityEngine;

namespace Core.Scripts.View
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioClip next;
        [SerializeField] private AudioClip toNormal;
        [SerializeField] private AudioClip toReverse;
        [SerializeField] private AudioClip collision;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayNext()
        {
            _audioSource.PlayOneShot(next);
        }

        public void PlayGravityChange(bool isReverse)
        {
            _audioSource.PlayOneShot(isReverse ? toReverse : toNormal);
        }

        public void PlayCollision()
        {
            _audioSource.PlayOneShot(collision);
        }
    
    
    }
}
