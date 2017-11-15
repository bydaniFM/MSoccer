using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;

public class Network : MonoBehaviour {

    public string username;
    public string password;
    public Text userText;
    public Text pwdText;

    public string Response;
    public Text loginMessage;

    // Use this for initialization
    void Start() {

        userText.GetComponent<Text>();
        pwdText.GetComponent<Text>();
        loginMessage.GetComponent<Text>();
        
    }
    
    // Update is called once per frame
    void Update() {
    }

    public void SetUsername() {
        username = userText.text;
    }
    public void SetPassword() {
        password = pwdText.text;
    }

    public void SendCredentials() {
        ConnectTCP("127.0.0.1", 80, "GET /userDB.php?login=" + username + "&" + "password=" + password + " HTTP/1.1\r\nHost: localhost:80\r\n\r\n");
    }
    
    /// <summary>
    /// 
    /// Uses a .php to connect with a database with the different users. If the database is set, the .php is set and Wamp is running,
    /// you can use this credentials to log in as player1 or player2:
    /// 
    /// - Player 1:
    ///     - Username: player1
    ///     - Password: 1111
    /// - Player 2:
    ///     - Username: player2
    ///     - Password: 2222
    ///     
    /// If the credentials are correct, after logging in the game will launch as player1 or player2
    /// 
    /// </summary>
    /// <param name="server"></param>
    /// <param name="port"></param>
    /// <param name="Message"></param>
    void ConnectTCP(String server, Int32 port, String Message) {
        try {
            TcpClient client = new TcpClient(server, port);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

            data = new Byte[1024];

            //String Response = String.Empty;

            Int32 bytes = stream.Read(data, 0, data.Length);
            Response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("--> " + Response + " <--");

            Response = Response.Substring(Response.IndexOf("\r\n\r\n") + 4);
            Debug.Log("--> " + Response + " <--");

            loginMessage.text = Response;

            if(Response == "player1") {
                StartGame(1);
            }else if(Response == "player2") {
                StartGame(2);
            }

            stream.Close();
            client.Close();
        } catch (ArgumentNullException e) {
            Debug.Log("ArgumentNullException");
        } catch (SocketException e) {
            Debug.Log("SocketException");
        }
    }

    public void StartGame(int player) {
        Debug.Log("Launching game as player " + player);
        if(player == 1 || player == 2) {
            SceneManager.LoadScene("scene1");
            Player.playerNum = player;
            ClientManager.playerNum = player;
        }
    }
}