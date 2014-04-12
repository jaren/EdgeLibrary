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
using EdgeLibrary;

namespace EdgeDemo
{
    public class GameScene : Scene
    {
        ParticleEmitter bulletEmitter;
        ParticleEmitter leftEmitter;
        ParticleEmitter rightEmitter;

        public GameScene() : base("GameScene")
        {
            EdgeGame.Instance.SceneHandler.SwitchScene(this);
            Sprite sprite = new Sprite("player", Vector2.One * 500);
            sprite.DrawLayer = 100;
            sprite.AddToGame();
            sprite.AddAction(new ARotateTowards("Rotate", Input.MouseSprite, 90f.ToRadians()));
            sprite.AddAction(new AFollow("Follow", Input.MouseSprite, 7));
            sprite.OnUpdate += new Element.ElementUpdateEvent(updateSprite);
            EdgeGame.Instance.Camera.ClampTo(sprite);

            DebugText Debug = new DebugText("ComicSans-10", Vector2.Zero);
            Debug.FollowsCamera = false;
            Debug.AddToGame();

            ParticleEmitter stars = new ParticleEmitter("Stars", Vector2.Zero)
            {
                BlendState = BlendState.AlphaBlend,
                EmitWait = 0,
                GrowSpeed = 0,
                MaxParticles = 300000,
                MinScale = new Vector2(0.5f),
                MaxScale = new Vector2(1f),
                MinVelocity = new Vector2(-0.1f),
                MaxVelocity = new Vector2(0.1f),
                MinLife = 10000,
                MaxLife = 10000,
                MinRotationSpeed = -0.1f,
                MaxRotationSpeed = 0.1f,
                EmitArea = new Vector2(5000, 5000),
                ColorIndex = new ColorChangeIndex(4000, Color.Transparent, Color.White, Color.Transparent),
            };
            stars.AddAction(new AClamp(sprite));
            stars.AddToGame();

            bulletEmitter = new ParticleEmitter("Fire", Vector2.Zero)
            {
                BlendState = BlendState.Additive,
                EmitWait = 10,
                GrowSpeed = 0,
                MaxParticles = 300,
                MinScale = new Vector2(0.5f),
                MaxScale = new Vector2(1f),
                MinVelocity = new Vector2(-1f),
                MaxVelocity = new Vector2(1f),
                MinLife = 1000,
                MaxLife = 1500,
                MinRotationSpeed = 0f,
                MaxRotationSpeed = 0f,
                EmitArea = new Vector2(1, 1),
                ColorIndex = new ColorChangeIndex(400, Color.LightGreen, Color.DarkGreen, Color.Transparent),
            };

            leftEmitter = new ParticleEmitter("Fire", Vector2.Zero)
            {
                BlendState = BlendState.Additive,
                EmitWait = 0,
                GrowSpeed = -1f,
                MaxParticles = 1000,
                MinParticlesToEmit = 5,
                MaxParticlesToEmit = 10,
                MinScale = new Vector2(0.6f),
                MaxScale = new Vector2(0.8f),
                MinVelocity = new Vector2(-1f),
                MaxVelocity = new Vector2(1f),
                MinLife = 500,
                MaxLife = 500,
                MinRotationSpeed = 0,
                MaxRotationSpeed = 0,
                EmitArea = new Vector2(5, 5),
                MinColorIndex = new ColorChangeIndex(125, Color.Orange, Color.DarkRed, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(125, Color.Red, Color.OrangeRed, Color.Transparent)
            };
            leftEmitter.AddToGame();

            rightEmitter = (ParticleEmitter)leftEmitter.Clone();
            rightEmitter.AddToGame();
        }

        double elapsed = 0;
        double toElapse = 200;
        float bulletspeed = 15;
        float maxPlayerSpeed = 10;
        void updateSprite(Element element, GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Vector2.Distance(Input.MousePosition, element.Position) / 20;
            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Math.Min(((Sprite)element).GetAction<AFollow>("Follow").Speed, maxPlayerSpeed);

            ((Sprite)element).GetAction<AFollow>("Follow").Paused = !Input.IsKeyDown(Keys.Space);

            if (Input.IsLeftClicking() && elapsed >= toElapse)
            {
                elapsed = 0;
                Sprite sprite = new Sprite("laserGreen", element.Position.PositionRelativeTo(40, ((Sprite)element).Rotation - 180f.ToRadians()));
                sprite.OnUpdate += new Element.ElementUpdateEvent(updateProjectile);
                sprite.Rotation = ((Sprite)element).Rotation;
                Vector2 MoveVector = Input.MousePosition.PositionRelativeTo(40, ((Sprite)element).Rotation - 180f.ToRadians()) - sprite.Position;
                MoveVector.Normalize();
                sprite.AddAction(new AMove(MoveVector * bulletspeed));
                element.AddSubElement(sprite);

                Sprite sprite2 = (Sprite)sprite.Clone();
                sprite2.Position = element.Position.PositionRelativeTo(40, ((Sprite)element).Rotation);
                element.AddSubElement(sprite2);

                ParticleEmitter bullet1emitter = (ParticleEmitter)bulletEmitter.Clone();
                bullet1emitter.AddAction(new AClamp(sprite));
                sprite.AddSubElement(bullet1emitter);

                ParticleEmitter bullet2emittter = (ParticleEmitter)bulletEmitter.Clone();
                bullet2emittter.AddAction(new AClamp(sprite2));
                sprite2.AddSubElement(bullet2emittter);
            }

            Vector2 average = (((Sprite)element).Rotation - (270f).ToRadians()).ToVector() * 2;
            leftEmitter.MinVelocity = average * 3f; leftEmitter.MaxVelocity = average * 4f;
            rightEmitter.MinVelocity = leftEmitter.MinVelocity; rightEmitter.MaxVelocity = leftEmitter.MaxVelocity;

            leftEmitter.Position = element.Position.PositionRelativeTo(35, ((Sprite)element).Rotation - 180f.ToRadians());
            rightEmitter.Position = element.Position.PositionRelativeTo(35, ((Sprite)element).Rotation);
        }

        void updateProjectile(Element element, GameTime gameTime)
        {
            if (element.CheckOffScreen())
            {
                element.Remove();
            }
        }
    }
}
