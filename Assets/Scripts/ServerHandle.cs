using System.Collections;
using System.Collections.Generic;
using Gameplay.Character.AnimationControllers;
using UnityEngine;
using Utils;

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
        Vector3 positionInput = packet.ReadVector3();
        Vector3 vectorInput = packet.ReadVector3();
        uint tick = (uint) packet.ReadInt();
        Server.clients[fromClient].player.SetInput(positionInput,vectorInput);
        ServerSend.SendUDPDataToAll(fromClient ,packet);
    }

    public static void PlayerAnimationBool(int fromClient, Packet packet)
    {
        string animationName = packet.ReadString();
        bool boolInput = packet.ReadBool();
        var animationType = EnumExtension.GetEnumByName<AnimationNameType>(animationName);
        Server.clients[fromClient].player.animator.SetAnimationBool(animationType,boolInput);
        ServerSend.SendTCPDataToAll(fromClient ,packet);
    }
    
    public static void PlayerAnimationTrigger(int fromClient, Packet packet)
    {
        string animationName = packet.ReadString();
        
        var animationType = EnumExtension.GetEnumByName<AnimationNameType>(animationName);
        Server.clients[fromClient].player.animator.SetAnimationTrigger(animationType);
        ServerSend.SendTCPDataToAll(fromClient ,packet);
    }

    public static void ReceivePlayerRotation(int fromClient, Packet packet)
    {
        Quaternion playerRotation = packet.ReadQuaternion();
        
        Server.clients[fromClient].player.RotateCharacter(playerRotation);
        ServerSend.SendTCPDataToAll(fromClient ,packet);
    }

    public static void InvokeFirstSkill(int fromClient,Packet packet)
    {
        Vector3 position = packet.ReadVector3();
        
        Server.clients[fromClient].player.firstRemySkill.OnPress();
        ServerSend.SendTCPDataToAll(fromClient ,packet);
    }

    public static void PlayerShoot(int fromClient, Packet packet)
    {
        Vector3 _shootDirection = packet.ReadVector3();

        //Server.clients[fromClient].player.Shoot(_shootDirection);
    }

    public static void PlayerThrowItem(int fromClient, Packet packet)
    {
        Vector3 _throwDirection = packet.ReadVector3();
    }
}
