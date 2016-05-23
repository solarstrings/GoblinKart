using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Engine;
using GameEngine.Source.Network;
using GameEngine.Source.Systems;
using Lidgren.Network;

namespace GameEngine.Source.Managers
{
    public class NetworkManager
    {
        // How do we know if someone is host or client?
        public NetServer Server { get; set; }
        public NetClient Client { get; set; }

        public bool IamAServer = false;

        private static NetworkManager _instance;
        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NetworkManager();
                return _instance;
            }
        }

        public bool InitClientConnection()
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

            // Set the managers client
            Client = client;

            return true;
        }

        private bool EstablishInfo(NetClient client)
        {
            var time = DateTime.Now;
            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds < 5)
                {
                    return false;
                }

                NetIncomingMessage inc;
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = inc.ReadByte();
                        if (data == (byte)PacketType.Login)
                        {
                            var accepted = inc.ReadBoolean();
                            return accepted;
                        }
                        else
                            return false;
                }
            }
        }

        public void InitNetworkServer(ECSEngine engine)
        {
            var config = new NetPeerConfiguration("networkGame") { Port = 9981 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            var server = new NetServer(config);
            server.Start();

            var serverEntity = EntityFactory.Instance.NewEntityWithTag("Server");

            // Set the managers server
            Server = server;

            SystemManager.Instance.RegisterSystem("Lobby", new NetworkServerSystem());
        }
    }
}
