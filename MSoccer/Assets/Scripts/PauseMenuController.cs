using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    public GameObject pauseMenu;

	// Use this for initialization
	void Start () {

        pauseMenu.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ShowHidePauseMenu();
        }
		
	}

    public void ShowHidePauseMenu() {
        if (pauseMenu.activeInHierarchy)
            pauseMenu.SetActive(false);
        else
            pauseMenu.SetActive(true);
    }

    public void Exit() {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
