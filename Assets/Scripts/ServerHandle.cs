using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class ServerHandle : MonoBehaviour
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Debug.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {fromClient}.");
            Debug.Log($"new player with nickname: {username}");
            if (fromClient != clientIdCheck)
            {
                Debug.LogError($"Player \"{username}\" (ID:{fromClient} has assumed the wrong client ID({clientIdCheck}!)");
            }
            
        }
    }
}

