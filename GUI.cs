using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class GUI : Sprite
    {
        private SpriteFont font;

        // Constructor
        public GUI(GamePong pong, String path) : base (pong, path)
        {
            
        }

        // Load Sprite
        public override void LoadContent()
        {
            base.LoadContent();

            font = game.Content.Load<SpriteFont>("font");
        }

        // Update GUI
        public override void Update(GameTime gameTime)
        {

        }

        // Draw GUI
        public override void Draw(GameTime gameTime)
        {
            int totalTime = gameTime.TotalGameTime.Milliseconds;
            Viewport viewport = game.GraphicsDevice.Viewport;

            Rectangle rect = new Rectangle(0, 0, sprite.Width, sprite.Height);

            spriteBatch.Begin();

            // Draw Lives
            int padding = rect.Width / 4;
            int offset = (int)game.players[0].transform.Width;
            rect.Y = padding;
            for (int i = 0; i < game.players[0].Health; i++)
            {
                rect.X = i * rect.Width + (i+2) * padding + offset;
                spriteBatch.Draw(sprite, rect, game.players[0].color);
            }
            for (int i = 0; i < game.players[1].Health; i++)
            {
                rect.X = viewport.Width - (i+1) * rect.Width - (i+2) * padding - offset;
                spriteBatch.Draw(sprite, rect, game.players[1].color);
            }

            // Draw inactive screen
            if (game.eventHandler.CurrentState == EventHandler.State.Inactive)
            {
                // Draw predicted ball direction
                int a = 5;
                for (int i = 1; i < a; i++) {
                    double localTime = totalTime / 1000;
                    rect.X = (int)(game.balls[0].transform.position.X + game.balls[0].transform.direction.X * i * 16 * Math.Sin((totalTime/1000.0) * 0.5 * Math.PI));
                    rect.Y = (int)(game.balls[0].transform.position.Y + game.balls[0].transform.direction.Y * i * 16 * Math.Sin((totalTime/1000.0) * 0.5 * Math.PI));
                    spriteBatch.Draw(game.balls[0].sprite, rect, game.balls[0].color * (i/5f) * 0.4f);
                }

                // Draw String
                String nextRoundString = "Press spacebar";
                Vector2 stringDimensions = font.MeasureString(nextRoundString);
                spriteBatch.DrawString(font, nextRoundString, new Vector2((viewport.Width-stringDimensions.X)/2, viewport.Height/4), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            // Draw end screen
            if (game.eventHandler.CurrentState == EventHandler.State.Ended)
            {
                String playerName = "Nobody";
                for (int i = 0; i < game.players.Count; i++)
                {
                    if (game.players[i].CurrentState == Paddle.State.Dead)
                    {
                        playerName = (game.players[i].name == "yellow") ? "Red" : "Yellow";
                    }
                }

                // Draw a new background
                Color c = Color.White;
                if (totalTime % 500 > 150) {
                    Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    texture.SetData<Color>(new Color[] { Color.White });

                    int w = (playerName == "Yellow") ? 0 : 1;
                    spriteBatch.Draw(texture, new Rectangle(0, 0, viewport.Width, viewport.Height), game.players[w].color);

                    c = game.scene.backgroundColor;
                }

                

                String gameOverString = playerName + " won!";
                Vector2 stringDimensions = font.MeasureString(gameOverString);

                spriteBatch.DrawString(font, gameOverString, new Vector2((viewport.Width - stringDimensions.X) / 2, viewport.Height / 4), c);
            }

            spriteBatch.End();
        }
    }
}
