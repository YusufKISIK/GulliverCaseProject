using System;
using System.Collections;
using UnityEngine;

namespace Challenges._5._Gizmos.Scripts
{
    public class ClusterBomb : MonoBehaviour
    {
        [SerializeField] private ClusterBombData clusterBombData;

        //Edit below


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, clusterBombData.ChildExplosionRadius);
            //Gizmos.DrawLine(transform.position);

        }

    }
}

