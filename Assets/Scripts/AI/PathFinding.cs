using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    GridNode[,] grid;
    GridNode startNode;
    GridNode endNode;
    List<GridNode> visitedNodes = new List<GridNode>();

    List<GameObject> Tiles = new List<GameObject>();

    GridSpawner GS;

    public static PathFinding PF_Instance;

    private void Awake()
    {
        if(PF_Instance == null) 
            PF_Instance = this;
        else
            Destroy(PF_Instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        GS = GridSpawner.GS_Instance;
    }

    // This function takes the starting and ending position as parameters and will find and return the shortest path using some other function.
    public List<Vector2> FindShortestPath(Vector2 sn, Vector2 en)
    {
        //Define select start and end points
        startNode = grid[Mathf.RoundToInt(sn.x), Mathf.RoundToInt(sn.y)];
        endNode = grid[Mathf.RoundToInt(en.x), Mathf.RoundToInt(en.y)];

        // Get the path
        List<GridNode> shortestPath = GetPath();
        List<Vector2> path = new List<Vector2>();
        visitedNodes = new List<GridNode>();

        if (shortestPath == null)
        {
            return null;
        }

        Debug.Log(shortestPath.Count);

        foreach(GridNode gn in shortestPath)
        {
            Vector2 pathV = new Vector2(gn.Position.x,gn.Position.z);
            Debug.Log(pathV.x + " " + pathV.y + " " + gn.Position);
            path.Add(pathV);
        }
        return path;
    }

    //this funtion finds a path to EndNode from StartNode
    List<GridNode> GetPath()
    {
        List<GridNode> path = new List<GridNode>();
        GridNode current = startNode;

        GridNode prevNode = current;

        path.Add(startNode);
        visitedNodes.Add(startNode);

        while (current.Position != endNode.Position) 
        {
            prevNode = current;
            current = NearestNeighbourNode(current);

            if(current == null)
                return null;

            if (!path.Contains(current)) { path.Add(current); visitedNodes.Add(current); }
            else { 
                current = path[path.Count - 2]; 
                path.Remove(prevNode); 
            }

        }
        //path.Add(endNode);
        visitedNodes.Add(endNode);
        return path;
    }

    //This function returns the Nearest neighbour node from current node to end node
    GridNode NearestNeighbourNode(GridNode current)
    {
        List<GridNode> neighbour = GetNeighbors(current);

        if (neighbour.Count == 0) {
            return null;
        }

        GridNode nearrestToEnd = null;

        float minDistance = -1;

        foreach (GridNode node in neighbour)
        {
            float dis = Vector3.Distance(node.Position, endNode.Position);

            if(minDistance == -1)
            {
                minDistance = dis;
                nearrestToEnd = node;
            }
            else if (dis < minDistance)
            {
                minDistance = dis;
                nearrestToEnd = node;
            }
        }

        return nearrestToEnd;
    }

    //This function returns a list of GridNode that are in the adjacent sides of the given node
    List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        int x = Mathf.RoundToInt((node.Position.x - GS.transform.position.x) / GS.nodeSpacing);
        int y = Mathf.RoundToInt((node.Position.z - GS.transform.position.z) / GS.nodeSpacing);

        if (x >= 0 && x < GS.gridSizeX && y >= 0 && y < GS.gridSizeY)
        {
            if (x > 0) neighbors.Add(grid[x - 1, y]);
            if (x < GS.gridSizeX - 1) neighbors.Add(grid[x + 1, y]);
            if (y > 0) neighbors.Add(grid[x, y - 1]);
            if (y < GS.gridSizeY - 1) neighbors.Add(grid[x, y + 1]);
        }

        neighbors.RemoveAll(node => visitedNodes.Contains(node));
        neighbors.RemoveAll(node => GS.SelectedTileInfo(new Vector2(node.Position.x,node.Position.z)).bIsOccupied);
        neighbors.RemoveAll(node => !GS.SelectedTileInfo(new Vector2(node.Position.x,node.Position.z)).bIsPath);

        if (neighbors.Contains(endNode)) //Checks if any of the available neighbour is EndNode
        {
            neighbors.Clear();
            neighbors.Add(endNode);
        }

        return neighbors;
    }

    //This functions gets the grid information from the Class GridSpawner
    public void GetTiles( GridNode[,] grid)
    {
        this.grid = grid;
        //FindShortestPath(new Vector2(0, 0), new Vector2(4, 2));
    }

    //This function is the same function as in above with a extra parameter, so this helps in finding the nearest neighbour tile to player for enemy.
    public GridNode NearestNeighbourNode(GridNode current, Vector3 fromPos)
    {
        List<GridNode> neighbour = GetNeighbors(current);

        if (neighbour.Count == 0)
        {
            return null;
        }

        GridNode nearrestToEnd = null;

        float minDistance = -1;

        foreach (GridNode node in neighbour)
        {
            float dis = Vector3.Distance(node.Position, fromPos);

            if (minDistance == -1)
            {
                minDistance = dis;
                nearrestToEnd = node;
            }
            else if (dis < minDistance)
            {
                minDistance = dis;
                nearrestToEnd = node;
            }
        }

        return nearrestToEnd;
    }

}
