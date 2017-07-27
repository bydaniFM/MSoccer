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
		printf("Waiting for data...");
		fflush(stdout);

		//clear the buffer by filling null, it might have previously received data
		memset(buf, '\0', BUFLEN);

		//try to receive some data, this is a blocking call
		if ((recv_len = recvfrom(s, buf, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == SOCKET_ERROR)
		{
			printf("recvfrom() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}

		//print details of the client/peer and the data received
		printf("Received packet from %s:%d\n", inet_ntoa(si_other.sin_addr), ntohs(si_other.sin_port));
		printf("Data: %s\n", buf);

		std::string str = "";
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
				//strcpy(buf, str.c_str());
				sendToPlayer(s, str, 0);
			}
			recv_len = str.length();
		}
		//printf("%s\n", buf);

		//now reply the client with the same data
		/*if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &si_other, slen) == SOCKET_ERROR)
		{
			printf("sendto() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}*/
	}

	closesocket(s);
	WSACleanup();

	return 0;
}

// playNum -> 0 : send to both players
void sendToPlayer(SOCKET s, string str, int playNum) {
	char buf[BUFLEN];
	memset(buf, '\0', BUFLEN);
	strcpy(buf, str.c_str());
	int recv_len = str.length();

	printf("Sending to %i: %s\n", playNum, buf);

	if (playNum == 1 || playNum == 0) {
		if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &player1.my_si_other, player1.my_slen) == SOCKET_ERROR)
		{
			printf("sendto() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}
	}
	if (playNum == 2 || playNum == 0) {
		if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &player2.my_si_other, player2.my_slen) == SOCKET_ERROR)
		{
			printf("sendto() failed with error code : %d", WSAGetLastError());
			exit(EXIT_FAILURE);
		}
	}
}