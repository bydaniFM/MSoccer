using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugHandler : MonoBehaviour {

    public Text debugText;
    public int capacity;

    private List<string> messages;

	// Use this for initialization
	void Awake () {

        debugText.GetComponent<Text>();
        messages = new List<string>();
        messages.Add(debugText.text);
		
	}
	
	public void Log(string message) {
        Debug.Log(message);
        if (message != null) {
            message += "\n";
            debugText.text = "";
            if (messages.Count > capacity) {
                messages.RemoveAt(0);
            }
            messages.Add(message);
            //debugText.text += message;
            foreach (string msg in messages) {
                debugText.text += msg;
            }
        }
    }
}
