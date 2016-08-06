using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Interfaces;
using GameEngine.Components;
using GameEngine.Engine.Source.Engine;
using GameEngine.Managers;
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

            if (server == null) return;

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
            var message = NetworkManager.Instance.Server.CreateMessage();
            message.Write((byte)PacketType.PlayerData);

            message.Write(inc.ReadInt32());
            message.Write(inc.ReadString());

            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());

            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());

            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());

            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());
            message.Write(inc.ReadFloat());

            //message.Write(inc.ReadFloat());
            //message.Write(inc.ReadFloat());
            //message.Write(inc.ReadFloat());

            NetworkManager.Instance.ServerSend(message, inc.SenderConnection);

            //.WriteLine(message.);
        }

        private void HandleNewConnection(NetServer server, NetIncomingMessage inc)
        {
            var nm = NetworkManager.Instance;
            
            Debug.WriteLine("New connection...");

            if (inc.ReadByte() == (byte)PacketType.Login)
            {
                Debug.WriteLine("... Connection accepted.");
                var player = new PlayerComponent();
                inc.ReadAllProperties(player);

                // TODO Maybe change this
                player.Id = nm.Players.Count+1;
                nm.Players.Add(player);

                // Debug.WriteLine(player.Id + player.Name);

                inc.SenderConnection.Approve();

                // Need to sleep, else it will send these things below too fast...
                System.Threading.Thread.Sleep(1000);

                // Send back accepted connection (true) and the player id
                var outmsg = server.CreateMessage();
                outmsg.Write((byte)PacketType.Login);              
                outmsg.Write(nm.Players.Count);
                server.SendMessage(outmsg, inc.SenderConnection,
                    NetDeliveryMethod.ReliableOrdered, 0);

                // Has to be bigger than 1, else you're the only player!
                if (nm.Players.Count > 1)
                {
                    // Send data from new connection to all the other players
                    SendNewPlayerToAllPlayers(inc, player);

                    // Send all the other players to the new connection                                                    
                    SendOldPlayersToTheNewPlayer(inc, player);

                    Debug.WriteLine("InitNetworkInfo sent for all!");
                }                
            }
            else
            {
                inc.SenderConnection.Deny("Didn't send correct information");
            }
        }

        private void SendNewPlayerToAllPlayers(NetIncomingMessage inc, PlayerComponent player)
        {
            var outmsg = NetworkManager.Instance.Server.CreateMessage();
            outmsg.Write((byte)PacketType.InitNetworkInformation);

            outmsg.Write(1);
            outmsg.WriteAllProperties(player);
            NetworkManager.Instance.ServerSend(outmsg, inc.SenderConnection);
        }

        private void SendOldPlayersToTheNewPlayer(NetIncomingMessage inc, PlayerComponent player)
        {
            var nm = NetworkManager.Instance;

            var outmsg = NetworkManager.Instance.Server.CreateMessage();
            outmsg.Write((byte)PacketType.InitNetworkInformation);

            outmsg.Write(nm.Players.Count-1);
            
            foreach (var p in nm.Players)
            {
                if (p.Id != player.Id)
                    outmsg.WriteAllProperties(p);
            }
            nm.Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
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
            // Debug.WriteLine("Should not happen!");
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
