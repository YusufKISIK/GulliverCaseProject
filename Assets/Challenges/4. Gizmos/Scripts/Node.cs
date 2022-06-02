using System.Collections.Generic;
using UnityEngine;

namespace Challenges._5._Gizmos.Scripts
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private List<Node> childrenNodes;
        //Edit below

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            for (int i = 0; i < this.childrenNodes.Count; i++)
            {
                Vector3 childrenPosition = childrenNodes[i].transform.position;
                Vector3 position = Vector3.Lerp(transform.position, childrenPosition, 1f);

                Gizmos.DrawLine(transform.position, childrenPosition);
            }
        }
    }
}
