using GameServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class ServerSend
    {
        private static void SendTCPData(int toClient,Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            foreach (var client in Server.clients.Values)
            {
                client.tcp.SendData(packet);
            }
        }

        private static void SendTCPDataToAll(int exceptClient,Packet packet)
        {
            packet.WriteLength();
            foreach (var client in Server.clients.Values)
            {
                if (client.id == exceptClient) return;

                client.tcp.SendData(packet);
            }
        }

        public static void Welcome(int toClient,string message)
        {
            using(Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(message);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }
    }
}
