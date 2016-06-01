using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Network;
using GameEngine.Source.Systems;
using Lidgren.Network;


namespace GameEngine.Source.Managers
{
    /// <summary>
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    public sealed class NetworkManager
    {
        private static readonly NetworkManager _instance = new NetworkManager();
        // How do we know if someone is host or client?
        public NetServer Server { get; set; }
        public NetClient Client { get; set; }

        public bool IamAServer = false;


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static NetworkManager() { }

        private NetworkManager() { }

        
        public static NetworkManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool InitClientConnection()
        {
            var client = new NetClient(new NetPeerConfiguration("networkGame") { Port = 9981 });
            client.Start();

            var player = new PlayerComponent { Name = "ClientPlayer" };

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.Login);
            outmsg.WriteAllProperties(player);
            client.Connect("localhost", 9981, outmsg);

            if (!EstablishInfo(client))
            {
                Debug.WriteLine("Connection to the host failed!");
                return false;
            }
            var clientEntity = EntityFactory.Instance.NewEntityWithTag("Client");

            // Set the managers client
            Client = client;
            Debug.WriteLine("You have successfully connected to the host!");

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

        public void InitNetworkServer()
        {
            var config = new NetPeerConfiguration("networkGame") { Port = 9981 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            var server = new NetServer(config);
            server.Start();

            // Set the managers server
            Server = server;
            IamAServer = true;
        }

        public void Send(NetOutgoingMessage message)
        {
            Client.SendMessage(message, Client.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
