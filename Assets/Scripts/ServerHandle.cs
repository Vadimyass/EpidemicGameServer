using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        Vector3 inputs = packet.ReadVector3();
        Debug.Log(inputs);
        Server.clients[fromClient].player.SetInput(inputs);
    }

    public static void PlayerShoot(int fromClient, Packet packet)
    {
        Vector3 _shootDirection = packet.ReadVector3();

        Server.clients[fromClient].player.Shoot(_shootDirection);
    }

    public static void PlayerThrowItem(int fromClient, Packet packet)
    {
        Vector3 _throwDirection = packet.ReadVector3();
    }
}
