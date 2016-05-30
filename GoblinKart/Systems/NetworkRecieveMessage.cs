using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Managers;
using GoblinKart.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class NetworkRecieveMessage : IUpdateSystem
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

        public void Data(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            switch (packetType)
            {
                case PacketType.PlayerPosition:
                    HandleRecievedPositionData(inc);
                    break;
                case PacketType.Login:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleRecievedPositionData(NetIncomingMessage inc)
        {
            Debug.WriteLine("New position recieved...");

            var nm = NetworkManager.Instance;
            
            // Read the information
            var info = new NetworkInformation();
            inc.ReadAllProperties(info);

            // Use the information to update needed network-stuff

            // If you are the host, send the position to all the others except the sender.
            if (nm.IamAServer)
            {
                var message = nm.Client.CreateMessage();
                message.WriteAllProperties(info);
                nm.Send(message);
            }
        }
    }
}
