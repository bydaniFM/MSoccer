using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //private int player;
    //public int Player {
    //    get {
    //        return this.player;
    //    }
    //    set {
    //        this.player = value;
    //    }
    //}

    public bool debugMode;

    private ClientManager myClientManager;

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

        DebugConsole.isVisible = debugMode;

        myClientManager = this.GetComponent<ClientManager>();

        timeRunning = false;
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
        //Timer logic
        if (myClientManager.inGame) {
            timeRunning = true;
        }
        if(timer < 0) {
            Time.timeScale = 0;
            timeRunning = false;
            myEndMenu.SetActive(true);
            if(pointsP1 > pointsP2) {
                if(ClientManager.playerNum == 1)
                    winText.text = "Player 1 wins!";
                else
                    winText.text = "Player 2 wins!";
            } else if(pointsP2 > pointsP1) {
                if(ClientManager.playerNum == 1)
                    winText.text = "Player 2 wins!";
                else
                    winText.text = "Player 1 wins!";
            }else {
                winText.text = "It's a tie!";
            }
        }
    }

    /// <summary>
    /// 
    /// Updates the score and sets the ball position back to the center
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public IEnumerator UpdateScore(int player, int score) {
        if(player == 1) {
            pointsP1 = score;
        }else if(player == 2) {
            pointsP2 = score;
        }
        Debug.Log("New score: " + pointsP1 + " : " + pointsP2);
        scoreText.text = pointsP1 + " : " + pointsP2;

        yield return new WaitForSeconds(.5f);

        ballRB.transform.position = Vector2.zero;
        ballRB.velocity = Vector2.zero;
    }

    public void restartGame() {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene("menu1");
    }

    /// <summary>
    /// 
    /// Controls the display timer
    /// 
    /// </summary>
    /// <returns></returns>
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
