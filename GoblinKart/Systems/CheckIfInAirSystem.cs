﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Source.Components;
using GameEngine.Systems;
using GoblinKart.Components;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class CheckIfInAirSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            //var kartEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<KartComponent>();

            //foreach (var e in kartEntities)
            //{
            //        SetIfInAir(e);
            //}

        }

        public void SetIfInAir(Entity e)
        {
            var physicsComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(e);
            var terComp = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(e);
            var trsComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);
            var kartComp = ComponentManager.Instance.GetEntityComponent<KartComponent>(e);

            var distanceToGround =
                -(TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.Position.X, Math.Abs(trsComp.Position.Z)) -
                  trsComp.Position.Y);

            if (distanceToGround <= kartComp.KartGroundOffset) 
            {
                LockModelToHeight(terComp, trsComp, kartComp.KartGroundOffset);
                trsComp.Velocity.Y = 0;
                physicsComp.InAir = false;
                return;
            }
            else if (distanceToGround > 10f)
            {
                physicsComp.InAir = true;
            }
            else
            {
                physicsComp.InAir = false;
            }
        }
        public void LockModelToHeight(TerrainMapComponent terComp, TransformComponent trsComp, float offset)
        {
            
            trsComp.Position= new Vector3(trsComp.Position.X, offset + TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.Position.X, Math.Abs(trsComp.Position.Z)), trsComp.Position.Z);
        }
    }
}
