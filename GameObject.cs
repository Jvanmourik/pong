using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pong
{
    public class GameObject
    {
        public GamePong game;

        public String name;

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
                if (value && parent.ActiveInHierarchy || !value)
                {
                    ActiveInHierarchy = value;
                }
            }
        }

        private bool activeInHierarchy = true;
        public bool ActiveInHierarchy
        {
            get
            {
                return activeInHierarchy;
            }
            set
            {
                activeInHierarchy = value;
                for (int i = 0; i < localChildren.Count; i++)
                    localChildren[i].ActiveInHierarchy = value;
            }
        }

        public bool IgnoreCollision = true;

        private GameObject Parent = null;
        public GameObject parent
        {
            get
            {
                return Parent;
            }
            set
            {
                Parent = value;
                if (value.GetType() == typeof(GameObject))
                    hasParent = true;
                else
                    hasParent = false;
            }
        }

        public bool hasParent = false;
        public List<GameObject> localChildren = new List<GameObject>();
        public List<GameObject> children
        {
            get
            {
                return getAllChildren(this);
            }
        }

        public Transform transform = new Transform(0f, 0f, 0f, 0f);

        // Constructor
        public GameObject(
            GamePong pong,
            float x = 0f,
            float y = 0f,
            float width = 0f,
            float height = 0f)
        {
            game = pong;
            transform.position = new Vector2(x, y);
            transform.Width = width;
            transform.Height = height;
        }

        // Add child
        public void AddChild(GameObject gameObject)
        {
            localChildren.Add(gameObject);
            gameObject.parent = this;
        }

        // Get child with specified name.
        public GameObject GetChild(String withName, bool recursively)
        {
            List<GameObject> c;
            if (recursively)
                c = children;
            else
                c = localChildren;

            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].name == withName)
                    return c[i];
            }
            return null;
        }

        // Update GameObject
        public virtual void Update(GameTime gameTime)
        {
            
        }

        // Update GameObject.activeInHierarchy of all GameObjects
        public void UpdateHierarchyActiveConditions()
        {
            // TODO: Add Hierarchy dependency for the active condition
            // Determine if GameObject is active in the game
            // That is the case if GameObject.active is true, aswell as it's parents
            if (Active)
            {
                for (var i = 0; i < children.Count; i++)
                    children[i].ActiveInHierarchy = true;
            }
            else
            {
                for (var i = 0; i < children.Count; i++)
                    children[i].ActiveInHierarchy = false;
            }
        }

        public Vector2 GetWorldCoordinates()
        {
            Vector2 position = transform.position;
            List<GameObject> parents = getAllParents(this);
            for (int i = 0; i < parents.Count; i++)
            {
                GameObject parent = parents[i];
                position += parent.transform.position;
            }
            return position;
        }

        private List<GameObject> getAllParents(GameObject child)
        {
            List<GameObject> list = new List<GameObject>();
            if (child.hasParent)
            {
                list.Add(child.parent);
                if (child.parent.hasParent)
                    list.AddRange(getAllParents(parent));
            }
            return list;
        }

        private List<GameObject> getAllChildren(GameObject parent)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < parent.localChildren.Count; i++)
            {
                GameObject gameObject = parent.localChildren[i];
                list.Add(gameObject);
                if (gameObject.children.Count > 0)
                    list.AddRange(getAllChildren(gameObject));
            }
            return list;
        }
    }
}
