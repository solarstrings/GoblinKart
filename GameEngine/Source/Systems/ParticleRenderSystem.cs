using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ParticleRenderSystem : IRender3DSystem
    {
        static EffectParameter effectTimeParameter;
        static Random random = new Random();

        static EffectParameter effectViewParameter;
        static EffectParameter effectProjectionParameter;
        static EffectParameter effectViewportScaleParameter;

        GraphicsDevice device;

        public ParticleRenderSystem(GraphicsDevice device)
        {
            this.device = device;
        }

        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            List<Entity> particleEnts = ComponentManager.Instance.GetAllEntitiesWithComponentType<SmokeParticleComponent>();
            Entity cameraEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraEnt);

            foreach (Entity e in particleEnts)
            {
                SmokeParticleComponent pc = ComponentManager.Instance.GetEntityComponent<SmokeParticleComponent>(e);
                

                //// Restore the vertex buffer if context is lost
                //if (pc.vertexBuffer.IsContentLost)
                //{
                //    pc.vertexBuffer.SetData(pc.particles);
                //}

                // If there are any particles waiting in the newly added queue,
                // we'd better upload them to the GPU ready for drawing.
                if (pc.firstNewParticle != pc.firstFreeParticle)
                {
                    AddNewParticlesToVertexBuffer(pc);
                }

                // If there are any active particles, draw them now!
                if (pc.firstActiveParticle != pc.firstFreeParticle)
                {
                    graphicsDevice.BlendState = pc.BlendState;
                    graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;


                    // Set an effect parameter describing the current time. All the vertex
                    // shader particle animation is keyed off this value.
                    effectTimeParameter.SetValue(pc.currentTime);

                    // Set the particle vertex and index buffer.
                    graphicsDevice.SetVertexBuffer(pc.vertexBuffer);
                    graphicsDevice.Indices = pc.indexBuffer;

                    effectViewParameter.SetValue(c.viewMatrix);
                    effectProjectionParameter.SetValue(c.projectionMatrix);
                    effectViewportScaleParameter.SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f));

                    // Activate the particle effect.
                    foreach (EffectPass pass in pc.effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        if (pc.firstActiveParticle < pc.firstFreeParticle)
                        {
                            // If the active particles are all in one consecutive range,
                            // we can draw them all in a single call.
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         pc.firstActiveParticle * 4, (pc.firstFreeParticle - pc.firstActiveParticle) * 4,
                                                         pc.firstActiveParticle * 6, (pc.firstFreeParticle - pc.firstActiveParticle) * 2);
                        }
                        else
                        {
                            // If the active particle range wraps past the end of the queue
                            // back to the start, we must split them over two draw calls.
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         pc.firstActiveParticle * 4, (pc.MaxParticles - pc.firstActiveParticle) * 4,
                                                         pc.firstActiveParticle * 6, (pc.MaxParticles - pc.firstActiveParticle) * 2);

                            if (pc.firstFreeParticle > 0)
                            {
                                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                             0, pc.firstFreeParticle * 4,
                                                             0, pc.firstFreeParticle * 2);
                            }
                        }
                    }

                    // Reset some of the renderstates that we changed,
                    // so as not to mess up any other subsequent drawing.
                    graphicsDevice.DepthStencilState = DepthStencilState.Default;
                }

                pc.drawCounter++;
            }
        }

        void AddNewParticlesToVertexBuffer(SmokeParticleComponent particleComp)
        {
            int stride = ParticleVertex.SizeInBytes;

            if (particleComp.firstNewParticle < particleComp.firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                particleComp.vertexBuffer.SetData(particleComp.firstNewParticle * stride * 4, particleComp.particles,
                                     particleComp.firstNewParticle * 4,
                                     (particleComp.firstFreeParticle - particleComp.firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                particleComp.vertexBuffer.SetData(particleComp.firstNewParticle * stride * 4, particleComp.particles,
                                     particleComp.firstNewParticle * 4,
                                     (particleComp.MaxParticles - particleComp.firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (particleComp.firstFreeParticle > 0)
                {
                    particleComp.vertexBuffer.SetData(0, particleComp.particles,
                                         0, particleComp.firstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }
            // Move the particles we just uploaded from the new to the active queue.
            particleComp.firstNewParticle = particleComp.firstFreeParticle;
        }

        public static void AddParticle(SmokeParticleComponent particleComp,Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = particleComp.firstFreeParticle + 1;

            if (nextFreeParticle >= particleComp.MaxParticles)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == particleComp.firstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= particleComp.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(particleComp.MinHorizontalVelocity,
                                                       particleComp.MaxHorizontalVelocity,
                                                       (float)random.NextDouble());

            double horizontalAngle = random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some random amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(particleComp.MinVerticalVelocity,
                                          particleComp.MaxVerticalVelocity,
                                          (float)random.NextDouble());

            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255));

            // Fill in the particle vertex structure.
            for (int i = 0; i < 4; i++)
            {
                particleComp.particles[particleComp.firstFreeParticle * 4 + i].Position = position;
                particleComp.particles[particleComp.firstFreeParticle * 4 + i].Velocity = velocity;
                particleComp.particles[particleComp.firstFreeParticle * 4 + i].Random = randomValues;
                particleComp.particles[particleComp.firstFreeParticle * 4 + i].Time = particleComp.currentTime;
            }

            particleComp.firstFreeParticle = nextFreeParticle;
        }

        public static void setParticleOffsetPosition(ref SmokeParticleComponent particleComp, Vector3 positionOffset)
        {
            particleComp.positionOffset = positionOffset;
        }

        public static void LoadParticleEffect(GraphicsDevice device, Effect effect, Texture2D particleTexture, ref SmokeParticleComponent particleComp)
        {

            // If we have several particle systems, the content manager will return
            // a single shared effect instance to them all. But we want to preconfigure
            // the effect with parameters that are specific to this particular
            // particle system. By cloning the effect, we prevent one particle system
            // from stomping over the parameter settings of another.

            particleComp.effect = effect.Clone();

            EffectParameterCollection parameters = particleComp.effect.Parameters;

            effectViewParameter = parameters["View"];
            effectProjectionParameter = parameters["Projection"];
            effectViewportScaleParameter = parameters["ViewportScale"];

            // Look up shortcuts for parameters that change every frame.
            effectTimeParameter = parameters["CurrentTime"];

            // Set the values of parameters that do not change.
            parameters["Duration"].SetValue((float)particleComp.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(particleComp.LifeDurationRandomness);
            parameters["Gravity"].SetValue(particleComp.Gravity);
            parameters["EndVelocity"].SetValue(particleComp.EndVelocity);
            parameters["MinColor"].SetValue(particleComp.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(particleComp.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(particleComp.MinRotateSpeed, particleComp.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(particleComp.MinStartSize, particleComp.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(particleComp.MinEndSize, particleComp.MaxEndSize));
            parameters["Texture"].SetValue(particleTexture);

            // Create a dynamic vertex buffer.
            particleComp.vertexBuffer = new DynamicVertexBuffer(device, ParticleVertex.VertexDeclaration,
                                                   particleComp.MaxParticles * 4, BufferUsage.WriteOnly);
            // Create and populate the index buffer.
            ushort[] indices = new ushort[particleComp.MaxParticles * 6];

            for (int i = 0; i < particleComp.MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            particleComp.indexBuffer = new IndexBuffer(device, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            particleComp.indexBuffer.SetData(indices);

        }
    }
}
