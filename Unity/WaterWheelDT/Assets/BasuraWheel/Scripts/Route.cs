using UnityEngine;

namespace BasuraWaterWheel
{
    public class Route : MonoBehaviour
    {
        [SerializeField] private Transform[] controlPoints;
        private Vector3 _gizmosPosition;
        [SerializeField] private float speed = 1f;
        private float _speedModifier = 1f;

        private void OnDrawGizmos()
        {
            if (GetSpeed() <= 0.01) return;

            for (float t = 0; t <= 1; t += 0.05f * GetSpeed())
            {
                _gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                                  3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                                  3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                                  Mathf.Pow(t, 3) * controlPoints[3].position;
                Gizmos.DrawSphere(_gizmosPosition, 0.01f);
            }

            Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
            Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);
        }

        public float GetSpeed()
        {
            return speed * _speedModifier;
        }

        public void SetSpeed(float speedModifier)
        {
            _speedModifier = speedModifier;
        }
    }
}