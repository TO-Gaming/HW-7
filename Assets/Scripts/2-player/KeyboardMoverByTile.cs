using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] TileBase grass = null;

    public float downTime, upTime, pressTime = 0;
    public float countDown = 0.3f;
    public bool ready = false;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    

    void Update()  {
        
        Vector3 newPosition = NewPosition();
        Vector3Int p = tilemap.WorldToCell(newPosition);
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        Vector3Int ans = tilemap.LocalToCell(getDelpos());

        if (Input.GetKeyDown(KeyCode.Space) && ready == false)
        {
            downTime = Time.time;
            pressTime = downTime + countDown;
            ready = true;
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ready = false;
        }
        if (Time.time >= pressTime && ready == true)
        {
            ready = false;
            Debug.Log("yourPos : " + transform.position + " ans: " + ans);
            tilemap.SetTile(ans, grass);
        }

        if (allowedTiles.Contain(tileOnNewPosition)) {
            transform.position = newPosition;
        } else {
            Debug.Log("new position: " + newPosition);
            Debug.Log("player: " + transform.position);
            Debug.Log("p: " + p);
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
        
        
        
            
        
        }

    
}
