using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasuraWaterWheel
{
    public class AnimationSpeed : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private float animationSpeed = 1f;
        private float _speedModifier = 1f;

        void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        public void SetSpeed(float speedModifier)
        {
            _speedModifier = speedModifier;
            _animator.speed = animationSpeed * _speedModifier;
        }
    }
}