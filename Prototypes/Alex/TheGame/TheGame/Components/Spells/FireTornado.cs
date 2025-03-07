﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Library;

namespace TheGame
{
    public class FireTornado : Spell
    {
        public FireTornado(GameScreen parent, SpellInfo spellInfo, Vector3 target)
            : base(parent, spellInfo)
        {
            this.target = target;
        }

        private static Random random = new Random();

        Vector3 target;

        PointSpriteSystem vortex;

        int timeElpase;

        private float scale = 1.0f;

        public override void Initialize()
        {

            base.Initialize();

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.OrangeRed;
            settings.MaxParticles = 500;
            settings.BasePosition = target;
            settings.BaseRotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleF");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 200.0f;

            vortex = new PointSpriteSystem(Parent, settings);
            Add((IMoveable)vortex);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardDevice kbd = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (kbd.IsKeyDown(Keys.Up))
            {
                Position += -Vector3.UnitZ * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Left))
            {
                Position += -Vector3.UnitX * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Down))
            {
                Position += Vector3.UnitZ * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Right))
            {
                Position += Vector3.UnitX * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }

            Position = target;

            timeElpase += gameTime.ElapsedGameTime.Milliseconds;

            if (timeElpase > 10)
            {
                timeElpase = 0;

                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                float theta = (float)MathHelper.Pi * 2.0f;
                float r = 0.2f;

                for (int i = 0; i < particlesToMake; i++)
                {
                    vortex.AddParticle(new Vector3(r * (float)random.NextDouble() * scale, theta * (float)random.NextDouble(), 0.0f), new Vector3(0.3f, 2.0f, 3.0f), 0.8f, 1.3f, Color.OrangeRed, null);
                }
            }
        }
    }
}
