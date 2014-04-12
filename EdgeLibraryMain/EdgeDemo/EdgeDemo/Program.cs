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
    /// <summary>
    /// TODO:
    /// -Add a physics engine
    /// -Mouse position is affected by camera rotation / scale
    /// -Fix FPS Counter
    /// 
    /// Optional TODO:
    /// -Add the ability to render a scene to a RenderTarget2D
    /// -SubElements are not copied in clone
    /// -Add scene transitions
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS || XBOX
    static class Program
    {
        static EdgeGame game;
        static ParticleEmitter bulletEmitter;
        static ParticleEmitter leftEmitter;
        static ParticleEmitter rightEmitter;

        static void Main(string[] args)
        {
            game = new EdgeGame();
            game.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            game.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            game.Run();
        }

        static void game_OnInit(EdgeGame game)
        {
            game.ClearColor = Color.Black;

            Sprite sprite = new Sprite("player", Vector2.Zero);
            sprite.DrawLayer = 100;
            sprite.AddToGame();
            sprite.AddAction(new ARotateTowards("Rotate", Input.MouseSprite, (90f).ToRadians()));
            sprite.AddAction(new AFollow("Follow", Input.MouseSprite, 7));
            sprite.OnUpdate += new Element.ElementUpdateEvent(updateSprite);
            game.Camera.ClampTo(sprite);

            DebugText Debug = new DebugText("ComicSans-10", Vector2.Zero);
            Debug.FollowsCamera = false;
            Debug.AddToGame();

            ParticleEmitter stars = new ParticleEmitter("Stars", Vector2.Zero)
            {
                BlendState = BlendState.AlphaBlend,
                EmitWait = 0,
                GrowSpeed = 0,
                MaxParticles = 300,
                MinScale = new Vector2(0.5f), MaxScale = new Vector2(1f),
                MinVelocity = new Vector2(-0.1f), MaxVelocity = new Vector2(0.1f),
                MinLife = 1000, MaxLife = 3000,
                MinRotationSpeed = -0.1f, MaxRotationSpeed = 0.1f,
                EmitArea = new Vector2(2000, 2000),
                ColorIndex = new ColorChangeIndex(1000, Color.White, Color.Black, Color.Transparent),
            };
            stars.AddAction(new AClamp(sprite));
            stars.AddToGame();

            bulletEmitter = new ParticleEmitter("Fire", Vector2.Zero)
            {
                BlendState = BlendState.Additive,
                EmitWait = 0,
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
                MinParticlesToEmit = 5, MaxParticlesToEmit = 10,
                MinScale = new Vector2(0.6f), MaxScale = new Vector2(0.8f),
                MinVelocity = new Vector2(-1f), MaxVelocity = new Vector2(1f),
                MinLife = 500, MaxLife = 500,
                MinRotationSpeed = 0, MaxRotationSpeed = 0,
                EmitArea = new Vector2(5, 5),
                MinColorIndex = new ColorChangeIndex(125, Color.Orange, Color.DarkRed, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(125, Color.Red, Color.OrangeRed, Color.Transparent)
            };
            leftEmitter.AddToGame();

            rightEmitter = (ParticleEmitter)leftEmitter.Clone();
            rightEmitter.AddToGame();
        }

        static double elapsed = 0;
        static double toElapse = 200;
        static float bulletspeed = 15;
        static float maxPlayerSpeed = 10;
        static void updateSprite(Element element, GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Vector2.Distance(Input.MousePosition, element.Position)/20;
            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Math.Min(((Sprite)element).GetAction<AFollow>("Follow").Speed, maxPlayerSpeed);

            ((Sprite)element).GetAction<AFollow>("Follow").Paused = !Input.IsKeyDown(Keys.Space);

            if (Input.IsLeftClicking() && elapsed >= toElapse)
            {
                elapsed = 0;
                Sprite sprite = new Sprite("laserGreen", element.Position.PositionRelativeTo(40, ((Sprite)element).Rotation - (180f).ToRadians()));
                sprite.OnUpdate += new Element.ElementUpdateEvent(updateProjectile);
                sprite.Rotation = ((Sprite)element).Rotation;
                Vector2 MoveVector = Input.MousePosition.PositionRelativeTo(40, ((Sprite)element).Rotation - (180f).ToRadians()) - sprite.Position;
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

            leftEmitter.Position = element.Position.PositionRelativeTo(35, ((Sprite)element).Rotation - (180f).ToRadians());
            rightEmitter.Position = element.Position.PositionRelativeTo(35, ((Sprite)element).Rotation);
        }

        static void updateProjectile(Element element, GameTime gameTime)
        {
            if (element.CheckOffScreen())
            {
                element.Remove();
            }
        }

        static void game_OnLoadContent(EdgeGame game)
        {
            game.WindowSize = new Vector2(1000);

            Resources.LoadFont("Fonts/Comic Sans/ComicSans-10");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-20");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-30");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-40");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-50");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-60");
            Resources.LoadFont("Fonts/Courier New/CourierNew-10");
            Resources.LoadFont("Fonts/Courier New/CourierNew-20");
            Resources.LoadFont("Fonts/Courier New/CourierNew-30");
            Resources.LoadFont("Fonts/Courier New/CourierNew-40");
            Resources.LoadFont("Fonts/Courier New/CourierNew-50");
            Resources.LoadFont("Fonts/Courier New/CourierNew-60");
            Resources.LoadFont("Fonts/Georgia/Georgia-10");
            Resources.LoadFont("Fonts/Georgia/Georgia-20");
            Resources.LoadFont("Fonts/Georgia/Georgia-30");
            Resources.LoadFont("Fonts/Georgia/Georgia-40");
            Resources.LoadFont("Fonts/Georgia/Georgia-50");
            Resources.LoadFont("Fonts/Georgia/Georgia-60");
            Resources.LoadFont("Fonts/Impact/Impact-10");
            Resources.LoadFont("Fonts/Impact/Impact-20");
            Resources.LoadFont("Fonts/Impact/Impact-30");
            Resources.LoadFont("Fonts/Impact/Impact-40");
            Resources.LoadFont("Fonts/Impact/Impact-50");
            Resources.LoadFont("Fonts/Impact/Impact-60");
            Resources.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");
            Resources.LoadTexturesInSpritesheet("SpaceSheet", "SpaceSheet");
            //Resources.LoadTexturesInSpritesheet("ButtonSheet", "ButtonSheet");
        }

    }
    #endif
}

