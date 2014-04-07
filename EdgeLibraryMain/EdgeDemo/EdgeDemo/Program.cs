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

            Sprite sprite = new Sprite("player", Vector2.One * 500);
            sprite.AddToGame();
            sprite.AddAction(new ARotate(Input.MouseSprite, 90));
            sprite.AddAction(new AFollow(Input.MouseSprite, 7));
            sprite.OnUpdate += new Element.ElementUpdateEvent(updateSprite);
            game.Camera.ClampTo(sprite);

            DebugText Debug = new DebugText("ComicSans-10", Vector2.Zero);
            Debug.FollowsCamera = false;
            Debug.AddToGame();

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
        }

        static void updateSprite(Element element, GameTime gameTime)
        {
            ((Sprite)element).RemoveAction(1);
            ((Sprite)element).AddAction(new AFollow(Input.MouseSprite, Vector2.Distance(Input.MousePosition, element.Position)/10));
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

