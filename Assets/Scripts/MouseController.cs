using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseController : MonoBehaviour
{

    [SerializeField] TMP_Text tilePos_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Here we are Using Raycast to find the details about the tile that the mouse hovers on.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.CompareTag("Tile"))
            {
                tilePos_Text.text = "X : " +hit.transform.position.x + " Y : " + hit.transform.position.y + " Z : " + hit.transform.position.z + "\nIs Occupied : " + hit.transform.GetComponent<TileInfo>().bIsOccupied;
            }
            else
            {
                tilePos_Text.text = "";
            }
        }
    }
}
