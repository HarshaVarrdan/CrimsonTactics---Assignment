using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public bool bIsOccupied;
    public GameObject OccupiedBy;

    public bool bIsPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is to Change the Tile's occupancy Details 
    public void OccupyStatus(bool val, GameObject by = null)
    {
        bIsOccupied = val;
        if (by != null)
        {
            OccupiedBy = by;
        }
    }
}
