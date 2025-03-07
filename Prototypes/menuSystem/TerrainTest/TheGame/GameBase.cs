using System;
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
using TheGame.Game_Screens;

namespace TheGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameBase : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameEngine.Initialize(this);

            

            GameEngine.BaseScreen = new GameScreen("base");

            Camera camera = new Camera(GameEngine.BaseScreen);
            camera.Position = new Vector3(0.0f, 25.0f, 10.0f);
            camera.LookAt = new Vector3(0.0f, -5.0f, -10.0f);
            GameEngine.Services.AddService(typeof(Camera), (object)(camera));

            GamepadDevice gamepadDevice = new GamepadDevice(GameEngine.BaseScreen, PlayerIndex.One);
            GameEngine.Services.AddService(typeof(GamepadDevice), gamepadDevice);

            KeyboardDevice keyboardDevice = new KeyboardDevice(GameEngine.BaseScreen);
            GameEngine.Services.AddService(typeof(KeyboardDevice), keyboardDevice);


            new MainMenuScreen("main");
            
            
            //new TestScreen("test");

            base.Initialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameEngine.GameTime = gameTime;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
