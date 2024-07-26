using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    PathFinding PF;
    [SerializeField]UnityEvent OnPlayerMoved;

    bool bCanMove;
    internal bool bIsMoving;


    // Start is called before the first frame update
    void Start()
    {
        PF = PathFinding.PF_Instance;
        foreach(GameObject gb in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Debug.Log(gb.name);
            OnPlayerMoved.AddListener(gb.GetComponent<EnemyController>().TriggerMovement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Here When bCanMove is true and player clicks the right mouse button, then the player finds his way to the tile that the player clicked on
        if (bCanMove)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        if (hit.collider.GetComponent<TileInfo>().bIsOccupied)
                        {
                            Debug.Log("Can't Move to that Tile as its Occupies");
                        }
                        else
                        {
                            Vector3 target = hit.transform.position;
                            Debug.Log("Dest : " + hit.transform.name + " From : " + transform.position);
                            List<Vector2> pathVec =  new List<Vector2>();
                            pathVec = PF.FindShortestPath(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z)); 
                            if (pathVec.Count > 0) 
                            { 
                                StartCoroutine(MoveToDest(pathVec));
                            }
                        }
                    }
                }
            }
        }
    }

    //This is a Coroutine which handles the player movement from his current position to the destination given as a list<vector2> in parameter
    IEnumerator MoveToDest(List<Vector2> path)
    {
        bCanMove = false;
        GridSpawner.GS_Instance.SelectedTileInfo(new Vector2(path[0].x, path[0].y)).OccupyStatus(false);
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPos = new Vector3(path[i].x, transform.position.y, path[i].y);
            Debug.Log("Current Dest " + path[i]);

            // Move towards the target position until close enough
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                Debug.Log(Vector3.Distance(transform.position, targetPos));
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed* Time.deltaTime);
                yield return null; // Wait for the next frame
            }
            transform.position = targetPos;
        }
        GridSpawner.GS_Instance.SelectedTileInfo(new Vector2(path[path.Count -1].x, path[path.Count - 1].y)).OccupyStatus(true,gameObject);
        bCanMove = true;
        OnPlayerMoved?.Invoke();
        Debug.Log("Movement completed.");
    }


    //This function changes the value of bCanMove.
    public void DecideCanMove(bool val)
    {
        bCanMove = val;
    }
}
