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
            //Debug.WriteLine("Sending information to the server!");

            // Get all networkShareComponents (should be the component to entities with information necessary to send)
            var networkShareEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<NetworkShareComponent>();
            
            NetOutgoingMessage message = null;
            // msg.Write((int)MessageType.StringMessage); <---------- Detta kan vara bra att göra för att bestämma messageType, måste man kanske göra..?

            if (networkShareEntities == null) return;

            // Check for the kartComponent
            foreach (var e in networkShareEntities)
            {
                // We could implement a method in the componentmanager that gets entities depending on multiple components..?
                //var kartComponent = ComponentManager.Instance.GetEntityComponent<KartComponent>(e);

                //if (kartComponent == null) continue;

                var playerComponent = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(e);
                var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                message = NetworkManager.Instance.Client.CreateMessage();
                message.Write((byte)PacketType.PlayerData);

                message.Write(playerComponent.Id);
                message.Write(playerComponent.Name);

                message.Write(transformComponent.Position.X);
                message.Write(transformComponent.Position.Y);
                message.Write(transformComponent.Position.Z);

                message.Write(transformComponent.Forward.X);
                message.Write(transformComponent.Forward.Y);
                message.Write(transformComponent.Forward.Z);

                message.Write(transformComponent.Velocity.X);
                message.Write(transformComponent.Velocity.Y);
                message.Write(transformComponent.Velocity.Z);




                //NetworkInformation info = new NetworkInformation()
                //{
                //    Id = playerComponent.Id,
                //    Name = playerComponent.Name,
                //    Position = transformComponent.Position,
                //    Forward = transformComponent.Forward,
                //    Velocity = transformComponent.Velocity
                //};
                //message = NetworkManager.Instance.Client.CreateMessage();
                //message.Write((byte)PacketType.PlayerData);              
                //message.WriteAllProperties(info);

                //Debug.WriteLine(info.Id + info.Name);
                //Debug.WriteLine(info.Forward);
                //Debug.WriteLine(info.Position);
                //Debug.WriteLine(info.Velocity);

            }

            // Maybe check for other things to send...?
            // this feels like kinda bad idea to handle this..?

            // Send message
            NetworkManager.Instance.ClientSend(message);
        }

    }
}
