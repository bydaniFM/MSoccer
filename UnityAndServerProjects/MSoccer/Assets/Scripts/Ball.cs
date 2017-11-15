using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public GameObject ball1;
    public GameObject ball2;
    public GameManager gameManager;

    private bool cooldown = true;

	// Use this for initialization
	void Start () {

        rb.GetComponent<Rigidbody2D>();
        gameManager.GetComponent<GameManager>();

        // Choose one of the two balls depending on the player (ball 1 has physics)
        if(ClientManager.playerNum == 1) {
            ball2.SetActive(false);
        } else {
            ball1.SetActive(false);
        }
		
	}

    private void Update() {
        // Check check for goals
        if (cooldown) {
            if (ClientManager.playerNum == 2) {
                if (ball2.transform.position.x >= 10) {
                    StartCoroutine(gameManager.UpdateScore(1, gameManager.pointsP1 + 1));
                    StartCoroutine(Cooldown());
                } else if (ball2.transform.position.x <= -10) {
                    StartCoroutine(gameManager.UpdateScore(2, gameManager.pointsP2 + 1));
                    StartCoroutine(Cooldown());
                }
            }
            if (ClientManager.playerNum == 1) {
                if (ball1.transform.position.x >= 10) {
                    StartCoroutine(gameManager.UpdateScore(1, gameManager.pointsP1 + 1));
                    StartCoroutine(Cooldown());
                } else if (ball1.transform.position.x <= -10) {
                    StartCoroutine(gameManager.UpdateScore(2, gameManager.pointsP2 + 1));
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (ClientManager.playerNum == 1) {
            if (collision.gameObject.tag == "Border") {
                Debug.Log("Bounce!");
                //if (collision.gameObject.name == "BorderRight") {
                //    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                //} else if (collision.gameObject.name == "BorderLeft") {
                //    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                //} else if (collision.gameObject.name == "BorderUp") {
                //    rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                //} else if (collision.gameObject.name == "BorderRight") {
                //    rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                //}
            }
            //else if (collision.gameObject.tag == "Goal") {
            //    Debug.Log("Goal!!");
            //    if (collision.gameObject.name == "GoalRight") {
            //        gameManager.UpdateScore(1, gameManager.pointsP1 + 1);
            //    } else if (collision.gameObject.name == "GoalLeft") {
            //        gameManager.UpdateScore(2, gameManager.pointsP2 + 1);
            //    }
            //}
            else if (collision.gameObject.tag == "Player") {
                if (collision.gameObject.name == "Player1") {
                    rb.AddForce(new Vector2(100, 0));
                } else if (collision.gameObject.name == "Player2") {
                    rb.AddForce(new Vector2(-100, 0));
                }
            }
        }
    }

    // Places the ball inside the field if it eventually escapes from it
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Field") {
            rb.velocity = Vector2.zero;
            rb.transform.position = Vector2.zero;
        }
    }

    IEnumerator Cooldown() {
        cooldown = false;
        yield return new WaitForSeconds(2);
        cooldown = true;
    }
}