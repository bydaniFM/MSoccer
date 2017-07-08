using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public Rigidbody2D ballRB;

    public Text timerText;
    public bool timeRunning;
    public int matchTime;
    public int timer;

    public int pointsP1;
    public int pointsP2;

    public GameObject myEndMenu;
    public Text winText;

	// Use this for initialization
	void Start () {

        Time.timeScale = 1;

        timeRunning = true;
        timer = matchTime;
        StartCoroutine(Timer());

        scoreText.GetComponent<Text>();
        timerText.GetComponent<Text>();
        ballRB.GetComponent<Rigidbody2D>();

        pointsP1 = pointsP2 = 0;
        scoreText.text = pointsP1 + " : " + pointsP2;

		
	}
	
	// Update is called once per frame
	void Update () {
        if(timer < 0) {
            Time.timeScale = 0;
            timeRunning = false;
            myEndMenu.SetActive(true);
            if(pointsP1 > pointsP2) {
                winText.text = "Player 1 wins!";
            }else if(pointsP2 > pointsP1) {
                winText.text = "Player 1 wins!";
            }else {
                winText.text = "It's a tie!";
            }
        }
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

    public void restartGame() {
        Debug.Log("Restarting Match...");
        SceneManager.LoadScene("scene1");
    }

    IEnumerator Timer() {
        if (timeRunning) {
            int minutes = timer / 60;
            int seconds = timer - minutes * 60;
            string message = minutes + ":" + seconds;

            if (minutes < 10 && seconds >= 10) {
                message = "0" + minutes + ":" + seconds;
            }
            if (seconds < 10 && minutes >= 10) {
                message = minutes + ":" + "0" + seconds;
            }
            if (minutes < 10 && seconds < 10) {
                message = "0" + minutes + ":" + "0" + seconds;
            }

            timerText.text = message;
            timer--;
        }
        yield return new WaitForSeconds(1);

        StartCoroutine(Timer());
    }
}
