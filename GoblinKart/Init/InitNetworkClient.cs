using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Engine;
using GameEngine;
using Lidgren.Network;
using GameEngine.Source.Network;

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

            return EstablishInfo(client);

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
