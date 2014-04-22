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
    /// 
    /// BUGS:
    /// -Mouse position is affected by camera rotation
    /// -Particles randomly appear at (0, 0)
    /// -Particle emitters emit extremely quickly regardless of game speed
    /// -SubElements not copied in clone
    /// -Transitions are EXTREMELY expensive
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            EdgeGame.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            EdgeGame.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            EdgeGame.Start();
        }

        static void game_OnInit()
        {
            EdgeGame.GameSpeed = 1f;

            EdgeGame.ClearColor = Color.Black;

            Scene blank = new Scene("blank");
            EdgeGame.AddScene(blank);
            EdgeGame.SwitchScene("blank");
            Sprite sprite = new Sprite("Pixel", Vector2.One * 500);
            sprite.Scale = Vector2.One * 500;
            blank.AddElement(sprite);
        }

        static void game_OnLoadContent()
        {
            EdgeGame.WindowSize = new Vector2(1000);

            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-10");
            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-20");
            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-30");
            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-40");
            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-50");
            EdgeGame.LoadFont("Fonts/Comic Sans/ComicSans-60");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-10");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-20");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-30");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-40");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-50");
            EdgeGame.LoadFont("Fonts/Courier New/CourierNew-60");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-10");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-20");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-30");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-40");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-50");
            EdgeGame.LoadFont("Fonts/Georgia/Georgia-60");
            EdgeGame.LoadFont("Fonts/Impact/Impact-10");
            EdgeGame.LoadFont("Fonts/Impact/Impact-20");
            EdgeGame.LoadFont("Fonts/Impact/Impact-30");
            EdgeGame.LoadFont("Fonts/Impact/Impact-40");
            EdgeGame.LoadFont("Fonts/Impact/Impact-50");
            EdgeGame.LoadFont("Fonts/Impact/Impact-60");
            EdgeGame.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");
            EdgeGame.LoadTexturesInSpritesheet("SpaceSheet", "SpaceSheet");
            //EdgeGame.LoadTexturesInSpritesheet("ButtonSheet", "ButtonSheet");
        }

    }
#endif
}
