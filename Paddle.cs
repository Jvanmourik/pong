using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Paddle : Sprite
    {
        public enum State
        {
            Alive,
            Dead,
            Invurnable
        }

        public State CurrentState = State.Alive;

        public Input input = new Input();
        public float Speed = 0.45f;
        private int health = 5;
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0)
                    CurrentState = State.Dead;
                else
                    CurrentState = State.Alive;
            }
        }

        // Constructor
        public Paddle(
            GamePong pong,
            String path = null,
            float x = 0f,
            float y = 0f,
            float width = 10f,
            float height = 10f) : base(pong, path, x, y, width, height)
        {
            color = Color.Black;
        }
        
        // Update Paddle
        public override void Update(GameTime gameTime)
        {
            int deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            Viewport viewport = game.GraphicsDevice.Viewport;
            
            // Update position
            if (Keyboard.GetState().IsKeyDown(input.Up))
            {
                transform.position.Y -= Speed * deltaTime;
                if (transform.position.Y < 0)
                    transform.position.Y = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(input.Down))
            {
                transform.position.Y += Speed * deltaTime;
                if (transform.position.Y > viewport.Height - transform.Height)
                    transform.position.Y = viewport.Height - transform.Height;
            }
        }
    }
}