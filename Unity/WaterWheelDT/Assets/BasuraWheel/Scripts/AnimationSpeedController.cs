using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BasuraWaterWheel
{
    public class AnimationSpeedController : MonoBehaviour
    {
        [SerializeField] private float speedModifier = 1f;
        private AnimationSpeed[] _animationSpeeds;
        private Route[] _routeSpeeds;

        void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.background = Texture2D.grayTexture;
            
            GUILayout.BeginVertical(guiStyle, GUILayout.Height(Screen.height), GUILayout.Width(160));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Water Speed: " + speedModifier);
            speedModifier = GUILayout.HorizontalSlider(speedModifier, 0f, 4f);
            GUILayout.EndVertical();
        }
        
        void Start()
        {
            _animationSpeeds = GetComponentsInChildren<AnimationSpeed>();
            _routeSpeeds = GetComponentsInChildren<Route>();
        }

        private void Update()
        {
            foreach (var animationSpeed in _animationSpeeds)
            {
                animationSpeed.SetSpeed(speedModifier);
            }

            foreach (var routeSpeed in _routeSpeeds)
            {
                routeSpeed.SetSpeed(speedModifier);
            }
        }
    }
}