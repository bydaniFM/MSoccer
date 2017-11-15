#include<stdio.h>
#include<winsock2.h>
#include<string>

using namespace std;

#pragma comment(lib,"ws2_32.lib") //Winsock Library

#define BUFLEN 512  //Max length of buffer
#define PORT 8888   //The port on which to listen for incoming data

struct playerData {
	sockaddr_in my_si_other;
	int my_slen = sizeof(my_si_other);
} player1, player2;

/*
enum State { connecting, confirm, ingame };
State state1 = connecting;
State state2 = connecting;
*/

void sendToPlayer(SOCKET s, string str, int playNum);

int main()
{
	SOCKET s;
	struct sockaddr_in server, si_other;
	int slen, recv_len;
	char buf[BUFLEN];
	WSADATA wsa;

	slen = sizeof(si_other);

	bool player1Connected = false;
	bool player2Connected = false;
	bool firstLoop1 = true;
	bool firstLoop2 = true;

	string lastPos1 = "1(7,0,0)";
	string lastPos2 = "2(7,0,0)";
	string lastPosBall = "B(0,0,0)";

	//Initialise winsock
	printf("\nInitialising Winsock...");
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		printf("Failed. Error Code : %d", WSAGetLastError());
		exit(EXIT_FAILURE);
	}
	printf("Initialised.\n");

	//Create a socket
	if ((s = socket(AF_INET, SOCK_DGRAM, 0)) == INVALID_SOCKET)
	{
		printf("Could not create socket : %d", WSAGetLastError());
	}
	printf("Socket created.\n");

	//Prepare the sockaddr_in structure
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(PORT);

	//Bind
	if (bind(s, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR)
	{
		printf("Bind failed with error code : %d", WSAGetLastError());
		exit(EXIT_FAILURE);
	}
	puts("Bind done");

	//keep listening for data
	while (1)
	{
		printf("\nWaiting for data...\n");
		fflush(stdout);

		//clear the buffer by filling null, it might have previously received data
		memset(buf, '\0', BUFLEN);

		//try to receive some data, this is a blocking call
		if ((recv_len = recvfrom(s, buf, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == SOCKET_ERROR)
		{
			printf("recvfrom() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}

		//print details of the client and the data received
		printf("Received packet from %s:%d\n", inet_ntoa(si_other.sin_addr), ntohs(si_other.sin_port));
		printf("Data: %s\n", buf);

		std::string str = "";
		str = buf;

		//	Reply logic. Depends on the two first chars of the buffer and the state of the game.
		if (buf[0] == '1') {
			if (buf[1] == 'B') {
				if (!firstLoop1) {
					lastPosBall = str;
				} else {
					firstLoop1 = false;
				}
				str = "Ball sent";
			} else {
				if (!firstLoop1) {
					lastPos1 = str;
				} else {
					player1Connected = true;
					player1.my_si_other = si_other;
					player1.my_slen = slen;
				}
				str = lastPos2;
			}
			sendToPlayer(s, str, 1);
		}
		if (buf[0] == '2') {
			if (buf[1] == 'B') {
				if (!firstLoop2) {
				} else {
					firstLoop2 = false;
				}
				str = lastPosBall;
			} else {
				if (!firstLoop2) {
					lastPos2 = str;
				} else {
					player2Connected = true;
					player2.my_si_other = si_other;
					player2.my_slen = slen;
				}
				str = lastPos1;
			}
			sendToPlayer(s, str, 2);
		}
		
	}

	closesocket(s);
	WSACleanup();

	return 0;
}

// Converts the string into char[] and sends it to the player
void sendToPlayer(SOCKET s, string str, int playNum) {
	char buf[BUFLEN];
	memset(buf, '\0', BUFLEN);
	strcpy(buf, str.c_str());
	int recv_len = str.length();

	printf("Sending to %i: %s\n", playNum, buf);

	if (playNum == 1) {
		if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &player1.my_si_other, player1.my_slen) == SOCKET_ERROR)
		{
			printf("sendto() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}
	}
	if (playNum == 2) {
		if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &player2.my_si_other, player2.my_slen) == SOCKET_ERROR)
		{
			printf("sendto() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}
	}
}



/*	LEGACY



if (buf[1] == 'B') {
if (!firstLoop1) {
lastPosBall = str;
} else {
firstLoop1 = false;
}
str = lastPosBall;
sendToPlayer(s, str, 2);
}*/

/*
if (buf[0] == '1') {
switch (state1)
{
case connecting:

if (!player1Connected) {
player1Connected = true;
player1.my_si_other = si_other;
player1.my_slen = slen;
str = "1Player connected";
}
state1 = confirm;

break;
case confirm:

if (!player2Connected) {
str = "no players found";
} else {
confirmed1 = true;
str = "connection established";
}

break;
case ingame:

if (!firstLoop1)
lastPos1 = str;
else
firstLoop1 = false;

str = lastPos2;

break;
default:
break;
}
sendToPlayer(s, str, 1);

}
if (buf[0] == '2') {
switch (state2)
{
case connecting:

if (!player2Connected) {
player2Connected = true;
player2.my_si_other = si_other;
player2.my_slen = slen;
str = "2Player connected";
}
state2 = confirm;

break;
case confirm:

if (!player1Connected) {
str = "no players found";
} else {
confirmed2 = true;
str = "connection established";
}

break;
case ingame:

if (!firstLoop2)
lastPos2 = str;
else
firstLoop2 = false;

str = lastPos1;

break;
default:
break;
}
sendToPlayer(s, str, 2);
}

if (confirmed1 && confirmed2) {
state1 = ingame;
state2 = ingame;
}*/

/*
if (!player1Connected || !player2Connected) {
if (buf[0] == '1') {
player1Connected = true;
player1.my_si_other = si_other;
player1.my_slen = slen;
if (!player2Connected) {
str = "1Player connected";
//strcpy(buf, str.c_str());
sendToPlayer(s, str, 1);
}
} else if (buf[0] == '2') {
player2Connected = true;
player2.my_si_other = si_other;
player2.my_slen = slen;
if (!player1Connected) {
str = "2Player connected";
//strcpy(buf, str.c_str());
sendToPlayer(s, str, 2);
}
}
if (player1Connected && player2Connected) {
str = "connection established";
if (buf[0] == 1) {
confirmed1 = true;
sendToPlayer(s, str, 1);
} else if (buf[0] == 2) {
confirmed2 = true;
sendToPlayer(s, str, 2);
}
}
}

if (!confirmed1 || !confirmed2) {

}

else if ((player1Connected && player2Connected) && !inGame) {
str = "connection established";
if (confirmed1) {
sendToPlayer(s, str, 2);
} else {
sendToPlayer(s, str, 1);
}
} else if (inGame) {

if (buf[0] == 1) {

if (!firstLoop1)
lastPos1 = str;
else
firstLoop1 = false;

sendToPlayer(s, lastPos2, 1);

} else if (buf[0] == 2) {

if (!firstLoop2)
lastPos2 = str;
else
firstLoop2 = false;

sendToPlayer(s, lastPos1, 2);

}

}*/
//printf("%s\n", buf);

//now reply the client with the same data
/*if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &si_other, slen) == SOCKET_ERROR)
{
printf("sendto() failed with error code : %d", WSAGetLastError());
exit(EXIT_FAILURE);
}*/