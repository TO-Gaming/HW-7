using UnityEngine;

/**
 * This component allows the player to move by clicking the arrow keys.
 */
public class KeyboardMover : MonoBehaviour {
    public char direction;
    public Vector3 postoDel;
    protected Vector3 NewPosition() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            postoDel = transform.position+ Vector3.left;
            return transform.position + Vector3.left;
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            postoDel = transform.position + Vector3.right;
            return transform.position + Vector3.right;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            postoDel = transform.position + Vector3.down;
            return transform.position + Vector3.down;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            postoDel = transform.position + Vector3.up;
            return transform.position + Vector3.up;
        } else {
            direction = '0';

            return transform.position;
        }
    }
    public Vector3 getDelpos()
    {
        return postoDel;
    }

    void Update()  {
        transform.position = NewPosition();
    }
}
