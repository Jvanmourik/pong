using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class EventHandler : GameObject
    {
        public enum State
        {
            Inactive,
            Active,
            Paused,
            Ended
        }

        public enum GameMode
        {
            Undefined,
            Normal,
            CameraFollow,
            InTheDark,
            Hallucinating
        }

        private State currentState = State.Active;
        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                if (value == State.Active)
                {
                    game.scene.rootNode.GetChild("container", true).Active = true;
                }
                else
                {
                    game.scene.rootNode.GetChild("container", true).Active = false;
                    SetupGameObjects();
                    if (currentGameMode == GameMode.Undefined)
                        currentGameMode = GameMode.Normal;
                    else
                        ChangeGameMode();
                }
            }
        }

        private GameMode currentGameMode = GameMode.Undefined;
        public GameMode CurrentGameMode
        {
            get
            {
                return currentGameMode;
            }
            set
            {
                if (currentGameMode == GameMode.InTheDark)
                {
                    for (int i = 0; i < game.players.Count; i++)
                    {
                        game.players[i].opacity = 1;
                    }
                }

                if (value == GameMode.Hallucinating) {
                    for (int i = 0; i < game.players.Count; i++)
                    {
                        Keys Up = game.players[i].input.Up;
                        game.players[i].input.Up = game.players[i].input.Down;
                        game.players[i].input.Down = Up;
                    }
                }
                
                currentGameMode = value;

            }
        }

        private Random r = new Random();
        private Vector2 WindowOffset = new Vector2(0f, 0f);
        private Point InitialWindowPosition;
        private bool spaceUp = true;

        private float playerOpacity = 1;

        private const int Timer = 3000;
        private int timer = Timer;
        private bool animating = false;
        private int animateFloatDuration = 0;
        private int animateFloatElapsed = 0;
        private float animateFloatStartValue;
        private float animateFloatDeltaValue;

        // Constructor
        public EventHandler(GamePong pong) : base(pong)
        {
            InitialWindowPosition = game.Window.Position;
        }

        // Update EventHandler
        public override void Update(GameTime gameTime)
        {
            int deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            int totalTime = gameTime.TotalGameTime.Milliseconds;
            Viewport viewport = game.GraphicsDevice.Viewport;

            // Check if any player is dead
            if (CurrentState != State.Ended)
            {
                for (int i = 0; i < game.players.Count; i++)
                {
                    if (game.players[i].CurrentState == Paddle.State.Dead)
                    {
                        // Game over
                        CurrentState = State.Ended;
                    }
                }
            }

            if (CurrentState == State.Inactive && Keyboard.GetState().IsKeyDown(Keys.Space) && spaceUp)
            {
                // Next round
                CurrentState = State.Active;
                spaceUp = false;
            }

            if (CurrentState == State.Ended && Keyboard.GetState().IsKeyDown(Keys.Space) && spaceUp)
            {
                // New Game
                ResetLives();
                CurrentState = State.Inactive;
                spaceUp = false;
            }

            // Reset mechanism for spacebar
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                spaceUp = true;
            else
                spaceUp = false;

            // Game modes
            if (CurrentGameMode == GameMode.CameraFollow)
            {
                // Update window and 'container' GameObject position
                Rectangle bounds = game.balls[0].transform.getRectangle();
                WindowOffset = new Vector2(bounds.Center.X - viewport.Width * 0.5f, bounds.Center.Y - viewport.Height * 0.5f);

                game.scene.rootNode.GetChild("container", true).transform.position = -WindowOffset;
                if (game.Window != null)
                    game.Window.Position = new Point(InitialWindowPosition.X + (int)WindowOffset.X, InitialWindowPosition.Y + (int)WindowOffset.Y);
            }
            else if (CurrentGameMode == GameMode.InTheDark && CurrentState == State.Active)
            {
                // Add a timer in between animations
                if (!animating)
                {
                    timer -= deltaTime;
                    if (timer <= 0)
                    {
                        timer = Timer;
                        if (playerOpacity == 1)
                            AnimateFloat(playerOpacity, 0f, 1000);
                        else
                            AnimateFloat(playerOpacity, 1f, 1000);
                    }
                }

                // Animate opacity linearly
                // TODO: Should be placed inside a dedicated animater class
                if (animateFloatElapsed < animateFloatDuration && animating) {
                    animateFloatElapsed += deltaTime;
                    if (animateFloatElapsed > animateFloatDuration)
                    {
                        animateFloatElapsed = animateFloatDuration;
                        animating = false;
                    }
                    playerOpacity = animateFloatStartValue + animateFloatDeltaValue * (animateFloatElapsed / (float)animateFloatDuration);
                }

                for (int i = 0; i < game.players.Count; i++)
                {
                    game.players[i].opacity = playerOpacity;
                }
            }
            else if (CurrentGameMode == GameMode.Hallucinating && CurrentState != State.Ended)
            {
                float r = totalTime % 335 / 335f;
                float g = totalTime % 124 / 124f;
                float b = totalTime % 700 / 700f;

                List<GameObject> gameObjects = game.scene.rootNode.GetChild("container", true).children;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Paddle || gameObjects[i] is Ball)
                    {
                        Sprite sprite = (Sprite)gameObjects[i];
                        if (sprite.isVisible)
                            sprite.color = new Color(r, g, b, sprite.opacity);
                    }
                }
            }
        }

        // Interpolate a float over time
        // TODO: Should be placed inside a dedicated animater class
        private void AnimateFloat(float f, float toValue, int miliseconds)
        {
            animating = true;
            animateFloatElapsed = 0;
            animateFloatDuration = miliseconds;
            animateFloatStartValue = f;
            animateFloatDeltaValue = toValue - f;
        }

        // Setup GameObjects for next Round
        public void SetupGameObjects()
        {
            Viewport viewport = game.GraphicsDevice.Viewport;

            Ball ball = game.balls[0];
            Paddle yellow = game.players[0];
            Paddle red = game.players[1];
            GameObject container = game.scene.rootNode.GetChild("container", true);

            ball.transform.position = new Vector2((viewport.Width - ball.transform.Width) / 2, (viewport.Height - ball.transform.Height) / 2);
            ball.transform.direction = new Vector2(
                (float)(Math.Round(r.NextDouble())* 2 - 1),
                (float)((Math.Round(r.NextDouble()) * 2 - 1) * (r.NextDouble() + 0.5)));
            ball.transform.direction.Normalize();
            ball.speed = 0.45f;
            ball.color = Color.White;

            yellow.transform.position = new Vector2(yellow.transform.Width/2, (viewport.Height - yellow.transform.Height) / 2);
            yellow.color = new Color(236, 167, 44);
            yellow.input.Up = Keys.W;
            yellow.input.Down = Keys.S;

            red.transform.position = new Vector2(viewport.Width - red.transform.Width - red.transform.Width / 2, (viewport.Height - red.transform.Height) / 2);
            red.color = new Color(238, 86, 34);
            red.input.Up = Keys.Up;
            red.input.Down = Keys.Down;

            container.transform.position = new Vector2(0f, 0f);
            game.Window.Position = InitialWindowPosition;

            playerOpacity = 1;
            timer = Timer;
            animating = false;
        }

        // Reset player lives
        public void ResetLives()
        {
            int initialHealth = 5;
            Paddle yellow = game.players[0];
            Paddle red = game.players[1];

            yellow.Health = initialHealth;
            red.Health = initialHealth;
        }

        // Change Game mode
        public void ChangeGameMode()
        {
            GameMode[] array = (GameMode[])Enum.GetValues(typeof(GameMode));
            int i = (int)(r.NextDouble() * (array.Length - 1)) + 1;
            CurrentGameMode = array[i];
            Console.WriteLine(CurrentGameMode);
        }
    }
}
