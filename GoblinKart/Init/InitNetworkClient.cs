using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Network;
using Lidgren.Network;

namespace GoblinKart.Init
{
    class InitNetworkClient
    {
        public InitNetworkClient()
        {
            // Will try to initiate a network-connection
            InitClientConnection();
        }

        private bool InitClientConnection()
        {
            var client = new NetClient(new NetPeerConfiguration("networkGame") { Port = 9981 });
            client.Start();

            var loginInformation = new LoginInformation() { Name = "ClientPlayer" };

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.Login);
            outmsg.WriteAllProperties(loginInformation);
            client.Connect("localhost", 9981, outmsg);


            if (!EstablishInfo(client)) return false;
            var clientEntity = EntityFactory.Instance.NewEntityWithTag("Client");
            var clientComponent = new NetworkClientComponent() { Client = client};
            ComponentManager.Instance.AddComponentToEntity(clientEntity, clientComponent);
            return true;

            // If the connection is successful, save it down in a component (might need it for sending information?)
        }

        private bool EstablishInfo(NetClient client)
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;
            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds < 5)
                {
                    return false;
                }

                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = inc.ReadByte();
                        if (data == (byte)PacketType.Login)
                        {
                            var accepted = inc.ReadBoolean();
                            if (accepted)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                }
            }
        }
    }
}
