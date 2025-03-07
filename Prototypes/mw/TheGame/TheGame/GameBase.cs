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
using TheGame.Components.Cameras;

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
            GameEngine.BaseScreen.Initialize();
            //TestCamera camera = new TestCamera(GameEngine.BaseScreen);
            //Camera camera = new ActionCamera(GameEngine.BaseScreen);
            //camera.Position = new Vector3(0.0f, 10.0f, 25.0f);
            //camera.LookAt = new Vector3(0, 0, 0);
            //camera.Initialize();
            //GameEngine.Services.AddService(typeof(Camera), (object)(camera));

            //GamepadDevice gamepadDevice = new GamepadDevice(GameEngine.BaseScreen, PlayerIndex.One);
            //gamepadDevice.Initialize();
            //GameEngine.Services.AddService(typeof(GamepadDevice), gamepadDevice);

            ActionCamera camera = new ActionCamera(GameEngine.BaseScreen);
            GameEngine.Services.AddService(typeof(Camera), (object)(camera));

            AudioManager audioManager = new AudioManager(GameEngine.BaseScreen);
            audioManager.Initialize();
            GameEngine.Services.AddService(typeof(AudioManager), audioManager);

            InputHub inputHub = new InputHub();
            GameEngine.Services.AddService(typeof(InputHub), inputHub);

            KeyboardDevice keyboardDevice = new KeyboardDevice(GameEngine.BaseScreen);
            keyboardDevice.Initialize();
            GameEngine.Services.AddService(typeof(KeyboardDevice), keyboardDevice);

            GameEngine.BaseScreen.AlwaysUpdate = true;

            //new TestScreen("test");
            //new SkyboxScreen("sky");
            //Level l = new Level("level", "Terrain\\terrain");
            GameScreen mainMenu = new MainMenuScreen("main");
            mainMenu.Initialize();
            //GameScreen charselect = new CharacterSelectScreen("charsel");
            //charselect.Initialize();

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
