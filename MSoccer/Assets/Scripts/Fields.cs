using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fields : MonoBehaviour {

    public GameObject field;
    public GameObject player;
    public GameObject ball;

	// Use this for initialization
	void Start () {

        //Physics2D.IgnoreCollision(field.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
        //Physics2D.IgnoreCollision(field.GetComponent<Collider2D>(), ball.GetComponent<Collider2D>(), false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
