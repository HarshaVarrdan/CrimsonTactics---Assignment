using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{

    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float nodeSpacing = 1f;

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject playerPrefab;

    GridNode[,] grid;
    List<TileInfo> Tiles = new List<TileInfo>();

    public static GridSpawner GS_Instance;

    private void Awake()
    {
        if (GS_Instance == null)
        {
            GS_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = GameObject.FindWithTag("Player");
        GenerateGrid();
    }


    //This function creates a grid of size gridSizeX and gridSizeY using Rectangular 2D Array (grid).
    public void GenerateGrid()
    {
        grid = new GridNode[gridSizeX, gridSizeY];

        Vector3 startingPosition = transform.position; // Get the current position of the GameObject

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodePosition = new Vector3(startingPosition.x + x * nodeSpacing, startingPosition.y, startingPosition.z + y * nodeSpacing);
                grid[x, y] = new GridNode(nodePosition);
            }
        }

        SpawnTile();
    }

    // This function spawns the tiles in the grid generated in the above function
    public void SpawnTile()
    {
        foreach (GridNode node in grid)
        {
            GameObject tileGB = Instantiate(tilePrefab, node.Position,Quaternion.identity);
            tileGB.transform.parent = transform;
            tileGB.name = $"Tile {node.Position.x} {node.Position.z}";
            Tiles.Add(tileGB.GetComponent<TileInfo>());
        }
        Debug.Log("Grid Length : " + grid.Length);
        ObstacleManager.OM_Instance.GenerateObstacles();
        PathFinding.PF_Instance.GetTiles(grid);

        playerPrefab.transform.position = new Vector3(0, 1.5f, 0);
        SelectedTileGO(grid[0,0]).GetComponent<TileInfo>().OccupyStatus(true,playerPrefab);
        playerPrefab.GetComponent<PlayerController>().DecideCanMove(true);
    }

    // This function returns TileInfo of the particular Tile by taking X and Z Postion of it.
    public TileInfo SelectedTileInfo(Vector2 pos)
    {
        foreach(var t in Tiles)
        {
            Vector3 v = t.transform.position;
            if(v.x == pos.x && v.z == pos.y)
            {
                return t;
            }
        }

        Debug.LogError($"Not Found {pos}");
        return null;
    }

    // This function returns Gameobject of the particular Tile by taking X and Z Postion of it.
    public GameObject SelectedTileGO(Vector2 pos)
    {
        foreach (var t in Tiles)
        {
            Vector3 v = t.transform.position;
            if (v.x == pos.x && v.z == pos.y)
            {
                return t.transform.gameObject;
            }
        }

        Debug.LogError($"Not Found {pos}");
        return null;
    }

    // This function returns Gameobject of the particular Tile by taking GridNode value.
    public GameObject SelectedTileGO(GridNode gn)
    {
        foreach (var t in Tiles)
        {
            Vector3 v = t.transform.position;
            if (v.x == gn.Position.x && v.z == gn.Position.y)
            {
                return t.transform.gameObject;
            }
        }

        Debug.LogError($"Not Found {gn.Position}");
        return null;
    }

    // This function returns GridNode of the particular Tile by taking X and Z Postion of it.
    public GridNode SelectedGridNode(Vector2 pos)
    {
        foreach (GridNode gn in grid)
        {
            if (gn.Position.x == pos.x && gn.Position.z == pos.y) { return gn; }
        }
        return null;
    }

}
