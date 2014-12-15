using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EdgeLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame
{
    public class Game
    {
        public void OnInit()
        {
            EdgeGame.InitializeWorld(new Vector2(0, 9.8f));

            EdgeGame.WindowSize = new Vector2(1000);

            EdgeGame.GameSpeed = 1;

            EdgeGame.ClearColor = Color.Gray;

            EdgeGame.IsShuffled = true;

            //EdgeGame.playPlaylist("Music");

            EdgeGame.OnUpdate += OnUpdate;
            EdgeGame.OnDraw += OnDraw;

            MenuManager.Init();
            MenuManager.AddMenu(new MainMenu());
            MenuManager.AddMenu(new GameMenu());
            MenuManager.SwitchMenu("MainMenu");
        }

        public void OnUpdate(GameTime gameTime)
        {
            
            EdgeGame.Camera.Scale += (Input.MouseWheelValue - Input.PreviousMouseWheelValue) / Config.CameraZoomSpeed;
            if (EdgeGame.Camera.Scale > Config.CameraMaxZoom)
            {
                EdgeGame.Camera.Scale = Config.CameraMaxZoom;
            }
            if (Input.JustMiddleClicked())
            {
                EdgeGame.Camera.Scale = 1;
            }

            if (Input.IsKeyDown(Keys.Left) && EdgeGame.Camera.Position.X >= Board.BoardArea.Left)
            {
                EdgeGame.Camera.Position -= new Vector2(Config.CameraScrollSpeed / EdgeGame.Camera.Scale, 0);
            }
            if (Input.IsKeyDown(Keys.Right) && EdgeGame.Camera.Position.X <= Board.BoardArea.Right)
            {
                EdgeGame.Camera.Position += new Vector2(Config.CameraScrollSpeed / EdgeGame.Camera.Scale, 0);
            }
            if (Input.IsKeyDown(Keys.Down) && EdgeGame.Camera.Position.Y <= Board.BoardArea.Bottom)
            {
                EdgeGame.Camera.Position += new Vector2(0, Config.CameraScrollSpeed / EdgeGame.Camera.Scale);
            }
            if (Input.IsKeyDown(Keys.Up) && EdgeGame.Camera.Position.Y >= Board.BoardArea.Top)
            {
                EdgeGame.Camera.Position -= new Vector2(0, Config.CameraScrollSpeed / EdgeGame.Camera.Scale);
            }
             

            MenuManager.Update(gameTime);
        }

        public void OnDraw(GameTime gameTime)
        {
            MenuManager.Draw(gameTime);
        }

        public void OnLoadContent()
        {
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
            EdgeGame.LoadTexturesInSpritesheet("SpaceSheet", "SpaceSheet");
            EdgeGame.LoadTexturesInSpritesheet("ButtonSheet", "ButtonSheet");
            EdgeGame.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");

            EdgeGame.LoadTexture(Config.PieceTexture);
            EdgeGame.LoadTexture(Config.KingTexture);
            EdgeGame.LoadTexture(Config.XTexture);

            EdgeGame.LoadSong("Music/Carefree");
            EdgeGame.LoadSong("Music/Fig Leaf Times Two");
            EdgeGame.LoadSong("Music/Fun in a Bottle");
            EdgeGame.LoadSong("Music/Hyperfun");
            EdgeGame.LoadSong("Music/Run Amok");
            EdgeGame.LoadSong("Music/Wallpaper");
            EdgeGame.AddPlaylist("Music", "Carefree", "Fig Leaf Times Two", "Fun in a Bottle", "Hyperfun", "Run Amok", "Wallpaper");
        }

    }
}
