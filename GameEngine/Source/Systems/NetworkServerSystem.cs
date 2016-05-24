﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Managers;
using GameEngine.Source.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems
{
    public class NetworkServerSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            if(NetworkManager.Instance.Server==null)
            {
                return;
            }
            var server = NetworkManager.Instance.Server;

            NetIncomingMessage inc;
            
            if ((inc = server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleConnectionStatus(inc);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        Debug.WriteLine("New connection...");
                        var data = inc.ReadByte();
                        if (data == (byte) PacketType.Login)
                        {
                            Debug.WriteLine("... Connection accepted.");
                            var player = new PlayerComponent();
                            inc.ReadAllProperties(player);
                            inc.SenderConnection.Approve();

                            var outmsg = server.CreateMessage();
                            outmsg.Write((byte) PacketType.Login);
                            outmsg.Write(true);
                            server.SendMessage(outmsg, inc.SenderConnection,
                                NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        else
                        {
                            inc.SenderConnection.Deny("Didn't send correct information");
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        // Handle debug messages
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
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // TODO implement me
        private void HandleConnectionStatus(NetIncomingMessage message)
        {
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
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    // The client has disconnected, remove him from the "connection list"?
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
