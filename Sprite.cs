using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Sprite : GameObject
    {
        String texturePath;

        public SpriteBatch spriteBatch;
        public Texture2D sprite;
        public Color color = Color.White;
        public float opacity = 1;
        public bool isVisible = true;

        // Constructor
        public Sprite(
            GamePong pong,
            String path = null,
            float x = 0f,
            float y = 0f,
            float width = 0f,
            float height = 0f) : base (pong, x, y, width, height)
        {
            texturePath = path;
        }

        // Load Sprite
        public virtual void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            try
            {
                if (texturePath != null)
                    sprite = game.Content.Load<Texture2D>(texturePath);
                else
                {
                    sprite = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    sprite.SetData<Color>(new Color[] { Color.White });
                }
            }
            catch
            {
                Console.WriteLine("Couldn't load texture: " + texturePath);
                sprite = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                sprite.SetData<Color>(new Color[] { Color.White });
            }
        }

        // Draw Sprite
        public virtual void Draw(GameTime gameTime)
        {
            Vector2 worldPosition = GetWorldCoordinates();
            Rectangle rect = transform.getRectangle();
            rect.X = (int)worldPosition.X;
            rect.Y = (int)worldPosition.Y;
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, rect, color * opacity);
            spriteBatch.End();
        }
    }
}
