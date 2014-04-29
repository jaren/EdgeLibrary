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
    public class Sprite : DrawableGameComponent, ICloneable
    {
        //Used to identify the sprite
        public string ID;

        //Gets the texture size of the sprite
        public virtual float Width { get { return Texture == null ? 0 : Texture.Width; } }
        public virtual float Height { get { return Texture == null ? 0 : Texture.Height; } }

        //If the Sprite is supposed to be removed from the game, this will be set to true
        public bool ShouldBeRemoved { get; protected set; }

        //Sets the texture through a string
        public string TextureName { set { Texture = EdgeGame.GetTexture(value); } }

        //Sets the scale with a Vector2
        public virtual Vector2 Scale { get { return _scale; } set
            {
                _scale = new Vector2(
                    (value.X < 0 ? 0 : value.X),
                    (value.Y < 0 ? 0 : value.Y));
            }
        }
        private Vector2 _scale;

        //Optional visual effects
        public virtual Color Color { get; set; }
        //Measured in radians
        public virtual float Rotation { get; set; }
        public virtual SpriteEffects SpriteEffects { get; set; }

        //Used to store data in a sprite
        public List<string> Data;

        //Gets the origin point of the sprite, which is either (0, 0) or half of the texture size
        public virtual Vector2 OriginPoint { get; protected set; }

        //If set to true, origin will be top left; if not, origin will be center
        public virtual bool CenterAsOrigin { get { return _centerAsOrigin; } set { _centerAsOrigin = value; reloadOriginPoint(); } }
        private bool _centerAsOrigin;

        //Used for detecting collisions
        public virtual CollisionBody CollisionBody { get; set; }
        public virtual ShapeTypes CollisionBodyType { get; set; }

        //The texture for drawing
        public virtual Texture2D Texture { get { return _texture; } set { _texture = value; reloadOriginPoint(); } }
        private Texture2D _texture;

        //Used for OnCollideStart
        protected List<string> currentlyCollidingWithIDs;

        //Used to change properties of the sprite - could be uesd for moving, color changing, etc.
        protected Dictionary<string,Action> Actions;
        protected List<string> actionsToRemove;

        //Location of the sprite
        public virtual Vector2 Position { get; set; }

        //Which blend state to use when drawing the sprite
        public BlendState BlendState { get; set; }

        //Used for collisions
        public delegate void CollisionEvent(Sprite sender, Sprite collided, GameTime gameTime);
        public event CollisionEvent OnCollideStart = delegate { };
        public event CollisionEvent OnCollide = delegate { };

        //Gives a button functionality to sprites
        public delegate void ButtonEvent(Sprite sender, Vector2 mousePosition, GameTime gameTime);
        public event ButtonEvent OnClick = delegate { };
        public event ButtonEvent OnMouseOver = delegate { };
        public event ButtonEvent OnMouseOff = delegate { };

        public delegate void SpriteEvent(Sprite sprite, GameTime gameTime);
        public event SpriteEvent OnUpdate = delegate { };
        public event SpriteEvent OnDraw = delegate { };
        public event SpriteEvent OnAdded = delegate { };
        public event SpriteEvent OnRemoved = delegate { };

        public Sprite(string textureName, Vector2 position) : base(EdgeGame.Game)
        {
            ID = this.GenerateID();

            Position = position;

            Data = new List<string>();

            ShouldBeRemoved = false;

            //Sets the default visual effects
            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0f;
            SpriteEffects = SpriteEffects.None;

            _centerAsOrigin = true;

            Actions = new Dictionary<string, Action>();
            actionsToRemove = new List<string>();

            currentlyCollidingWithIDs = new List<string>();

            CollisionBodyType = ShapeTypes.rectangle;
            CollisionBody = CollisionBody.BodyWithSprite(this, CollisionLayers.All);

            if (textureName != null)
            {
                Texture = EdgeGame.GetTexture(textureName);
            }
            reloadOriginPoint();
        }
        public Sprite(string textureName, Vector2 position, Color color, Vector2 scale, float rotation = 0f, SpriteEffects effects = SpriteEffects.None) : this(textureName, position)
        {
            Color = color;
            Rotation = rotation;
            Scale = scale;
            SpriteEffects = effects;
        }

        //Initializes the collision body with this sprite
        public virtual void InitializeCollision()
        {
            CollisionBody = CollisionBody.BodyWithSprite(this, CollisionLayers.All);
        }
        public virtual void InitializeCollision(ShapeTypes shapeType, CollisionLayers layers)
        {
            CollisionBodyType = shapeType;
            CollisionBody = CollisionBody.BodyWithSprite(this, layers);
        }

        //Adds the sprite to the game
        public virtual void AddToGame()
        {
            EdgeGame.Game.Components.Add(this);
            OnAdded(this, EdgeGame.GameTime);
        }

        //Removes the sprite from the game
        public virtual void RemoveFromGame()
        {
            EdgeGame.Game.Components.Remove(this);
            ShouldBeRemoved = true;
            OnRemoved(this, EdgeGame.GameTime);
        }

        //Gets the bounding box of this sprite
        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(Position.X - Width / 2 * Scale.X), (int)(Position.Y - Height / 2 * Scale.Y), (int)(Width * Scale.X), (int)(Height * Scale.Y));
        }

        //Reloads origin point based on texture
        protected virtual void reloadOriginPoint()
        {
            if (Texture != null)
            {
                if (CenterAsOrigin)
                {
                    OriginPoint = new Vector2(Width / 2f, Height / 2f);
                }
                else
                {
                    OriginPoint = Vector2.Zero;
                }
            }
        }

        //Gets an action
        public T Action<T>(string id) where T : Action
        {
            return (T)Actions[id];
        }

        //Adds an action
        public void AddAction(string id, Action action)
        {
            //Makes a Clone so two sprites don't have the same action
            Action Clone = action.Clone();
            Actions.Add(id, Clone);
        }
        public void AddAction(Action action)
        {
            AddAction(action.GenerateID(), action);
        }

        //Removes an action
        public bool RemoveAction(string id)
        {
            return Actions.Remove(id);
        }

        //Checks if this is colliding with any other sprite
        public virtual void UpdateCollision(GameTime gameTime, GameComponentCollection components)
        {
            if (CollisionBody != null)
            {
                //Scales the collision body
                CollisionBody.ScaleWith(this);

                //If not using center as origin, change the CollisionBody position
                if (CenterAsOrigin)
                {
                    CollisionBody.Position = new Vector2(Position.X, Position.Y);
                }
                else
                {
                    CollisionBody.Position = new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
                }

                //Checking if this sprite collides with the mouse
                if (CollisionBody.CheckForCollide(new CollisionBody(new ShapeRectangle(Input.MousePosition, 1, 1))))
                {
                    //The mouse just clicked this sprite
                    if (Input.JustLeftClicked())
                    {
                        OnClick(this, Input.MousePosition, gameTime);
                    }

                    //The mouse just moved over this sprite
                    if (!CollisionBody.CheckForCollide(new CollisionBody(new ShapeRectangle(Input.PreviousMousePosition, 1, 1))))
                    {
                        OnMouseOver(this, Input.MousePosition, gameTime);
                    }
                }
                    //If it hasn't collided, then check if it just moved off
                else if (CollisionBody.CheckForCollide(new CollisionBody(new ShapeRectangle(Input.PreviousMousePosition, 1, 1))))
                {
                    OnMouseOff(this, Input.MousePosition, gameTime);
                }

                //Loops through all the elements and checks if they're colliding
                foreach (GameComponent component in components)
                {
                    if (component is Sprite)
                    {
                        Sprite sprite = component as Sprite;

                        if (sprite != this && sprite.CollisionBody != null)
                        {
                            if (CollisionBody.CheckForCollide(sprite.CollisionBody))
                            {
                                OnCollide(this, sprite, gameTime);
                            }
                        }
                    }
                }
            }
        }

        //Updates the sprite
        protected override void Update(GameTime gameTime)
        {
            UpdateCollision(gameTime, EdgeGame.Game.Components);

            //Updates the actions
            actionsToRemove.Clear();
            foreach (KeyValuePair<string, Action> action in Actions)
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

            base.Update(gameTime);

            OnUpdate(this, gameTime);
        }

        //Draws to the spritebatch
        public override void Draw(GameTime gameTime)
        {
            //Restarts the SpriteBatch if the BlendState is not AlphaBlend
            if (BlendState != BlendState.AlphaBlend)
            {
                EdgeGame.Game.SpriteBatch.End();
                EdgeGame.Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, EdgeGame.Camera.GetTransform());
            }

            EdgeGame.Game.SpriteBatch.Draw(Texture, Position, null, Color, Rotation, OriginPoint, Scale, SpriteEffects, 0);
            base.Draw(gameTime);

            if (BlendState != BlendState.AlphaBlend)
            {
                EdgeGame.Game.SpriteBatch.End();
                EdgeGame.Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, EdgeGame.Camera.GetTransform());
            }

            OnDraw(this, gameTime);
        }

        //Draws the area of the collision body
        public virtual void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (CollisionBody != null)
            {
                CollisionBody.Shape.DrawDebug(gameTime, spriteBatch, color);
            }
        }

        //Creates a copy of the sprite
        public override object Clone()
        {
            Sprite clone = (Sprite)MemberwiseClone();

            clone.Actions = new Dictionary<string, Action>();
            foreach (var action in Actions)
            {
                clone.AddAction(action.Key, action.Value);
            }
            return clone;
        }
    }
}
