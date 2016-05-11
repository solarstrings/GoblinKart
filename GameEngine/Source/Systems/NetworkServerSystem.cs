using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems
{
    public class NetworkServerSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            var serverComponent = ComponentManager.Instance.GetAllComponentsOfType<NetworkServerComponent>()[0];

            NetIncomingMessage inc;
            
            if ((inc = serverComponent.Server.ReadMessage()) != null)
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
                        Console.WriteLine("New connection...");
                        var data = inc.ReadByte();
                        if (data == (byte) PacketType.Login)
                        {
                            Console.WriteLine("... Connection accepted.");
                            var loginInformation = new LoginInformation();
                            inc.ReadAllProperties(loginInformation);
                            inc.SenderConnection.Approve();

                            var outmsg = serverComponent.Server.CreateMessage();
                            outmsg.Write((byte) PacketType.Login);
                            outmsg.Write(true);
                            serverComponent.Server.SendMessage(outmsg, inc.SenderConnection,
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
