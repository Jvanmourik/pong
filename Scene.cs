using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pong
{
    public class Scene
    {
        private GamePong game;
        public GameObject rootNode;
        public Color backgroundColor = new Color(36, 36, 36);

        private bool active = true;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                rootNode.UpdateHierarchyActiveConditions();
            }
        }

        public Scene(GamePong pong)
        {
            game = pong;
            rootNode = new GameObject(game);
        }

        // Load content
        public void LoadContent()
        {
            List<GameObject> gameObjects = rootNode.children;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is Sprite)
                {
                    Sprite sprite = (Sprite)gameObjects[i];
                    sprite.LoadContent();
                }
            }
        }

        // Update Scene
        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                List<GameObject> gameObjects = rootNode.children;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i].ActiveInHierarchy)
                        gameObjects[i].Update(gameTime);
                }
            }
        }

        // Draw Scene
        public void Draw(GameTime gameTime)
        {
            if (Active)
            {
                game.GraphicsDevice.Clear(backgroundColor * 0.1f);

                List<GameObject> gameObjects = rootNode.children;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Sprite)
                    {
                        Sprite sprite = (Sprite)gameObjects[i];
                        if (sprite.isVisible)
                            sprite.Draw(gameTime);
                    }
                }
            }
        }
    }
}
