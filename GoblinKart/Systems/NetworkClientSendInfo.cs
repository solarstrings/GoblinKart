using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using GoblinKart.Components;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class NetworkClientSendInfo : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            // Get all networkComponents (should be the component to entities with information necessary to send)
            var networkEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<NetworkComponent>();

            var client = NetworkManager.Instance.Client;

            
            NetOutgoingMessage message = null;
            // msg.Write((int)MessageType.StringMessage); <---------- Detta kan vara bra att göra för att bestämma messageType, måste man kanske göra..?

            // Check for the kartComponent
            foreach (var e in networkEntities)
            {
                // We could implement a method in the componentmanager that gets entities depending on multiple components..?
                var kartComponent = ComponentManager.Instance.GetEntityComponent<KartComponent>(e);

                if (kartComponent == null) continue;

                var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                message = client.CreateMessage();
                message.Write(transformComponent.Position.ToString());
                message.Write(transformComponent.Rotation.ToString());
                message.Write(transformComponent.Scale.ToString());
            }

            // Maybe check for other things to send...?
            // this feels like kinda bad idea to handle this..?

            // Send message
            Send(client, message);
        }

        public void Send(NetClient client, NetOutgoingMessage message)
        {
            client.SendMessage(message, client.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
