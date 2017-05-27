using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {

        player.GetComponent<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnMouseDrag() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        player.transform.position = mousePos;//Input.mousePosition;
    }
}
