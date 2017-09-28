using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Ball : Sprite
    {
        public Vector2 velocity = new Vector2(0f, 0f);
        public float speed = 1f;

        private Random r = new Random();

        // Constructor
        public Ball(
            GamePong pong,
            String path,
            float x = 0f,
            float y = 0f,
            float width = 10f,
            float height = 10f) : base(pong, path, x, y, width, height)
        {
            transform.direction = new Vector2((float)r.NextDouble()*2-1, (float)r.NextDouble()*2-1);
            transform.direction.Normalize();
        }

        // Update Ball
        public override void Update(GameTime gameTime)
        {
            int deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            Viewport viewport = game.GraphicsDevice.Viewport;
            Scene scene = game.scene;

            // Update position
            velocity = transform.direction * speed;
            transform.position += velocity * deltaTime;

            // Check collision with viewport
            Rectangle bounds = transform.getRectangle();
            if (bounds.Center.X < 0)
            {
                game.players[0].Health--;
                game.eventHandler.CurrentState = EventHandler.State.Inactive;
            }
            else if (bounds.Center.X > viewport.Width)
            {
                game.players[1].Health--;
                game.eventHandler.CurrentState = EventHandler.State.Inactive;
            }
            else if (bounds.Top < 0)
            {
                transform.direction.Y *= -1;
                transform.position.Y = 0;
            }
            else if (bounds.Bottom > viewport.Height)
            {
                transform.direction.Y *= -1;
                transform.position.Y = viewport.Height - transform.Height;
            }

            // Check collision with other gameObjects
            bounds = transform.getRectangle();
            for (int i = 0; i < scene.rootNode.children.Count; i ++)
            {
                GameObject gameObject = scene.rootNode.children[i];
                Rectangle collider = gameObject.transform.getRectangle();
                
                if (gameObject != this && bounds.Intersects(collider) && !gameObject.IgnoreCollision)
                {

                    // This section calculates at which diagonal quadrant the ball hits the paddle. 
                    // The problem with this is: ehen the paddle is small and the ball has a high velocity,
                    // it sometimes will intersect with the wrong quadrants.
                    // 
                    // Using a more simple approach without quadrant detection is a better fit for pong.
                    // The loss in precision is not affecting gameplay.
                    /*
                    
                    float slopeDiagonal = collider.Height / collider.Width;
                    Point intersection = bounds.Center - collider.Location;
                    
                    if (intersection.Y < slopeDiagonal * intersection.X) // Top && Right
                    {
                        if (intersection.Y < -1 * slopeDiagonal * intersection.X + collider.Height) // Top
                        {
                            transform.direction.Y *= (transform.direction.Y < 0) ? 1 : -1;
                            transform.position.Y = collider.Top - transform.Height;
                        }
                        else // Right
                        {
                            transform.direction.X *= -1;
                            transform.position.X = collider.Right;
                            game.eventHandler.ShakeWindow(velocity*25, 80);
                        }
                    }
                    else // Bottom && Left
                    {
                        if (intersection.Y > -1 * slopeDiagonal * intersection.X + collider.Height) // Bottom
                        {
                            transform.direction.Y *= (transform.direction.Y > 0) ? 1 : -1;
                            transform.position.Y = collider.Bottom;
                        }
                        else // Left/
                        {
                            transform.direction.X *= -1;
                            transform.position.X = collider.Left - transform.Width;
                            game.eventHandler.ShakeWindow(velocity*25, 80);
                        }
                    }
                    
                    */
                    if(transform.position.X > game.players[0].transform.getRectangle().Right && (transform.position.X + transform.Width) < game.players[1].transform.getRectangle().Left) { Console.WriteLine(gameObject.name); }
                    

                    // Update direction and position
                    if (transform.direction.X > 0) // Hit with a direction from Left to Right
                    {
                        transform.direction.X *= -1;
                        transform.position.X = collider.Left - transform.Width;
                    }
                    else // Hit with a direction from Right to Left
                    {
                        transform.direction.X *= -1;
                        transform.position.X = collider.Right;
                    }

                    // Update direction -> angle
                    int dirX = (transform.direction.X > 0) ? 1 : -1;

                    float offset = ((bounds.Center.Y - collider.Center.Y) / ((bounds.Height + collider.Height) * 0.5f));

                    transform.direction.X = transform.direction.X + (float)Math.Sin((1f - Math.Abs(offset)) * 0.5 * Math.PI) * dirX;
                    transform.direction.Y = transform.direction.Y + (float)Math.Sin(offset * 0.5 * Math.PI);
                    transform.direction.Normalize();

                    // Update speed
                    speed += 0.02f;
                }
            }
        }
    }
}
