using Challenges._2._ModifiedSnake.Scripts.Abstract;
using Challenges._2._ModifiedSnake.Scripts.Data;
using UnityEngine;
using Zenject;


public class TunelEnd : MonoBehaviour
{
    public class TunelEndBlockPull : MonoMemoryPool<Vector2Int, TunelEnd>
    {
        protected override void OnSpawned(TunelEnd item)
        {
            base.OnSpawned(item);
                
        }
        protected override void OnDespawned(TunelEnd item)
        {
            base.OnDespawned(item);
            item._occupancyHandler.ClearOccupancy(item._position);
        }

        protected override void Reinitialize(Vector2Int p2, TunelEnd item)
        {
            base.Reinitialize(p2, item);
            item._position = p2;
            item.transform.position = item._map.ToWorldPosition(p2);
            item._occupancyHandler.SetOccupied(item._position,OccupancyType.TunelEnd);
        }
    }
        
    [Inject]
    protected readonly IOccupancyHandler _occupancyHandler;
    [Inject]
    protected readonly IMap _map;
    protected Vector2Int _position;


    public Vector2Int Position => _position;

}
