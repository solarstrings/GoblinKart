using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Engine;
using GameEngine.Source.Managers;
using GoblinKart.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class NetworkServerRecieveMessage : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            var server = NetworkManager.Instance.Server;

            NetIncomingMessage inc;

            if ((inc = server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Data(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            switch (packetType)
            {
                case PacketType.PlayerData:
                    HandleRecievePlayerData(inc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleRecievePlayerData(NetIncomingMessage inc)
        {
            var info = new NetworkInformation();
            inc.ReadAllProperties(info);

            var message = NetworkManager.Instance.Server.CreateMessage();
            message.Write((byte)PacketType.PlayerData);
            message.WriteAllProperties(info);
            NetworkManager.Instance.Send(message);
        }
    }
}
