using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    GridSpawner GS;

    public static ObstacleManager OM_Instance;

    private void Awake()
    {
        if (OM_Instance == null)
            OM_Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GS = GridSpawner.GS_Instance;  
        //GenerateObstacles();
    }

    //This function generates Obstacles in the tiles according to the data saved in ObstacleData Scriptable Object
    public void GenerateObstacles()
    {
        for (int y = 0; y < GS.gridSizeY; y++)
        {
            for (int x = 0; x < GS.gridSizeX; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstacleGrid[index])
                {
                    TileInfo TI = GS.SelectedTileInfo(new Vector2 (x, y));
                    if (!TI.bIsOccupied)
                    {
                        Vector3 position = new Vector3(x, .5f, y + 0.15f);
                        TI.OccupyStatus(true, Instantiate(obstaclePrefab, position, Quaternion.identity));
                    }
                }
            }
        }
    }
}
