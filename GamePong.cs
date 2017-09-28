using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Pong
{
    public struct Input
    {
        public Keys Up;
        public Keys Down;
    }

    public class GamePong : Game
    {
        GraphicsDeviceManager graphics;

        public Scene scene;
        public GUI gui;
        public EventHandler eventHandler;

        public List<Paddle> players = new List<Paddle>();
        public List<Ball> balls = new List<Ball>();

        public GamePong()
        {
            Window.IsBorderless = true;
            Window.AllowUserResizing = false;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Initialize
        protected override void Initialize()
        {
            Viewport viewport = GraphicsDevice.Viewport;

            // Create a Scene
            scene = new Scene(this);

            // Create a GUI
            gui = new GUI(this, "heart16");
            gui.color = new Color(190, 147, 255);

            // Create an EventHandler
            eventHandler = new EventHandler(this);

            // Create an empty container GameObject
            GameObject container = new GameObject(this);
            container.name = "container";

            // Create an empty container GameObject
            Sprite background = new Sprite(this);
            background.color = new Color(24, 24, 24);
            background.transform.Width = viewport.Width;
            background.transform.Height = viewport.Height;

            // Create a Ball
            Ball ball = new Ball(this, "ball", width: 16, height: 16);
            ball.IgnoreCollision = false;

            // Create player1 player's Paddle
            Paddle player1 = new Paddle(this, width: 8, height: 96);
            player1.name = "yellow";
            player1.IgnoreCollision = false;

            // Create player2 player's Paddle
            Paddle player2 = new Paddle(this, width: 8, height: 96);
            player2.name = "red";
            player2.IgnoreCollision = false;

            // Add the game objects to the scene
            scene.rootNode.AddChild(eventHandler);
            scene.rootNode.AddChild(container);
            scene.rootNode.AddChild(gui);
            container.AddChild(background);
            container.AddChild(ball);
            container.AddChild(player1);
            container.AddChild(player2);

            // Add the ball objects to a list
            balls.Add(ball);

            // Add the player objects to a list
            players.Add(player1);
            players.Add(player2);

            eventHandler.CurrentState = EventHandler.State.Inactive;
            eventHandler.SetupGameObjects();

            base.Initialize();
        }

        // Load content
        protected override void LoadContent()
        {
            scene.LoadContent();
        }

        // Unload content
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Update game
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Window.Position = new Point(Window.Position.X + 1, Window.Position.Y);

            scene.Update(gameTime);
            base.Update(gameTime);
        }

        // Draw game
        protected override void Draw(GameTime gameTime)
        {
            scene.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
