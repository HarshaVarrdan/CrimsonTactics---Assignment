using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyController : MonoBehaviour , IEnemyAI
{

    [SerializeField] float moveSpeed;

    PathFinding PF;
    GridSpawner GS;
    GameObject player;

    bool bIsMoving;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PF = PathFinding.PF_Instance;
        GS = GridSpawner.GS_Instance;
    }

    //This function triggers the enemy to move from his current location to the adjacent tile of player
    public void TriggerMovement()
    {
        if (!bIsMoving)
        {
            Debug.Log("Enemy Movement Triggerred");

            GridNode grid;
            grid = GS.SelectedGridNode(new Vector2(player.transform.position.x, player.transform.position.z));
            GridNode target = PF.NearestNeighbourNode(grid, transform.position);
            List<Vector2> path = PF.FindShortestPath(new Vector2(transform.position.x, transform.position.z), new Vector2(target.Position.x, target.Position.z));

            StartCoroutine(MoveToDest(path));
        }
    }

    // this function deals with damage
    public void Damage()
    {
        throw new System.NotImplementedException();
    }

    //This is a Coroutine which handles the player movement from his current position to the destination given as a list<vector2> in parameter
    public IEnumerator MoveToDest(List<Vector2> path)
    {
        bIsMoving = true;
        GridSpawner.GS_Instance.SelectedTileInfo(new Vector2(path[0].x, path[0].y)).OccupyStatus(false);
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPos = new Vector3(path[i].x, transform.position.y, path[i].y);
            Debug.Log("Current Dest " + path[i]);

            // Move towards the target position until close enough
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                Debug.Log(Vector3.Distance(transform.position, targetPos));
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }
            transform.position = targetPos;
        }
        GridSpawner.GS_Instance.SelectedTileInfo(new Vector2(path[path.Count - 1].x, path[path.Count - 1].y)).OccupyStatus(true, gameObject);
        Debug.Log("Movement completed.");
        bIsMoving = false;
    }
}
