using System;
using System.Collections.Generic;
using System.Threading;
using Challenges._2._ModifiedSnake.Scripts.Abstract;
using Challenges._2._ModifiedSnake.Scripts.Blocks;
using Challenges._2._ModifiedSnake.Scripts.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class TunelGenerator : ITunelGenerator, IGameSystem , ISnakeMovementListener
{
    /// <summary>
    /// Periodically spawns a Tunnels on the Map
    /// </summary>
    private readonly SnakeGameData _snakeGameData;
    private readonly Tunel.TunelBlockPull _tunelBlockPull;
    private readonly TunelEnd.TunelEndBlockPull _tunelEndBlockPull;
    private readonly IMap _map;
    private readonly ISnakeBodyController _snakeBodyController;
    private readonly SnakeHeadBlock _snakeHeadBlock;
    private readonly IOccupancyHandler _occupancyHandler;
    private CancellationTokenSource _cts;
    private bool _running = false;
    private Dictionary<Vector2Int,Tunel> _spawnedBlock;
    private Dictionary<Vector2Int,TunelEnd> _spawnedEndBlock;
    private Vector2Int endBlockPosition;
    
    public TunelGenerator(SnakeGameData snakeGameData,ISnakeBodyController snakeBodyController, SnakeHeadBlock snakeHeadBlock,Tunel.TunelBlockPull tunelBlockPull,TunelEnd.TunelEndBlockPull tunnelEndPull, IMap map,IOccupancyHandler occupancyHandler)
    {
        _snakeGameData = snakeGameData;
        _snakeBodyController = snakeBodyController;
        _tunelBlockPull = tunelBlockPull;
        _tunelEndBlockPull = tunnelEndPull;
        _snakeHeadBlock = snakeHeadBlock;
        _map = map;
        _occupancyHandler = occupancyHandler;
        _spawnedBlock = new Dictionary<Vector2Int,Tunel>();
        _spawnedEndBlock = new Dictionary<Vector2Int,TunelEnd>();
    }

    private void SpawnTunelIfPossible(Vector2Int randomPosition)
    {
        if (_occupancyHandler.GetOccupancy(randomPosition) == OccupancyType.None)
        {
            var block = _tunelBlockPull.Spawn(randomPosition);
            _spawnedBlock.Add(randomPosition,block);
            _occupancyHandler.SetOccupied(randomPosition, OccupancyType.Tunel);
        }
    }
    private void SpawnTunnelEndIfPossible(Vector2Int randomEndPosition)
    {
        if (_occupancyHandler.GetOccupancy(randomEndPosition) == OccupancyType.None)
        {
            var blockEnd = _tunelEndBlockPull.Spawn(randomEndPosition);
            _spawnedEndBlock.Add(randomEndPosition,blockEnd);
            _occupancyHandler.SetOccupied(randomEndPosition, OccupancyType.TunelEnd);
        }
    }
    private void ClearTunnel()
    {
        foreach (var block in _spawnedBlock)
        {
            _tunelBlockPull.Despawn(block.Value);
        }
        _spawnedBlock.Clear();
    }
    public void SpawnTunnelAtStart()
    {
        if (_running) return;
        _running = true;
        var randomPosition = _map.GetRandomCoordinate();
        SpawnTunelIfPossible(randomPosition);
        Debug.Log("Tunel Has been Spawned.......................");
    }
    
    public void SpawnTunnelEnd()
    {
        var randomEndPosition = _map.GetRandomCoordinate();
        SpawnTunnelEndIfPossible(randomEndPosition);
        endBlockPosition = randomEndPosition;
        Debug.Log("Tunel End has been Spawned.........AAAAAAA.........");
    }
    
    public void StopTunnels()
    {
        if (!_running) return;
        _running = false;
        _cts.Cancel();
        _cts = null;
    }

    public void StartSystem()
    {
        SpawnTunnelAtStart();
        SpawnTunnelEnd();
    }

    public void StopSystem()
    {
        StopTunnels();
    }

    public void ClearSystem()
    {
        ClearTunnel();
    }

    public void BeforeSnakeMove(Vector2Int currentPosition, Vector2Int targetPosition)
    {
        var blockExists = _spawnedBlock.ContainsKey(targetPosition);
        if (blockExists)
        {
            _snakeHeadBlock.SnakeTeleport(endBlockPosition);
      
        }
    }

    public void AfterSnakeMove(Vector2Int previousPosition, Vector2Int currentPosition)
    {
        
    }
}
