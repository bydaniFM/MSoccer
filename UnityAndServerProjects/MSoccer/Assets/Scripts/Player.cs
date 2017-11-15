using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject player;

    static public int playerNum = 0;
    public int playerTmp = 0;

    public bool inField;

	// Use this for initialization
	void Start () {

        player.GetComponent<GameObject>();

        inField = true;
		
	}
	
	// Update is called once per frame
	void Update () {
        playerTmp = playerNum;
	}

    private void FixedUpdate() {
        // Check player is within his field
        if(player.transform.position.x > 0) {
            inField = false;
            Debug.Log("Player " + playerNum + " invaded opponent's field");
        }
    }

    /// <summary>
    /// 
    /// Player movement
    /// 
    /// </summary>
    private void OnMouseDrag() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        
        if(mousePos.x < 0) {
            inField = true;
        }

        if (!inField) {
            mousePos.x = 0;
        }
        player.transform.position = mousePos;
    }
}
