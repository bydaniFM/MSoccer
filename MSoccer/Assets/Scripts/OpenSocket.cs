//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Net.Sockets;
//using System.Net;
//using System.Threading;

//// Client

//public class OpenSocket : MonoBehaviour {

//    UdpClient udpClient = new UdpClient();

//    // Use this for initialization
//    void Start() {
//        //ConnectTCP("127.0.0.1", 80, "GET /hello.php HTTP/1.1\r\nHost: localhost:80\r\n\r\n");
//        ConnectUDP("127.0.0.1", 20000, "Hello Server");
//    }

//    // Update is called once per frame
//    void Update() {

//    }

//    void OnApplicationQuit() {
//        udpClient.Close();
//    }

//    void ConnectTCP(String server, Int32 port, String Message) {

//        try {

//            TcpClient client = new TcpClient(server, port);

//            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Message);

//            NetworkStream stream = client.GetStream();

//            stream.Write(data, 0, data.Length);

//            data = new Byte[1024];

//            String Response = String.Empty;

//            Int32 bytes = stream.Read(data, 0, data.Length);
//            Response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
//            Debug.Log("--> " + Response + " <--");

//            Response = Response.Substring(Response.IndexOf("\r\n\r\n") + 4);
//            Debug.Log("--> " + Response + " <--");

//            stream.Close();
//            client.Close();

//        } catch (ArgumentNullException e) {
//            Debug.Log("ArgumentNullException");
//        } catch (SocketException e) {
//            Debug.Log("SocketException");
//        }
//    }

//    void ConnectUDP(String server, Int32 port, String Message) {

//        UdpClient udpClient = new UdpClient();

//        try {

//            udpClient.Connect(server, port);

//            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(Message);
//            udpClient.Send(sendBytes, sendBytes.Length);

//            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
//            Byte[] received = udpClient.Receive(ref endPoint);  //ref is like a pointer

//            String data = System.Text.Encoding.ASCII.GetString(received);
//            Debug.Log("--> " + data + " <--");

//            udpClient.Close();

//        } catch (Exception e) {
//            Debug.Log("Exception");
//        }
//    }

//    public class Receiver {
//        UdpClient udpSocket;
//        IPEndPoint endPoint;

//        public Receiver(UdpClient udpClient) {
//            udpSocket = udpClient;
//            endPoint = new IPEndPoint(IPAddress.Any, 0);
//        }
//        public void ThreadProc() {
//            Debug.Log("Start");
//            try {
//                while (true) {
//                    Byte[] received = udpSocket.Receive(ref endPoint);  //ref is like a pointer
//                    String data = System.Text.Encoding.ASCII.GetString(received);
//                    Debug.Log("--> " + data + " <--");
//                }
//            } catch (Exception e) {
//                Debug.Log("Exception");
//            }
//        }
//    }

//    void ConnectUDPThreaded(String server, Int32 port, String Message) {
//        try {
//            Receiver rec = new Receiver(udpClient);

//            udpClient.Connect(server, port);

//            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(Message);
//            udpClient.Send(sendBytes, sendBytes.Length);

//            Thread udpThread = new Thread(new ThreadStart(rec.ThreadProc));
//            udpThread.Start();
//            while (!udpThread.IsAlive) ;

//        } catch (Exception e) {
//            Debug.Log("Exception");
//        }
//    }
//}


////void ConnectUDP(String serverName, Int32 port, String Message) {
////    UdpClient udpClient = new UdpClient();
////    try {
////        udpClient.Connect(serverName, port);

////        Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes("Hello Server. Love, Unity.");

////        udpClient.Send(sendBytes, sendBytes.Length);

////        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
////        Byte[] received = udpClient.Receive(ref endPoint);

////        String data = System.Text.Encoding.ASCII.GetString(received);

////        Debug.Log("udp--> " + data);

////        udpClient.Close();
////    } catch (ArgumentNullException ex) {
////        Debug.Log("ArgumentNullException" + ex);
////    } catch (SocketException ex) {
////        Debug.Log("SocketException" + ex);
////    } catch (Exception ex) {
////        Debug.Log("General Exception" + ex);
////    }
////}