using System;
using UnityEditor;
using UnityEngine;

namespace Challenges._5._Gizmos.Scripts
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField]
        private Transform point1;
        [SerializeField]
        private Transform handle1;
        [SerializeField]
        private Transform point2;
        [SerializeField]
        private Transform handle2;
        //Edit below

        
        [SerializeField]
        private Texture2D _texture;

        private void OnDrawGizmos()
        {
            Handles.DrawBezier(point1.position, point2.position, handle1.position, handle2.position, Color.red,
                _texture, 0.1f);
        }
    }
}
