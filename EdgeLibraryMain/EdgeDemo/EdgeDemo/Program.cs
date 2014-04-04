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
    /// -Make camera scaling based off center, not top left
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
            game.Camera.Position = new Vector2(500);

            TextSprite Info1 = new TextSprite("ComicSans-30", "This is a demo of EdgeLibrary.\nUse the WASD keys to pan the camera.\nUse the mouse wheel to zoom in and out.", new Vector2(10));
            Info1.Color = Color.Goldenrod;
            Info1.CenterAsOrigin = false;
            Info1.AddToGame();

            ParticleEmitter emitter = new ParticleEmitter("fire", new Vector2(400, 400));
            emitter.Position = game.WindowSize / 2;
            emitter.SetScale(new Vector2(5), new Vector2(7));
            emitter.SetVelocity(new Vector2(-5), new Vector2(5));
            emitter.BlendState = BlendState.Additive;
            emitter.SetLife(5000);
            emitter.EmitWait = 0;
            emitter.SetEmitArea(0, 0);
            ColorChangeIndex index = new ColorChangeIndex(700, Color.White, Color.Orange, Color.Purple, Color.Orange, Color.Purple, Color.Transparent);
            emitter.SetColor(index);
            emitter.AddToGame();

            game.OnUpdate += new EdgeGame.EdgeGameUpdateEvent(game_OnUpdate);
        }

        static void game_OnUpdate(GameTime gameTime, EdgeGame game)
        {
            int speed = 4;
            if (Input.IsKeyDown(Keys.W))
            {
                game.Camera.Position += new Vector2(0, -speed);
            }
            if (Input.IsKeyDown(Keys.A))
            {
                game.Camera.Position += new Vector2(-speed, 0);
            }
            if (Input.IsKeyDown(Keys.S))
            {
                game.Camera.Position += new Vector2(0, speed);
            }
            if (Input.IsKeyDown(Keys.D))
            {
                game.Camera.Position += new Vector2(speed, 0);
            }
            game.Camera.Scale = Input.MouseWheelValue/1500f + 1;
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
        }

    }
    #endif
}

