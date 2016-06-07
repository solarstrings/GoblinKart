using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.Source.Engine;
using Lidgren.Network;

namespace GameEngine.Managers
{
    /// <summary>
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    public sealed class NetworkManager
    {
        public NetServer Server { get; set; }
        public List<PlayerComponent> Players { get; set; } = new List<PlayerComponent>();
        public NetClient Client { get; set; }

        public bool IamAServer = false;


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static NetworkManager() { }

        private NetworkManager() { }

        
        public static NetworkManager Instance { get; } = new NetworkManager();

        public bool InitClientConnection()
        {
            var config = new NetPeerConfiguration("networkGame");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            var client = new NetClient(config);
            
            client.Start();

            var serverIp = CheckForBroadCast(client);

            if (serverIp != null)
            {
                var player = new PlayerComponent {Name = "ClientPlayer"};

                var outmsg = client.CreateMessage();
                outmsg.Write((byte) PacketType.Login);
                outmsg.WriteAllProperties(player);

                client.Connect(serverIp, outmsg);

                EstablishConnection(client);
              
                var clientEntity = EntityFactory.Instance.NewEntityWithTag("Client");

                // Set the managers client
                Client = client;
                Debug.WriteLine("You have successfully connected to the host!");

                return true;
            }

            else
            {
                // Make client handle failed connection
                return false;
            }            
        }



        public bool EstablishConnection(NetClient client)
        {
            // Se till att loopen tar slut ifall det ej blir ngn connection...
            while (true)
            {
                NetIncomingMessage inc;
                if ((inc = client.ReadMessage()) == null) continue;
                Debug.WriteLine("WAITING...");
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        switch (inc.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                // Do something! Save connection?
                                client.Connections.Add(inc.SenderConnection);
                                Debug.WriteLine("CONNECTION ACCEPTED");
                                return true;
                        }
                        break;
                }
            }
        }

        public IPEndPoint CheckForBroadCast(NetClient client)
        {
            Debug.WriteLine("Sending discovery signal");
            client.DiscoverLocalPeers(9981);

            while (true)
            {
                NetIncomingMessage inc;
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + inc.SenderEndPoint + " name: " + inc.ReadString());

                        return inc.SenderEndPoint;
                }
            }
        }

        public void InitNetworkServer()
        {
            var config = new NetPeerConfiguration("networkGame") {Port = 9981};
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            var server = new NetServer(config);
            server.Start();

            // Set the managers server
            Server = server;
            IamAServer = true;
        }

        //The second parameter is the connection that you should not send to
        public void ServerSend(NetOutgoingMessage message, NetConnection connection)
        {
           NetConnection [] connections = new NetConnection[Server.Connections.Count];

            Server.Connections.CopyTo(connections);
            var cons = connections.Where(x => x != connection).ToList();

            if (cons.Count > 0)
            {
                Server.SendMessage(message, cons, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public void ClientSend(NetOutgoingMessage message)
        {
            Client.SendMessage(message, Client.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
