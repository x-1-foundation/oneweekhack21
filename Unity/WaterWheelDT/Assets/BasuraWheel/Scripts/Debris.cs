using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasuraWaterWheel
{
    public class Debris : MonoBehaviour
    {
        public Vector3 offsetMin;
        public Vector3 offsetMax;
        public Vector3 rotationOffsetMin;
        public Vector3 rotationOffsetMax;
        public bool isCollectable = false;
        public int poolID;
        [HideInInspector] public FollowRoute followRoute;

        private void Awake()
        {
            if(gameObject.GetComponent<FollowRoute>() == null)
                followRoute = gameObject.AddComponent<FollowRoute>();
        }
    }
}
