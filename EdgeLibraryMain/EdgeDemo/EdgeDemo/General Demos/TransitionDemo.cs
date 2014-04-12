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
#if WINDOWS || XBOX
    public static class TransitionDemo
    {
        public static EdgeGame Game;

        public static void Main(string[] args)
        {
            Game = new EdgeGame();
            Game.OnInit += new EdgeGame.EdgeGameEvent(Game_OnInit);
            Game.OnLoadContent += new EdgeGame.EdgeGameEvent(Game_OnLoadContent);
            Game.Run();
        }

        private static void Game_OnInit(EdgeGame game)
        {
            game.ClearColor = Color.White;

            Scene scene1 = new Scene("1");
            game.SceneHandler.SwitchScene(scene1);
            Sprite pixel1 = new Sprite("Pixel", Vector2.One * 500);
            pixel1.Scale = Vector2.One * 500;

            Scene scene2 = new Scene("2");
            game.SceneHandler.SwitchScene(scene2);
            scene2.Background = Resources.GetTexture("player");

            FadeTransition fade = new FadeTransition("fade", scene1, scene2, 10000);
            game.SceneHandler.SwitchScene(fade);
        }

        private static void Game_OnLoadContent(EdgeGame game)
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
