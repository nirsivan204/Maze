
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GameMGR : MonoBehaviour
{
    [SerializeField] Tilemap walls;
    [SerializeField] Tilemap gameTiles;
    [SerializeField] Tilemap playerTiles;
    [SerializeField] TileBase wallTile;
    [SerializeField] TileBase playerTile;
    [SerializeField] TileBase lavaTile;
    [SerializeField] MazeMaker maze;
    [SerializeField] int numOfWallTiles;
    [SerializeField] int numOfLavaTiles;
    [SerializeField] int numOfStars;

    private Vector3Int playerPos;
    private bool isGameOver = false;
    private int numOfSteps;
    private int numOfStarsCollected;
    public IntEvent stepsCounterUpdateEvent;
    public IntEvent starsCounterUpdateEvent;
    public BoolEvent gameOverEvent;

    private void Awake()
    {
        stepsCounterUpdateEvent = new IntEvent();
        starsCounterUpdateEvent = new IntEvent();
        gameOverEvent = new BoolEvent();
    }
    // Start is called before the first frame update
    void Start()
    {
        maze.createMaze(numOfWallTiles, numOfLavaTiles, numOfStars);
        playerPos = maze.getStartPos();
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
    }

    private void tryMove(Vector3Int movement)
    {
        Vector3Int targetLocation = playerPos + movement;
        TileTypes targetTileType = maze.GetTileType(targetLocation);
        if (targetTileType != TileTypes.Wall)
        {
            numOfSteps++;
            stepsCounterUpdateEvent.Invoke(numOfSteps);
            playerTiles.SetTile(playerPos, null);
            playerTiles.SetTile(targetLocation, playerTile);
            playerPos = targetLocation;
            switch (targetTileType)
            {
                case TileTypes.Lava:
                    isGameOver = true;
                    gameOverEvent.Invoke(false);
                    break;
                case TileTypes.Star:
                    numOfStarsCollected++;
                    starsCounterUpdateEvent.Invoke(numOfStarsCollected);
                    gameTiles.SetTile(playerPos, null);
                    break;
                case TileTypes.End:
                    isGameOver = true;
                    gameOverEvent.Invoke(true);
                    break;
            }
        }
    }

    public void OnMove(InputValue input)
    {
        if (!isGameOver)
        {
            Vector2 movement = input.Get<Vector2>();
            if(movement.magnitude != 0)
            {
                if (movement.x != 0)
                {
                    movement.y = 0;
                }
                tryMove(Vector3Int.RoundToInt(Vector3.Normalize(movement)));
            }

        }
    }




}
