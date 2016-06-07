using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private double _time = 0;
        private double _updateRate = 0.04;

        public void Update(GameTime gameTime)
        {

            // Update every x second
            Debug.WriteLine(_time);
            _time += gameTime.ElapsedGameTime.TotalSeconds;

            if (_time < _updateRate)
                return;

            _time = 0;

            Debug.WriteLine("Sending information to the server!");

            // Get all networkShareComponents (should be the component to entities with information necessary to send)
            var networkShareEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<NetworkShareComponent>();
            
            if (networkShareEntities == null) return;

            NetOutgoingMessage message = null;
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

                message.Write(transformComponent.Scale.X);
                message.Write(transformComponent.Scale.Y);
                message.Write(transformComponent.Scale.Z);

                message.Write(transformComponent.Position.X);
                message.Write(transformComponent.Position.Y);
                message.Write(transformComponent.Position.Z);

                message.Write(transformComponent.Forward.X);
                message.Write(transformComponent.Forward.Y);
                message.Write(transformComponent.Forward.Z);

                //message.Write(transformComponent.Velocity.X);
                //message.Write(transformComponent.Velocity.Y);
                //message.Write(transformComponent.Velocity.Z);
            }

            // Send message
            NetworkManager.Instance.ClientSend(message);
        }

    }
}
