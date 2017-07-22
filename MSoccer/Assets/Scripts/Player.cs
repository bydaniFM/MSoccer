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

        //if (this.transform.position.x < 0)
        //    playerNum = 1;
        //else
        //    playerNum = 2;

        inField = true;
		
	}
	
	// Update is called once per frame
	void Update () {
        playerTmp = playerNum;
	}

    private void FixedUpdate() {

        //if(playerNum == 1) {
            if(player.transform.position.x > 0) {
                inField = false;
                Debug.Log("Player " + playerNum + " invaded opponent's field");
            }
        //}
        //if (playerNum == 2) {
        //    if (player.transform.position.x < 0) {
        //        inField = false;
        //        Debug.Log("Player " + playerNum + " invaded opponent's field");
        //    }
        //}
    }

    private void OnMouseDrag() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        //if(playerNum == 1) {
            if(mousePos.x < 0) {
                inField = true;
            }
        //}
        //else if (playerNum == 2) {
        //    if (mousePos.x > 0) {
        //        inField = true;
        //    }
        //}

        if (!inField) {
            mousePos.x = 0;
        }
        player.transform.position = mousePos;
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (playerNum == 1) {
    //        if (collision.gameObject.name == "FieldRight") {
    //            inField = false;
    //        }
    //    }
    //    if (playerNum == 2) {
    //        if (collision.gameObject.name == "FieldLeft") {
    //            inField = false;
    //        }
    //    }
    //}
}
