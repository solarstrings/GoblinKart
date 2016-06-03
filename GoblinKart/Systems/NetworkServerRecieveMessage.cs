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
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleConnectionStatus(server, inc);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        HandleNewConnection(server, inc);
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        HandleDiscoveryRequest(server, inc);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        break;
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
            Debug.WriteLine("Information recieved and will be sent to all clients!");
            var info = new NetworkInformation();
            inc.ReadAllProperties(info);
            
            // TODO Fixa så att man inte skickar till den som precis gav informationen! inc.sender...
            var message = NetworkManager.Instance.Server.CreateMessage();
            message.Write((byte)PacketType.PlayerData);
            message.WriteAllProperties(info);
            NetworkManager.Instance.Send(message);
        }

        private void HandleNewConnection(NetServer server, NetIncomingMessage inc)
        {
            Debug.WriteLine("New connection...");

            if (inc.ReadByte() == (byte)PacketType.Login)
            {
                Debug.WriteLine("... Connection accepted.");
                var player = new PlayerComponent();
                inc.ReadAllProperties(player);

                inc.SenderConnection.Approve();

                var outmsg = server.CreateMessage();
                outmsg.Write((byte)PacketType.Login);
                outmsg.Write(true);
                server.SendMessage(outmsg, inc.SenderConnection,
                    NetDeliveryMethod.ReliableOrdered, 0);
            }
            else
            {
                inc.SenderConnection.Deny("Didn't send correct information");
            }
        }

        private void HandleDiscoveryRequest(NetServer server, NetIncomingMessage inc)
        {
            if (server == null) return;

            switch (inc.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:

                    // Create a response and write some example data to it
                    NetOutgoingMessage response = server.CreateMessage();
                    response.Write("Goblin Server");

                    // Send the response to the sender of the request
                    server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                    break;
            }
        }

        // TODO implement me
        private void HandleConnectionStatus(NetServer server, NetIncomingMessage message)
        {
            Debug.WriteLine("Should not happen!");
            switch (message.SenderConnection.Status)
            {
                case NetConnectionStatus.None:
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    // A new client has connected, do nothing? Should not be possible while in-game already?
                    Debug.WriteLine("New connection in status!");
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    // The client has disconnected, remove him from the "connection list"?
                    Debug.WriteLine("Lost connection in status!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
