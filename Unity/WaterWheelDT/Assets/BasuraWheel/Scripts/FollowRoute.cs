using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BasuraWaterWheel
{
    public class FollowRoute : MonoBehaviour
    {

        [SerializeField] private Transform[] routes;
        [SerializeField] private float initDelay;
        [SerializeField] private float initOffset = .0f;
        [SerializeField][HideInInspector] private Vector3 offset = Vector3.zero;
        [SerializeField] private bool alignWithRoute = false;
        
        private int _routeToGo = 0;
        private float _tParam = 0.0f;
        private Vector3 _objectPosition;
        private bool _coroutineAllowed = true;
        [SerializeField] private bool initiated = false;

        public UnityAction<GameObject> OnRoutesFinished;

        void Update()
        {
            if (_coroutineAllowed)
            {
                StartCoroutine(GoByTheRoute(_routeToGo));
            }
        }

        public void SetRoute(Transform[] route)
        {
            routes = route;
        }

        public void SetOffset(Vector3 _offset)
        {
            offset = _offset;
        }
        
        private IEnumerator GoByTheRoute(int routeNum)
        {
            _coroutineAllowed = false;
            
            if (!initiated)
            {
                initiated = true;
                _tParam = initOffset;
                
                if (initDelay != 0.0f)
                    yield return new WaitForSeconds(initDelay);
                
                _coroutineAllowed = true;
                yield break;
            }

            
            Vector3 p0 = routes[routeNum].GetChild(0).position + offset;
            Vector3 p1 = routes[routeNum].GetChild(0).GetChild(0).position+ offset;
            Vector3 p3 = routes[routeNum].GetChild(1).position+ offset;
            Vector3 p2 = routes[routeNum].GetChild(1).GetChild(0).position+ offset;
            
            while (_tParam < 1)
            {
                //position
                _tParam += Time.deltaTime * routes[routeNum].GetComponent<Route>().GetSpeed();
                _objectPosition = Mathf.Pow(1 - _tParam, 3) * p0 
                                  + 3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 
                                  + 3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 
                                  + Mathf.Pow(_tParam, 3) * p3;

                //rotation
                var relativeUp = transform.up;
                var direction = transform.position - _objectPosition;

                //round to avoid precision error
                var distance = Mathf.RoundToInt(Vector3.Distance(relativeUp.normalized, Vector3.up.normalized));
                
                if(Mathf.RoundToInt(distance) == 2)
                    relativeUp = Vector3.down;
                else if(Mathf.RoundToInt(distance) == 0)
                    relativeUp = Vector3.up;
                
                //set transforms
                transform.position = _objectPosition;
                
                if(direction != Vector3.zero && alignWithRoute)
                    transform.rotation = Quaternion.LookRotation(direction,relativeUp);
                
                yield return new WaitForEndOfFrame();
            }
            
            _tParam = 0f;
            _routeToGo += 1;
            
            if (_routeToGo > routes.Length - 1)
            {
                _routeToGo = 0;
                if(OnRoutesFinished != null)
                    OnRoutesFinished.Invoke(this.gameObject);
            }


            _coroutineAllowed = true;
        }
    }
}