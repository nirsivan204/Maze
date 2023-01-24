
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GameMGR : MonoBehaviour
{
    [SerializeField] Tilemap gameTiles;
    [SerializeField] Tilemap playerTiles;
    [SerializeField] TileBase playerTile;
    [SerializeField] MazeMaker maze;
    [SerializeField] int numOfWallTiles;
    [SerializeField] int numOfLavaTiles;
    [SerializeField] int numOfStars;

    private Vector3Int _playerPos;
    private bool _isGameOver = false;
    private int _numOfSteps;
    private int _numOfStarsCollected;
    public static Action<int> stepsCounterUpdateEvent;
    public static Action<int> starsCounterUpdateEvent;
    public static Action<bool> gameOverEvent;

    void Start()
    {
        maze.CreateMaze(numOfWallTiles, numOfLavaTiles, numOfStars);
        _playerPos = maze.startPos;
    }

    private void OnEnable()
    {
        UIMGR.RestartEvent += Restart;
    }

    private void OnDisable()
    {
        UIMGR.RestartEvent -= Restart;
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void TryMove(Vector3Int movement)
    {
        Vector3Int targetLocation = _playerPos + movement;
        TileTypes targetTileType = maze.GetTileType(targetLocation);
        if (targetTileType != TileTypes.Wall)
        {
            _numOfSteps++;
            stepsCounterUpdateEvent?.Invoke(_numOfSteps);
            playerTiles.SetTile(_playerPos, null);
            playerTiles.SetTile(targetLocation, playerTile);
            _playerPos = targetLocation;
            switch (targetTileType)
            {
                case TileTypes.Lava:
                    _isGameOver = true;
                    gameOverEvent?.Invoke(false);
                    break;
                case TileTypes.Star:
                    _numOfStarsCollected++;
                    starsCounterUpdateEvent?.Invoke(_numOfStarsCollected);
                    gameTiles.SetTile(_playerPos, null);
                    break;
                case TileTypes.End:
                    _isGameOver = true;
                    gameOverEvent?.Invoke(true);
                    break;
            }
        }
    }

    public void OnMove(InputValue input)
    {
        if (!_isGameOver)
        {
            Vector2 movement = input.Get<Vector2>();
            if(movement.magnitude != 0)
            {
                if (movement.x != 0)
                {
                    movement.y = 0;
                }
                TryMove(Vector3Int.RoundToInt(Vector3.Normalize(movement)));
            }

        }
    }




}
