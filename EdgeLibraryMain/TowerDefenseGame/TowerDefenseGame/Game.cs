using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EdgeLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace TowerDefenseGame
{
    public class Game
    {
        public void OnInit()
        {
            EdgeGame.InitializeWorld(new Vector2(0, 0));

            EdgeGame.GameSpeed = 1;

            EdgeGame.ClearColor = new Color(10, 10, 10);

            EdgeGame.IsShuffled = true;

            OptionsMenu.FullscreenOn = false;
            OptionsMenu.MusicOn = false;
            OptionsMenu.ParticlesOn = true;
            OptionsMenu.SoundEffectsOn = true;

            EdgeGame.OnUpdate += OnUpdate;
            EdgeGame.OnDraw += OnDraw;
            EdgeGame.OnResized += EdgeGame_OnResized;

            MenuManager.Init();
            MenuManager.AddMenu(new MainMenu());
            MenuManager.AddMenu(new CreditsMenu());
            MenuManager.AddMenu(new GameMenu());
            MenuManager.AddMenu(new GameSelectMenu());
            MenuManager.AddMenu(new OptionsMenu());
            MenuManager.SwitchMenu("MainMenu");
        }

        void EdgeGame_OnResized()
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(GameTime gameTime)
        {
            MenuManager.Update(gameTime);
        }

        public void OnDraw(GameTime gameTime)
        {
            MenuManager.Draw(gameTime);
        }

        public void OnLoadContent()
        {
            //Window size must be set here for the credits render target
            EdgeGame.WindowSize = new Vector2(1024);

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
            EdgeGame.LoadBitmapFont("Fonts/KenVector/kenvector_future_regular_32", "Fonts/KenVector/kenvector_future_regular_32");
            EdgeGame.LoadBitmapFont("Fonts/windsong_regular_65", "Fonts/windsong_regular_65");
            EdgeGame.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");
            EdgeGame.LoadTexturesInSpritesheet("GUI/ButtonSheet", "GUI/ButtonSheet");
            EdgeGame.LoadTexturesInSpritesheet("GUI/GreyGUI", "GUI/GreyGUI");
            EdgeGame.LoadTexturesInSpritesheet("GUI/WhiteIcons2x", "GUI/WhiteIcons2x");
            EdgeGame.LoadTexturesInSpritesheet("GUI/UIRPG", "GUI/UIRPG");
            EdgeGame.LoadTexture("Levels/Grassy Plains");
            EdgeGame.LoadTexture("Levels/Islands");
            EdgeGame.LoadTexture("Levels/Rocky Bridges");
            EdgeGame.LoadTexture("Levels/Village Loop");
            EdgeGame.LoadTexture("Levels/Around the Lake");
            EdgeGame.LoadTexture("Levels/Return to the Village");
            EdgeGame.LoadTexture("Levels/Grassy Plains Preview");
            EdgeGame.LoadTexture("Levels/Islands Preview");
            EdgeGame.LoadTexture("Levels/Rocky Bridges Preview");
            EdgeGame.LoadTexture("Levels/Village Loop Preview");
            EdgeGame.LoadTexture("Levels/Around the Lake Preview");
            EdgeGame.LoadTexture("Levels/Return to the Village Preview");
            EdgeGame.LoadTexture("Tower1");
            EdgeGame.LoadTexture("Tower2");
            EdgeGame.LoadTexture("Tower3");
            EdgeGame.LoadTexture("Circle");

            EdgeGame.LoadSong("Music/Hyperfun");
            EdgeGame.LoadSong("Music/The Curtain Rises");
            EdgeGame.AddPlaylist("Music", "Hyperfun");
            EdgeGame.AddPlaylist("TitleMusic", "The Curtain Rises");

            //Creating the Credits 'particle' text
            RenderTargetImager imager = new RenderTargetImager();
            imager.Components.Add(new TextSprite(Config.MenuTitleFont, "Credits", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.05f)) { Color = Color.White });
            imager.Components.Add(new TextSprite(Config.MenuTitleFont, "Jaren", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Color.White });
            imager.Components.Add(new TextSprite(Config.MenuTitleFont, "Aaron", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Color.White });
            imager.Components.Add(new TextSprite("Georgia-40", "Incompetech", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.75f)) { Color = Color.White });
            imager.Components.Add(new TextSprite(Config.MenuTitleFont, "GMR", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.75f)) { Color = Color.White });
            Texture2D creditsTexture = imager.RenderToTarget(Color.Black);
            TextureEditor editor = new TextureEditor();
            editor.OnEditPixel += editor_OnEditPixel;
            editor.ApplyTo(creditsTexture);
            EdgeGame.AddTexture("CreditsTexture", creditsTexture.Clone());
        }

        private void editor_OnEditPixel(ref Color color, int x, int y)
        {
            if (color != Color.Black)
            {
                color = Color.Transparent;
            }
        }
    }
}
