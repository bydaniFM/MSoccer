using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject myCredits;

	// Use this for initialization
	void Start () {

        myCredits.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play() {
        SceneManager.LoadScene("login2");
    }

    public void Exit() {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void ShowHideCredits() {
        if (myCredits.activeInHierarchy)
            myCredits.SetActive(false);
        else
            myCredits.SetActive(true);
    }
}