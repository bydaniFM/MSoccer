using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public Rigidbody2D ballRB;

    public int pointsP1;
    public int pointsP2;

	// Use this for initialization
	void Start () {

        scoreText.GetComponent<Text>();
        ballRB.GetComponent<Rigidbody2D>();

        pointsP1 = pointsP2 = 0;
        scoreText.text = pointsP1 + " : " + pointsP2;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateScore(int player, int score) {
        if(player == 1) {
            pointsP1 = score;
        }else if(player == 2) {
            pointsP2 = score;
        }
        Debug.Log("New score: " + pointsP1 + " : " + pointsP2);
        scoreText.text = pointsP1 + " : " + pointsP2;

        ballRB.transform.position = Vector2.zero;
        ballRB.velocity = Vector2.zero;
    }
}
