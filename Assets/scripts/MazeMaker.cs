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

    private Vector3Int startPos;
    private Vector3Int endPos;
    private int MAX_TRY_COUNTER = 100;
    public Vector3Int getStartPos()
    {
        return startPos;
    }

    public Vector3Int getEndPos()
    {
        return endPos;
    }

    private int MIN_HORIZONTAL_BORDER = -5, MAX_HORIZONTAL_BORDER = 5, MIN_VERTICAL_BORDER = -5, MAX_VERTICAL_BORDER = 5; // assuming min is negative

    private Vector3Int getRandomValidPos()
    {
        Vector3Int res;
        do
        {
            res = new Vector3Int(Random.Range(MIN_HORIZONTAL_BORDER, MAX_HORIZONTAL_BORDER), Random.Range(MIN_VERTICAL_BORDER, MAX_VERTICAL_BORDER), 0);
        } while (gameTiles.HasTile(res) || wallsAndLava.HasTile(res) || playerTiles.HasTile(res));
        return res;
    }


    private bool areVectorsEqual(Vector3Int posA, Vector3Int posB)
    {
        return posA.x == posB.x && posA.y == posB.y && posA.z == posB.z;
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

    private bool isThereAWay(Vector3Int posA, Vector3Int posB)
    {
        var queue = new Queue<Vector3Int>();
        bool[,] visitMatrix = new bool[MAX_HORIZONTAL_BORDER - MIN_HORIZONTAL_BORDER - 1, MAX_VERTICAL_BORDER - MIN_VERTICAL_BORDER - 1];
        markVisited(visitMatrix, posA);
        queue.Enqueue(posA);
        while (queue.Count > 0)
        {
            Vector3Int point = queue.Dequeue();
            if (areVectorsEqual(point, posB))
            {
                return true;
            }
            foreach (Vector3Int pos in findAllLegalNaigbours(point, wallsAndLava))
            {
                if (!visitMatrix[pos.x - MIN_HORIZONTAL_BORDER, pos.y - MIN_VERTICAL_BORDER])
                {
                    markVisited(visitMatrix, pos);
                    queue.Enqueue(pos);
                }
            }

        }
        return false;
    }

    private List<Vector3Int> findAllLegalNaigbours(Vector3Int pos, Tilemap walls)
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
    private void markVisited(bool[,] visitMatrix, Vector3Int pos)
    {
        visitMatrix[pos.x - MIN_HORIZONTAL_BORDER, pos.y - MIN_VERTICAL_BORDER] = true;
    }


    public void createMaze(int numOfWalls, int numOfLavaTiles, int numOfStars)
    {
        startPos = getRandomValidPos();
        playerTiles.SetTile(startPos, playerTile);
        endPos = getRandomValidPos();
        gameTiles.SetTile(endPos, endTile);
        if(!placeObjects(wallTile, wallsAndLava, numOfWalls,false))
        {
            print("can not add more walls");
            return;
        }
        if (!placeObjects(lavaTile, wallsAndLava, numOfLavaTiles, false))
        {
            print("can not add more lava");
            return;
        }
        if (!placeObjects(starTile, gameTiles , numOfStars,true))
        {
            print("can not add more stars");
            return;
        }
    }
private bool placeObjects(TileBase tile, Tilemap tileMap,int numOfObjects, bool isCollectable)
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
            Vector3Int targetPos = getRandomValidPos();
            if (tryPlace(targetPos, tileMap, tile, isCollectable))
            {
                objectsPlaced++;
            }
        }
        return true;
    }

    private bool tryPlace(Vector3Int pos, Tilemap board, TileBase tile, bool isCollectable)
    {
        board.SetTile(pos, tile);
        if (isCollectable)
        {
            if (isThereAWay(startPos, pos))
            {
                return true;
            }
        }
        else
        {
            if (isThereAWay(startPos, endPos))
            {
                return true;
            }
        }
        board.SetTile(pos, null);
        return false;
    }
}
