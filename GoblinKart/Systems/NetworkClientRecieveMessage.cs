﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Components;
using GameEngine.Source.Engine;
using GameEngine.Source.Managers;
using GoblinKart.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GoblinKart.Systems
{
    public class NetworkClientRecieveMessage : IUpdateSystem
    {
        private ECSEngine _engine;
        private GameTime _gameTime;

        public NetworkClientRecieveMessage(ECSEngine engine)
        {
            _engine = engine;
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            var server = NetworkManager.Instance.Server;

            NetIncomingMessage inc;

            if ((inc = server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Data(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            switch (packetType)
            {
                case PacketType.PlayerData:
                    HandleRecievedPlayerData(inc);
                    break;
                case PacketType.Login:

                    break;
                case PacketType.InitNetworkInformation:
                    HandleInitNetworkInformation(inc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleInitNetworkInformation(NetIncomingMessage inc)
        {
            var info = new InitNetworkInformation();
            inc.ReadAllProperties(info);

            var cm = ComponentManager.Instance;

            foreach (var player in info.Players)
            {
                var e = EntityFactory.Instance.NewEntity();

                cm.AddComponentToEntity(e, player);
                cm.AddComponentToEntity(e, new TransformComponent());

                var modelComp = new ModelComponent(_engine.LoadContent<Model>("chopper"), true, false, false)
                {
                    staticModel = false
                };

                ModelRenderSystem.AddMeshTransform(ref modelComp, 1, Matrix.CreateRotationY(0.2f));
                ModelRenderSystem.AddMeshTransform(ref modelComp, 3, Matrix.CreateRotationY(0.5f));
                cm.AddComponentToEntity(e, modelComp);

                SceneManager.Instance.AddEntityToSceneOnLayer("Game", 3, e);
            }

        }

        private void HandleRecievedPlayerData(NetIncomingMessage inc)
        {
            Debug.WriteLine("New position recieved...");

            var nm = NetworkManager.Instance;
            
            // Read the information
            var info = new NetworkInformation();
            inc.ReadAllProperties(info);

            // Use the information to update needed network-stuff
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();

            // TODO Maybe implement me smarter!
            foreach (var e in entities)
            {
                PlayerComponent p = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(e);

                if (p.Id == info.Id)
                {
                    TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                    var difference = Vector3.Distance(info.Position, t.Position);
                    
                    // TODO add threshold constant
                    if (difference < 20)
                    {
                        t.Position = info.Position;
                        t.Forward = info.Forward;
                    }                
                    else
                    {
                        // TODO FIX INTERPOLATIONSHASTIGHET
                        // TODO FIX ROTATION-INTERPOLATION
                        var diff = new Vector3(info.Position.X - t.Position.X, info.Position.Y - t.Position.Y,
                            info.Position.Z - t.Position.Z);
                        t.Position += diff *(float) _gameTime.ElapsedGameTime.TotalSeconds*1;
                    }
                    break;
                }

            }
        }
    }
}