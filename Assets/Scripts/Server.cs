using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameServer;
using UnityEngine;

namespace GameServer
{
    public class Server : MonoBehaviour
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public delegate void PacketHandler(int fromClient, Packet packet);

        public static Dictionary<int, PacketHandler> PacketHandlers;

        private static TcpListener _tcpListener;

        void Start()
        {
            Port = 26950;
            MaxPlayers = 10;
            StartServer();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartServer()
        {
            Debug.Log("Starting server...");
            InitializeServerData();

            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

            Console.WriteLine($"Server started on {Port}.");
        }

        private void TcpConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = _tcpListener.EndAcceptTcpClient(_result);
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

            Debug.Log($"Incoming conection from {_client.Client.RemoteEndPoint}...");

            for (int i = 0; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect : Server full! ");
        }

        private static void InitializeServerData()
        {
            for (int i = 0; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived,ServerHandle.WelcomeReceived}
            };
            Debug.Log("Initialized packets...");
        }
    }

}
