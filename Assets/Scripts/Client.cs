using Assets.Scripts;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace GameServer
{
    public class Client
    {
        private static int _dataBufferSize = 4096;
        public int id;
        public TCP tcp;
    
        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
        
        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
    
            private NetworkStream _stream;
            private byte[] _receiveBuffer;
            public TCP(int _id)
            {
                id = _id;
            }
    
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = _dataBufferSize;
                socket.SendBufferSize = _dataBufferSize;
    
                _stream = socket.GetStream();
    
                _receiveBuffer = new byte[_dataBufferSize];
    
                _stream.BeginRead(_receiveBuffer, 0, _dataBufferSize, ReceiveCallBack, null);

                ServerSend.Welcome(id, "Welcome to my server");
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if(socket != null)
                    {
                        _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error sending packet:{ex} to player: {id}");
                }
            }
    
            private void ReceiveCallBack(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = _stream.EndRead(_result);
    
                    if (_byteLength <= 0)
                    {
                        return;
                    }
    
                    byte[] _data = new byte[_byteLength];
                    Array.Copy(_receiveBuffer, _data, _byteLength);
    
                    _stream.BeginRead(_receiveBuffer, 0, _dataBufferSize, ReceiveCallBack, null);
                }
                catch (Exception _ex)
                {
                    Debug.Log($"Error receiving TCP data: {_ex} ");
                }
            }
        }  
    }
}


