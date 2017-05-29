using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play() {
        SceneManager.LoadScene("scene1");
    }

    public void Exit() {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}