using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class ParticleUpdateSystem : IUpdateSystem
    {

        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
            {
                throw new ArgumentNullException("gameTime");
            }

            List<Entity> particlesEnt = ComponentManager.Instance.GetAllEntitiesWithComponentType<SmokeParticleComponent>();
            if(particlesEnt == null)
            {
                return;
            }

            foreach (Entity e in particlesEnt)
            {
                SmokeParticleComponent pc = ComponentManager.Instance.GetEntityComponent<SmokeParticleComponent>(e);
                var transformEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<TransformComponent>();
                var transformEntity = ComponentManager.Instance.GetEntityWithTag("Kart", transformEntities);

                TransformComponent tc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(transformEntity);

                pc.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                RetireActiveParticles(pc);
                FreeRetiredParticles(pc);

                ParticleRenderSystem.AddParticle(pc,Vector3.Zero, Vector3.Zero);

                //get the position and rotation of the kart
                Matrix PosRotMatrix = Matrix.CreateTranslation(-tc.Position) * Matrix.CreateFromQuaternion(tc.Rotation) * Matrix.CreateTranslation(tc.Position);

                //Set the particles position to the models position, applying the position and offset with the Positin and rotation of the 
                Vector3 newPos = Vector3.Transform(tc.Position + pc.positionOffset, PosRotMatrix);
                
                // If we let our timer go on increasing for ever, it would eventually
                // run out of floating point precision, at which point the particles
                // would render incorrectly. An easy way to prevent this is to notice
                // that the time value doesn't matter when no particles are being drawn,
                // so we can reset it back to zero any time the active queue is empty.
                if (pc.firstActiveParticle == pc.firstFreeParticle)
                    pc.currentTime = 0;

                if (pc.firstRetiredParticle == pc.firstActiveParticle)
                    pc.drawCounter = 0;

                pc.effect.Parameters["position"].SetValue(newPos);

            }
        }

        void RetireActiveParticles(SmokeParticleComponent particleComp)
        {
            float particleDuration = (float)particleComp.Duration.TotalSeconds;

            while (particleComp.firstActiveParticle != particleComp.firstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = particleComp.currentTime - particleComp.particles[particleComp.firstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                particleComp.particles[particleComp.firstActiveParticle * 4].Time = particleComp.drawCounter;

                // Move the particle from the active to the retired queue.
                particleComp.firstActiveParticle++;

                if (particleComp.firstActiveParticle >= particleComp.MaxParticles)
                    particleComp.firstActiveParticle = 0;
            }
        }

        void FreeRetiredParticles(SmokeParticleComponent particleComp)
        {
            while (particleComp.firstRetiredParticle != particleComp.firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = particleComp.drawCounter - (int)particleComp.particles[particleComp.firstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                particleComp.firstRetiredParticle++;

                if (particleComp.firstRetiredParticle >= particleComp.MaxParticles)
                    particleComp.firstRetiredParticle = 0;
            }
        }
    }
}
