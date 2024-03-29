﻿using Challenges._2._ModifiedSnake.Scripts.Abstract;
using UnityEngine;
using Zenject;

namespace Challenges._2._ModifiedSnake.Scripts.MonoBehaviours
{
    public class GameVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Transform ground;
        [SerializeField]
        private Transform mapLinePrefab;

        [Inject]
        private IMap _map;

        
        private Transform Spawn()
        {
            var ins = Instantiate(mapLinePrefab, transform);
            return ins;
        }
        private void Start()
        {
            ground.localScale = new Vector3(_map.MapSize.x+1f,1f,_map.MapSize.y+1f);
            if (_map == null) return;
            var center = (_map.ToWorldPosition(new Vector2Int(0, 0)) +
                             _map.ToWorldPosition(new Vector2Int(_map.MapSize.x - 1, _map.MapSize.y - 1))) / 2f;
            var mapSizeX = _map.MapSize.x;
            var mapSizeY = _map.MapSize.y;
            for (int x = 0; x < _map.MapSize.x+1; x++)
            {
                var line = Spawn();
                line.rotation = Quaternion.Euler(0, 0, 0);
                var worldPos = _map.ToWorldPosition(new Vector2Int(x, 0));
                worldPos.z = center.z;
                worldPos.x -= 0.5f;
                line.position = worldPos;
                line.localScale = new Vector3(0.05f, 0.05f, mapSizeY);
            }
            for (int y = 0; y < _map.MapSize.y+1; y++)
            {
                var line = Spawn();
                line.rotation = Quaternion.Euler(0, 90, 0);
                var worldPos = _map.ToWorldPosition(new Vector2Int(0, y));
                worldPos.x = center.x;
                worldPos.z -= 0.5f;
                line.position = worldPos;
                line.localScale = new Vector3(0.05f, 0.05f, mapSizeX);
            }
        }

     
    }
}
