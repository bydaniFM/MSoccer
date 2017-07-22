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

    public static int playerNum = 0;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Ball;

    static UdpClient udpClient = new UdpClient();
    private static System.Timers.Timer aTimer;
    static int cnt = 0;

    // Use this for initialization
    void Start () {

        //ConnectTCP("127.0.0.1", 80, "GET /userDB.php?login="+strName+" HTTP/1.1\r\nHost: localhost:80\r\n\r\n" );
        StartCoroutine(ConnectUDP("127.0.0.1", 8888, playerNum + " Player connected"));
        //ConnectUDPThreaded("127.0.0.1", 20000, "Hello Server - And waiting...");

        aTimer = new System.Timers.Timer(60);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;

    }

    static void OnTimedEvent(System.Object source, ElapsedEventArgs e) {
        Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes("Timer" + cnt);
        udpClient.Send(sendBytes, sendBytes.Length);
        cnt++;
    }

    //IEnumerator Connect() {



    //    yield return new WaitForSeconds(.1f);
    //}

    IEnumerator ConnectUDP(String server, Int32 port, String Message) {
        UdpClient udpClient = new UdpClient();
        //try {
            udpClient.Connect(server, port);

        Byte[] sendMessage = System.Text.Encoding.ASCII.GetBytes(Message);
        udpClient.Send(sendMessage, sendMessage.Length);

        while (true) {
            String positions = Player1.transform.position.ToString();// + "&" + Ball.transform.position;
            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(positions);
            udpClient.Send(sendBytes, sendBytes.Length);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] received = udpClient.Receive(ref endPoint);

            String data = System.Text.Encoding.ASCII.GetString(received);

            //Debug.Log("--> " + data);
            if(data.Substring(0, 1) != "0")
                sendPlayer2Pos(data);

            yield return new WaitForSeconds(.01f);
        }

            udpClient.Close();
        //} catch (Exception e) {
        //    Debug.Log("Exception");
        //}
    }

    public void sendPlayer2Pos(String pos) {
        Debug.Log("Setting player2 position to: " + pos);
        Vector3 newPos = StringToVector3(pos);
        Player2.transform.position = new Vector3(-newPos.x, newPos.y);
    }

    void OnApplicationQuit() {
        Debug.Log("Quit");
        aTimer.Stop();
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