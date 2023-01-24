using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeMaker: MonoBehaviour
{
    [SerializeField] Tilemap wallsAndLava;
    [SerializeField] Tilemap gameTiles;
    [SerializeField] Tilemap playerTiles;
    [SerializeField] TileBase endTile;
    [SerializeField] TileBase wallTile;
    [SerializeField] TileBase lavaTile;
    [SerializeField] TileBase playerTile;
    [SerializeField] TileBase starTile;

    public Vector3Int startPos { get; private set; }
    public Vector3Int endPos { get; private set; }

    private int MAX_TRY_COUNTER = 100;

    private int MIN_HORIZONTAL_BORDER = -5, MAX_HORIZONTAL_BORDER = 5, MIN_VERTICAL_BORDER = -5, MAX_VERTICAL_BORDER = 5; // assuming min is negative


    private Vector3Int GetRandomValidPos()
    {
        Vector3Int res;
        do
        {
            res = new Vector3Int(Random.Range(MIN_HORIZONTAL_BORDER, MAX_HORIZONTAL_BORDER), Random.Range(MIN_VERTICAL_BORDER, MAX_VERTICAL_BORDER), 0);
        } while (gameTiles.HasTile(res) || wallsAndLava.HasTile(res) || playerTiles.HasTile(res));
        return res;
    }

    public TileTypes GetTileType(Vector3Int pos)
    {
        if(pos == endPos)
        {
            return TileTypes.End;
        }
        if(wallsAndLava.GetTile(pos) == lavaTile)
        {
            return TileTypes.Lava;
        }
        if (wallsAndLava.GetTile(pos) == wallTile)
        {
            return TileTypes.Wall;
        }
        if (gameTiles.GetTile(pos) == starTile)
        {
            return TileTypes.Star;
        }
        return TileTypes.Empty;
    }

    private bool IsThereAWay(Vector3Int posA, Vector3Int posB)
    {
        var queue = new Queue<Vector3Int>();
        bool[,] visitMatrix = new bool[MAX_HORIZONTAL_BORDER - MIN_HORIZONTAL_BORDER - 1, MAX_VERTICAL_BORDER - MIN_VERTICAL_BORDER - 1];
        MarkVisited(visitMatrix, posA);
        queue.Enqueue(posA);
        while (queue.Count > 0)
        {
            Vector3Int point = queue.Dequeue();
            if (point.Equals(posB))
            {
                return true;
            }
            foreach (Vector3Int pos in FindAllLegalNaigbours(point, wallsAndLava))
            {
                if (!visitMatrix[pos.x - MIN_HORIZONTAL_BORDER, pos.y - MIN_VERTICAL_BORDER])
                {
                    MarkVisited(visitMatrix, pos);
                    queue.Enqueue(pos);
                }
            }

        }
        return false;
    }

    private List<Vector3Int> FindAllLegalNaigbours(Vector3Int pos, Tilemap walls)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        Vector3Int naighbor = new Vector3Int(pos.x + 1, pos.y, 0);
        if (!walls.HasTile(naighbor))
        {
            res.Add(naighbor);
        }
        naighbor = new Vector3Int(pos.x - 1, pos.y, 0);
        if (!walls.HasTile(naighbor))
        {
            res.Add(naighbor);
        }
        naighbor = new Vector3Int(pos.x, pos.y + 1, 0);
        if (!walls.HasTile(naighbor))
        {
            res.Add(naighbor);
        }
        naighbor = new Vector3Int(pos.x, pos.y - 1, 0);
        if (!walls.HasTile(naighbor))
        {
            res.Add(naighbor);
        }
        return res;
    }
    private void MarkVisited(bool[,] visitMatrix, Vector3Int pos)
    {
        visitMatrix[pos.x - MIN_HORIZONTAL_BORDER, pos.y - MIN_VERTICAL_BORDER] = true;
    }


    public void CreateMaze(int numOfWalls, int numOfLavaTiles, int numOfStars)
    {
        startPos = GetRandomValidPos();
        playerTiles.SetTile(startPos, playerTile);
        endPos = GetRandomValidPos();
        gameTiles.SetTile(endPos, endTile);
        if(!PlaceObjects(wallTile, wallsAndLava, numOfWalls,false))
        {
            print("can not add more walls");
            return;
        }
        if (!PlaceObjects(lavaTile, wallsAndLava, numOfLavaTiles, false))
        {
            print("can not add more lava");
            return;
        }
        if (!PlaceObjects(starTile, gameTiles , numOfStars,true))
        {
            print("can not add more stars");
            return;
        }
    }
private bool PlaceObjects(TileBase tile, Tilemap tileMap,int numOfObjects, bool isCollectable)
    {
        int tryCounter = 0;
        int objectsPlaced = 0;
        while (objectsPlaced < numOfObjects)
        {
            if (tryCounter > MAX_TRY_COUNTER)
            {
                return false;
            }
            tryCounter++;
            Vector3Int targetPos = GetRandomValidPos();
            if (TryPlace(targetPos, tileMap, tile, isCollectable))
            {
                objectsPlaced++;
            }
        }
        return true;
    }

    private bool TryPlace(Vector3Int pos, Tilemap board, TileBase tile, bool isCollectable)
    {
        board.SetTile(pos, tile);
        if (isCollectable)
        {
            if (IsThereAWay(startPos, pos))
            {
                return true;
            }
        }
        else
        {
            if (IsThereAWay(startPos, endPos))
            {
                return true;
            }
        }
        board.SetTile(pos, null);
        return false;
    }
}
