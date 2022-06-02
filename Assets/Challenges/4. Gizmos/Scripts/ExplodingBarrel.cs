using System;
using UnityEditor;
using UnityEngine;

namespace Challenges._5._Gizmos.Scripts
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField]
        private ExplodingBarrelData explodingBarrelData;
        //Edit below

        private GUIStyle texts;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, explodingBarrelData.ExplosionRadius);
            Handles.Label(transform.position + Vector3.up, explodingBarrelData.Damage.ToString());
            Handles.Label(transform.position, explodingBarrelData.DamageType.ToString());
        }
    }
}
