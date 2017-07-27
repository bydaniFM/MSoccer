using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;

public class ClientManager : MonoBehaviour {

    private String server = "127.0.0.1";
    private int port = 8888;

    private DebugHandler myDebug;

    public static int playerNum = 0;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Ball;

    public GameObject waitingForPlayers;

    static UdpClient udpClient;
    private IPEndPoint endPoint;

    public bool connected;
    public bool playersConnected;
    public bool inGame;

    // Use this for initialization
    void Start () {

        myDebug = this.GetComponent<DebugHandler>();

        connected = false;
        playersConnected = false;
        inGame = false;

        udpClient = new UdpClient();
        udpClient.Connect(server, port);
        endPoint = new IPEndPoint(IPAddress.Any, 0);

        //StartCoroutine(StartConnection(playerNum + " Player connecting"));
        StartCoroutine(GameLoop());

    }

    IEnumerator StartConnection(string Message) {

        DebugConsole.Log("Starting game as player " + playerNum);

        waitingForPlayers.SetActive(true);
        //int count = 0;
        while (!connected) {
            //myDebug.Log(count + "Some message");
            //DebugConsole.Log(count + "Some message");
            //count++;

            send(Message);
            if(receive() == playerNum+"Player connected") {
                waitingForPlayers.SetActive(false);
                StartCoroutine(WaitForPlayer());
                //StopCoroutine("StartConnection");
                connected = true;
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator WaitForPlayer() {
        while (!playersConnected) {
            send(playerNum + "waiting");
            if(receive() == "connection established") {
                StartCoroutine(GameLoop());
                //StopCoroutine("WaitForPlayer");
                playersConnected = true;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator GameLoop() {

        inGame = true;
        string newPos = "";

        while (inGame) {

            if (playerNum == 1) {
                send(1 + Player1.transform.position.ToString());
                newPos = receive();
                if(newPos[1] == '(')
                    sendPlayer2Pos(newPos);
            } else if (playerNum == 2) {
                send(2 + Player1.transform.position.ToString());
                newPos = receive();
                if (newPos[1] == '(')
                    sendPlayer2Pos(newPos);
            }

            yield return new WaitForSeconds(.01f);
        }

    }

    public void send(string Message) {
        DebugConsole.Log("Sending: " + Message);
        Byte[] sendMessage = System.Text.Encoding.ASCII.GetBytes(Message);
        udpClient.Send(sendMessage, sendMessage.Length);
    }

    public String receive() {
        //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
        Byte[] received = udpClient.Receive(ref endPoint);
        String data = System.Text.Encoding.ASCII.GetString(received);
        DebugConsole.Log("Received: " + data);
        return data;
    }

    public void sendPlayer2Pos(String posRaw) {
        String pos = posRaw.Substring(1);
        Vector3 newPos = StringToVector3(pos);
        //if (playerNum == 1)
            newPos = new Vector3(-newPos.x, newPos.y);
        DebugConsole.Log("Setting player2 position to: " + newPos);
        Player2.transform.position = new Vector3(newPos.x, newPos.y);
    }

    void OnApplicationQuit() {
        Debug.Log("Quit");
        //aTimer.Stop();
        udpClient.Close();
    }

    public static Vector3 StringToVector3(string sVector) {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")")) {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}

/*
    IEnumerator ConnectUDP(String server, Int32 port, String Message) {
        UdpClient udpClient = new UdpClient();
        //try {
        udpClient.Connect(server, port);

        //Wait for 2 players are connected
        while (!playersConnected) {
            // Send
            Byte[] sendMessage = System.Text.Encoding.ASCII.GetBytes(Message);
            udpClient.Send(sendMessage, sendMessage.Length);
            // Receive
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] received = udpClient.Receive(ref endPoint);
            String data = System.Text.Encoding.ASCII.GetString(received);

            yield return new WaitForSeconds(.1f);

            if (data == "Connection established")
                playersConnected = true;
        }

        while (true) {

            //if(playerNum == 1) {

            //}
            
            String positions = Player1.transform.position.ToString();// + "&" + Ball.transform.position;
            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(positions);
            udpClient.Send(sendBytes, sendBytes.Length);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] received = udpClient.Receive(ref endPoint);

            String data = System.Text.Encoding.ASCII.GetString(received);

            //Debug.Log("--> " + data);
            if(data.Substring(0, 1) != "0" && data.Substring(0, 1) != "1" && data.Substring(0, 1) != "2")
                sendPlayer2Pos(data);

            yield return new WaitForSeconds(.01f);
        }

        udpClient.Close();
        //} catch (Exception e) {
        //    Debug.Log("Exception");
        //}
    }
    */

//public class Receiver {
//    UdpClient udpSocket;
//    IPEndPoint endPoint;

//    public Receiver(UdpClient udpClient) {
//        udpSocket = udpClient;
//        endPoint = new IPEndPoint(IPAddress.Any, 0);
//    }

//    public void ThreadProc() {
//        Debug.Log("Start");
//        try {
//            while (true) {
//                Byte[] received = udpSocket.Receive(ref endPoint);
//                String data = System.Text.Encoding.ASCII.GetString(received);
//                Debug.Log("--> " + data);
//            }
//        } catch (Exception e) {
//            Debug.Log("Exception");
//        }
//    }
//}

//void ConnectUDPThreaded(String server, Int32 port, String Message) {
//    try {
//        Receiver rec = new Receiver(udpClient);

//        udpClient.Connect(server, port);

//        Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(Message);
//        udpClient.Send(sendBytes, sendBytes.Length);

//        Thread udpThread = new Thread(new ThreadStart(rec.ThreadProc));

//        udpThread.Start();
//        while (!udpThread.IsAlive) ;

//        //    udpClient.Close();
//    } catch (Exception e) {
//        Debug.Log("Exception");
//    }
//}