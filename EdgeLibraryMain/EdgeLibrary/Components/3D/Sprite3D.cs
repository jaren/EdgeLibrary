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

namespace EdgeLibrary
{
    /// <summary>
    /// A 3D sprite
    /// </summary>
    public class Sprite3D : DrawableGameComponent, ICloneable
    {
        //Used to identify the sprite
        public string ID;

        //Used to store data in a sprite
        public List<string> Data;

        //How the sprite is viewed in the world
        public SpriteModel Model;
        public Vector3 Position { get { return _position; } set { _position = value; ReloadMatrix(); } }
        private Vector3 _position;
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; ReloadMatrix(); } }
        private Vector3 _rotation;
        public Vector3 Scale { get { return _scale; } set { _scale = value; ReloadMatrix(); } }
        private Vector3 _scale;

        public delegate void Sprite3DEvent(Sprite3D sprite, GameTime gameTime);
        public event Sprite3DEvent OnUpdate;
        public event Sprite3DEvent OnDraw;
        public event Sprite3DEvent OnAdded;
        public event Sprite3DEvent OnRemoved;

        protected Dictionary<string, Action3D> Actions;
        protected List<string> actionsToRemove;

        //Indicates whether a sprite should still exist
        public bool ShouldBeRemoved { get; private set; }

        protected Matrix Matrix;

        public Sprite3D(Vector3 position, SpriteModel model)
            : base(EdgeGame.Game)
        {
            ID = this.GenerateID();

            _position = position;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;

            Data = new List<string>();

            ShouldBeRemoved = false;

            Actions = new Dictionary<string, Action3D>();
            actionsToRemove = new List<string>();

            Model = model;
            ReloadMatrix();
        }

        private void ReloadMatrix()
        {
            Matrix =
            Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position)
            * Matrix.CreateScale(Scale);
            Model.World = Matrix;
        }

        //Gets an action
        public T Action<T>(string id) where T : Action3D
        {
            return (T)Actions[id];
        }

        //Adds an action
        public void AddAction(string id, Action3D action)
        {
            //Makes a Clone so two sprites don't have the same action
            Action3D Clone = action.Clone();
            Actions.Add(id, Clone);
        }
        public void AddAction(Action3D action)
        {
            AddAction(action.GenerateID(), action);
        }

        //Removes an action
        public bool RemoveAction(string id)
        {
            return Actions.Remove(id);
        }

        //Clears the actions
        public void ClearActions()
        {
            Actions.Clear();
        }

        //Adds the sprite to the game
        public void AddToGame()
        {
            EdgeGame.Game.Components.Add(this);
            if (OnAdded != null)
            {
                OnAdded(this, EdgeGame.GameTime);
            }
        }

        //Removes the sprite from the game
        public void RemoveFromGame()
        {
            //This may not be in the game, it could be in a particle emitter
            if (EdgeGame.Game.Components.Contains(this))
            {
                EdgeGame.Game.Components.Remove(this);
            }
            ShouldBeRemoved = true;
            if (OnAdded != null)
            {
                OnAdded(this, EdgeGame.GameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Updates the actions
            actionsToRemove.Clear();
            foreach (KeyValuePair<string, Action3D> action in Actions)
            {
                action.Value.Update(gameTime, this);

                if (action.Value.toRemove)
                {
                    actionsToRemove.Add(action.Key);
                }
            }
            foreach (string key in actionsToRemove)
            {
                Actions.Remove(key);
            }

            //Updates the sprite
            Model.Update(gameTime);
            base.Update(gameTime);
            if (OnUpdate != null)
            {
                OnUpdate(this, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Model.Draw(gameTime);
            base.Draw(gameTime);
            if (OnDraw != null)
            {
                OnDraw(this, gameTime);
            }
        }

        public virtual object Clone()
        {
            Sprite3D clone = (Sprite3D)MemberwiseClone();
            clone.Model = (SpriteModel)Model.Clone();
            return clone;
        }
    }
}
