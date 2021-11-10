using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace BasuraWaterWheel
{
    public class DebrisSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] routes;
        [SerializeField] private Debris[] debrisPrefabs;
        [SerializeField] private float interval = 1f;

        private ObjectPool<Debris>[] _objectPools;
        private float[] _weights;
        private readonly Dictionary<int, int> _collectedDebris = new Dictionary<int, int>();
        private float _period = 0.0f;
        private bool _initialized = false;

        
        
        void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.background = Texture2D.grayTexture;

            if(!_initialized) return;
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(guiStyle,GUILayout.Width(160));
            GUILayout.Label("Spawn interval: " + Mathf.Round(interval*200)/200 +"s");
            interval = GUILayout.HorizontalSlider(interval, .01f, 4f);
            GUILayout.Space(10);
            
            GUILayout.Label("Spawn chance");
            for (int i = 0; i < _weights.Length; i++)
            {
                GUILayout.Label( debrisPrefabs[i].name + ": " + Mathf.RoundToInt(_weights[i]/_weights.Sum() * 100) + "%");
                _weights[i] = GUILayout.HorizontalSlider( _weights[i], 0.0F, 1.0F);
                
            }

            GUILayout.EndVertical();
            GUILayout.Space(20);
            GUILayout.BeginVertical(guiStyle);
            GUILayout.Label("Collected Junk:");
            
            String s = "";
            foreach (var junkPart in _collectedDebris)
            {
                s += debrisPrefabs[junkPart.Key].name + ": " + _collectedDebris[junkPart.Key] + "\n";
            }

            GUILayout.Label(s);
            
            if (GUILayout.Button("Reset"))
            {
                _collectedDebris.Clear();
                for (int i = 0; i < debrisPrefabs.Length; i++)
                {
                    if (debrisPrefabs[i].isCollectable)
                        _collectedDebris.Add(i, 0);
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        
        
        
        private void Awake()
        {
            _weights = new float[debrisPrefabs.Length];
            for (int i = 0; i < debrisPrefabs.Length; i++)
            {
                //Set inital weights
                _weights[i] = 1f;
                
                //init collectedJunkList
                if(debrisPrefabs[i].isCollectable)
                    _collectedDebris.Add(i, 0);
            }
        }

        private void Start()
        {
            //Set up objectPools
            _objectPools = new ObjectPool<Debris>[debrisPrefabs.Length];
            for (int i = 0; i < debrisPrefabs.Length; i++)
            {
                var index = i;
                
                _objectPools[i] = new ObjectPool<Debris>(
                    () => CreateDebris(index),
                    debris => debris.gameObject.SetActive(true),
                    debris => debris.gameObject.SetActive(false),
                    debris => Destroy(debris.gameObject),
                    false,
                    50,
                    50
                );
            }
            _initialized = true;
        }

        void Update()
        {
            if (_period > interval)
            {
                if(_weights.Sum() > 0)
                    SpawnDebris();
                _period = 0;
            }
            _period += Time.deltaTime;
        }

        private Debris CreateDebris(int prefabIndex)
        {
            var debris = Instantiate(debrisPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);

            debris.poolID = prefabIndex;
            debris.followRoute.SetRoute(routes);

            return debris;
        }
        
        private void SpawnDebris()
        {
            int id = WeightedRandomExtension.GetRandomWeightedID(_weights);
            var debris = _objectPools[id].Get();
            
            debris.gameObject.transform.Rotate(new Vector3(
                Random.Range(debris.rotationOffsetMin.x, debris.rotationOffsetMax.x),
                Random.Range(debris.rotationOffsetMin.y, debris.rotationOffsetMax.y),
                Random.Range(debris.rotationOffsetMin.z, debris.rotationOffsetMax.z)
            ));

            debris.followRoute.SetOffset(new Vector3(
                Random.Range(debris.offsetMin.x, debris.offsetMax.x),
                Random.Range(debris.offsetMin.y, debris.offsetMax.y),
                Random.Range(debris.offsetMin.z, debris.offsetMax.z)
            ));
            debris.followRoute.OnRoutesFinished += OnDebrisCollected;
        }

        private void OnDebrisCollected(GameObject go)
        {
            var debris = go.GetComponent<Debris>();
            debris.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            debris.followRoute.OnRoutesFinished -= OnDebrisCollected;
            _objectPools[debris.poolID].Release(debris);
            _collectedDebris[debris.poolID]++;
        }
    }
}