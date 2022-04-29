using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Model;
using UniRx;
using UnityEngine;

namespace Core.Scripts.View
{
    public class Player : UniversalGravitationBehaviour
    {
        [SerializeField] private Transform powerDirection;
        [SerializeField] private GameObject smoke;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject spark;
        
        public Subject<ScoreInfo> OnGameClear = new Subject<ScoreInfo>();
        public Subject<Unit> OnCollision = new Subject<Unit>();
        
        public Vector3 CenterPosition => transform.position + transform.up * 2.5f;

        [Header("VELOCITY")]
        private float _maxVelocity;
        [SerializeField] private float lowVelocity;
        [SerializeField] private float highVelocity;
        
        [Header("COLLISION")]
        private float _collisionNumber;
        [SerializeField] private int brokenNumber;
        
        [Header("DISTANCE")]
        private float _maxDistance;
        [SerializeField] private float longDistance;
        [SerializeField] private float nearDistance;
        
        [Header("GRAVITY")]
        private int _gravityFlipNumber;
        [SerializeField] private float manyFlip;
        [SerializeField] private float fewFlip;
        
        private readonly Dictionary<string, bool> _collisionPlanets = new Dictionary<string, bool>()
        {
            {"Sun", false},
            {"Mercury", false},
            {"Venus", false},
            {"Mars", false},
            {"Jupiter", false},
            {"Saturn", false},
            {"Uranus", false},
            {"Neptune", false}
        };

        private void Update()
        {
            var v = UgRigidBody.velocity.magnitude;
            if (v < _maxVelocity) return;
            _maxVelocity = v;
            // Debug.Log($"_maxVelocity: {_maxVelocity}");

            if (_maxVelocity < highVelocity) return;
            spark.SetActive(true);
        }

        public void AddForce(Vector3 force)
        {
            UgRigidBody.AddForce(force, ForceMode.Acceleration);
            powerDirection.position = CenterPosition + force.normalized * 10;
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollision.OnNext(Unit.Default);
            _collisionNumber++;
            if (_collisionNumber > brokenNumber)
            {
                smoke.SetActive(true);
            }
            // Debug.Log($"_collisionNumber: {_collisionNumber}");

            var n = other.gameObject.name;
            if (_collisionPlanets.Keys.Contains(n))
            {
                _collisionPlanets[n] = true;
                var log = "";
                foreach (var pair in _collisionPlanets)
                {
                    log += $"{pair.Key}: {pair.Value}, ";
                }
                // Debug.Log(log);
            }
            if (n.Equals("Sun"))
            {
                fire.SetActive(true);
            }
            
            if (n.Equals("Earth"))
            {
                var info = new ScoreInfo
                {
                    IsSunCollision = fire.activeSelf,
                    IsAllPlanets = _collisionPlanets.Values.All(_ => _),
                    IsNonePlanets = _collisionPlanets.Values.All(_ => !_),
                    IsManyCollisions = _collisionNumber > brokenNumber,
                    IsHighSpeed = _maxVelocity > highVelocity,
                    IsLowSpeed = _maxVelocity < lowVelocity,
                    IsManyFlips = _gravityFlipNumber > manyFlip,
                    IsFewFlips = _gravityFlipNumber < fewFlip,
                    IsLongDistance = _maxDistance > longDistance,
                    IsNearDistance = _maxDistance < nearDistance
                };
                OnGameClear.OnNext(info);
            }
        }

        public void SetMaxDistance(float d)
        {
            if (_maxDistance > d) return;
            _maxDistance = d;
        }
        
        public void SetGravityFlip()
        {
            _gravityFlipNumber++;
        }
    }
}