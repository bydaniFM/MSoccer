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

        //string strName = "user";
        //ConnectTCP("127.0.0.1", 80, "GET /userDB.php?login="+strName+" HTTP/1.1\r\nHost: localhost:80\r\n\r\n" );
        //ConnectUDP("127.0.0.1", 20000, "Hello Server");
        //ConnectUDPThreaded("127.0.0.1", 20000, "Hello Server");
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