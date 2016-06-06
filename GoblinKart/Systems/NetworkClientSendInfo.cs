using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Managers;
using GoblinKart.Components;
using GoblinKart.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class NetworkClientSendInfo : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            Debug.WriteLine("Sending information to the server!");
            // Get all networkComponents (should be the component to entities with information necessary to send)
            var networkEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<NetworkComponent>();
            
            NetOutgoingMessage message = null;
            // msg.Write((int)MessageType.StringMessage); <---------- Detta kan vara bra att göra för att bestämma messageType, måste man kanske göra..?

            if (networkEntities == null) return;

            // Check for the kartComponent
            foreach (var e in networkEntities)
            {
                // We could implement a method in the componentmanager that gets entities depending on multiple components..?
                //var kartComponent = ComponentManager.Instance.GetEntityComponent<KartComponent>(e);

                //if (kartComponent == null) continue;

                var playerComponent = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(e);
                var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                NetworkInformation info = new NetworkInformation()
                {
                    Id = playerComponent.Id,
                    Name = playerComponent.Name,
                    Position = transformComponent.Position,
                    Forward = transformComponent.Forward,
                    Velocity = transformComponent.Velocity
                };
                message = NetworkManager.Instance.Client.CreateMessage();
                message.Write((byte)PacketType.PlayerData);              
                message.WriteAllProperties(info);
            }

            // Maybe check for other things to send...?
            // this feels like kinda bad idea to handle this..?

            // Send message
            NetworkManager.Instance.ClientSend(message);
        }

    }
}
