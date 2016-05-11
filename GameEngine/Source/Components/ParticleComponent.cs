using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ParticleComponent :IComponent
    {
        public Texture2D texture;
        public int MaxParticles { get; set; }
        public TimeSpan Duration = TimeSpan.FromSeconds(1);

        public float LifeDurationRandomness = 0;

        public float EmitterVelocitySensitivity = 1;
        public float MinHorizontalVelocity = 0;
        public float MaxHorizontalVelocity = 0;
        public float MinVerticalVelocity = 0;
        public float MaxVerticalVelocity = 0;
        public Vector3 Gravity = Vector3.Zero;

        //if 0, particles stop before they die
        //if 1, particles keep constant speed
        //if greater than 1, particles speed up over time
        public float EndVelocity = 1;

        public Color MinColor = Color.White;
        public Color MaxColor = Color.White;

        public float MinRotateSpeed = 0;
        public float MaxRotateSpeed = 0;

        public float MinStartSize = 100;
        public float MaxStartSize = 100;

        public float MinEndSize = 100;
        public float MaxEndSize = 100;

        public int firstActiveParticle;
        public int firstNewParticle;
        public int firstFreeParticle;
        public int firstRetiredParticle;

        // Store the current time, in seconds.
        public float currentTime;

        public Effect effect { get; set; }

        // Count how many times Draw has been called. This is used to know
        // when it is safe to retire old particles back into the free list.
        public int drawCounter;

        //list of particles
        public ParticleVertex[] particles;

        //indexbuffer
        public IndexBuffer indexBuffer { get; set; }

        //vertexbuffer
        public DynamicVertexBuffer vertexBuffer { get; set; }

        public BlendState BlendState = BlendState.NonPremultiplied;

        public ParticleComponent()
        {
            MaxParticles = 600;

            Duration = TimeSpan.FromSeconds(10);

            MinHorizontalVelocity = 0;
            MaxHorizontalVelocity = 15;

            MinVerticalVelocity = 10;
            MaxVerticalVelocity = 20;

            // Create a wind effect by tilting the gravity vector sideways.
            Gravity = new Vector3(-20, -5, 0);

            EndVelocity = 0.75f;

            MinRotateSpeed = -1;
            MaxRotateSpeed = 1;

            MinStartSize = 4;
            MaxStartSize = 7;

            MinEndSize = 35;
            MaxEndSize = 140;

            InitParticles();
        }
        
        private void InitParticles()
        {
            // Allocate the particle array, and fill in the corner fields (which never change).
            particles = new ParticleVertex[this.MaxParticles * 4];

            for (int i = 0; i < this.MaxParticles; i++)
            {
                particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                particles[i * 4 + 1].Corner = new Vector2(1, -1);
                particles[i * 4 + 2].Corner = new Vector2(1, 1);
                particles[i * 4 + 3].Corner = new Vector2(-1, 1);
            }
        }
    }
}
