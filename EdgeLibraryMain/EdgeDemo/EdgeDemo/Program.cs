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
    /// -Add scene transitions
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS || XBOX
    static class Program
    {
        static EdgeGame game;

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
            sprite.AddAction(new ARotateTowards("Rotate", Input.MouseSprite, 90));
            sprite.AddAction(new AFollow("Follow", Input.MouseSprite, 7));
            sprite.OnUpdate += new Element.ElementUpdateEvent(updateSprite);
            game.Camera.ClampTo(sprite);

            DebugText Debug = new DebugText("ComicSans-10", Vector2.Zero);
            Debug.FollowsCamera = false;
            Debug.AddToGame();

            ParticleEmitter emitter = new ParticleEmitter("Stars", new Vector2(400, 400));
            emitter.Position = game.WindowSize / 2;
            emitter.SetScale(new Vector2(0.5f), new Vector2(1));
            emitter.SetVelocity(new Vector2(-1), new Vector2(1));
            emitter.BlendState = BlendState.AlphaBlend;
            emitter.SetLife(3000);
            emitter.EmitWait = 0;
            emitter.SetRotationSpeed(-1, 1);
            emitter.GrowSpeed = 0;
            emitter.MaxParticles = 300;
            emitter.SetEmitArea(2000, 2000);
            ColorChangeIndex index = new ColorChangeIndex(1000, Color.White, Color.Black, Color.Transparent);
            emitter.SetColor(index);
            emitter.AddAction(new AClamp(sprite));
            emitter.AddToGame();
        }

        static double elapsed = 0;
        static double toElapse = 200;
        static float bulletspeed = 20;
        static float maxPlayerSpeed = 15;
        static void updateSprite(Element element, GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Vector2.Distance(Input.MousePosition, element.Position)/20;
            ((Sprite)element).GetAction<AFollow>("Follow").Speed = Math.Min(((Sprite)element).GetAction<AFollow>("Follow").Speed, maxPlayerSpeed);

            ((Sprite)element).GetAction<AFollow>("Follow").Paused = !Input.IsKeyDown(Keys.Space);

            if (Input.IsLeftClicking() && elapsed >= toElapse)
            {
                elapsed = 0;
                Sprite sprite = new Sprite("laserGreen", element.Position.PositionRelativeTo(40, ((Sprite)element).Rotation - 180));
                sprite.OnUpdate += new Element.ElementUpdateEvent(updateProjectile);
                sprite.Rotation = ((Sprite)element).Rotation;
                Vector2 MoveVector = Input.MousePosition.PositionRelativeTo(40, ((Sprite)element).Rotation - 180) - sprite.Position;
                MoveVector.Normalize();
                sprite.AddAction(new AMove(MoveVector * bulletspeed));
                element.AddSubElement(sprite);

                Sprite sprite2 = (Sprite)sprite.Clone();
                sprite2.Position = element.Position.PositionRelativeTo(40, ((Sprite)element).Rotation);
                element.AddSubElement(sprite2);
            }
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

