using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using Lidgren.Network;

namespace GoblinKart.Init
{
    class InitNetworkServer
    {
        public InitNetworkServer(ECSEngine engine)
        {
            var config = new NetPeerConfiguration("networkGame") { Port = 9981 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            var server = new NetServer(config);
            server.Start();

            var serverEntity = EntityFactory.Instance.NewEntityWithTag("Server");
            //var serverComponent = new NetworkServerComponent() { Server = server };
            //ComponentManager.Instance.AddComponentToEntity(serverEntity, serverComponent);

            SystemManager.Instance.RegisterSystem("Lobby", new NetworkServerSystem());
        }
    }
}
