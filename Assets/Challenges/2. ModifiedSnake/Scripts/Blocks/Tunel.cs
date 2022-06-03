using Challenges._2._ModifiedSnake.Scripts.Abstract;
using Challenges._2._ModifiedSnake.Scripts.Data;
using UnityEngine;
using Zenject;


public class Tunel : MonoBehaviour
{
    public class TunelBlockPull : MonoMemoryPool<Vector2Int, Tunel>
    {
        protected override void OnSpawned(Tunel item)
        {
            base.OnSpawned(item);
                
        }
        protected override void OnDespawned(Tunel item)
        {
            base.OnDespawned(item);
            item._occupancyHandler.ClearOccupancy(item._position);
        }

        protected override void Reinitialize(Vector2Int p1, Tunel item)
        {
            base.Reinitialize(p1, item);
            item._position = p1;
            item.transform.position = item._map.ToWorldPosition(p1);
            item._occupancyHandler.SetOccupied(item._position,OccupancyType.Tunel);
        }
    }
        
    [Inject]
    protected readonly IOccupancyHandler _occupancyHandler;
    [Inject]
    protected readonly IMap _map;
    protected Vector2Int _position;


    public Vector2Int Position => _position;
        
}

