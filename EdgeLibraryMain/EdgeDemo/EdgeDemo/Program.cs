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
    /// -In SpriteModel, only set Effect.View and Effect.Projection when the Camera's View or Projection changes
    /// 
    /// BUGS:
    /// -Fix SpriteBatch needed to be restart for 2D objects combined with 3D
    /// 
    /// NOTES:
    /// -In Models, the texture is stored at 'Relative Filename: "..\textures\X"'
    /// -Apply Rotation to Vector2 to the XZ plane for camera movement
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS
    static class Program
    {
        static void Main(string[] args)
        {
            EdgeGame.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            EdgeGame.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            EdgeGame.OnUpdate += new EdgeGame.EdgeGameUpdateEvent(EdgeGame_OnUpdate);
            EdgeGame.Start();
        }

        static Vector3 relativeLocation = new Vector3(0, 0, -10);
        static float speed = 2;
        static bool mouseLocked = false;
        static void EdgeGame_OnUpdate(GameTime gameTime)
        {
            #region CameraMovement
            if (Input.IsKeyDown(Keys.W))
            {
                EdgeGame.Camera3D.Position += new Vector3(0, 0, -speed);
            }
            if (Input.IsKeyDown(Keys.S))
            {
                EdgeGame.Camera3D.Position += new Vector3(0, 0, speed);
            }
            if (Input.IsKeyDown(Keys.A))
            {
                EdgeGame.Camera3D.Position += new Vector3(-speed, 0, 0);
            }
            if (Input.IsKeyDown(Keys.D))
            {
                EdgeGame.Camera3D.Position += new Vector3(speed, 0, 0);
            }
            if (Input.IsKeyDown(Keys.Q))
            {
                EdgeGame.Camera3D.Position += new Vector3(0, speed, 0);
            }
            if (Input.IsKeyDown(Keys.E))
            {
                EdgeGame.Camera3D.Position += new Vector3(0, -speed, 0);
            }
            #endregion
            if (mouseLocked && Input.MousePosition != Vector2.One * 500)
            {
                relativeLocation += new Vector3(Input.PreviousMousePosition.Y - Input.MousePosition.Y);
            }
            #region CameraRotation

            EdgeGame.Camera3D.Target = EdgeGame.Camera3D.Position + relativeLocation;
            #endregion

            if (Input.KeyJustPressed(Keys.Escape))
            {
                mouseLocked = !mouseLocked;
            }

            if (mouseLocked)
            {
                Mouse.SetPosition((int)EdgeGame.WindowSize.X / 2, (int)EdgeGame.WindowSize.Y / 2);
                EdgeGame.MouseVisible = false;
            }
            else
            {
                EdgeGame.MouseVisible = true;
            }
        }

        static void game_OnInit()
        {
            EdgeGame.WindowSize = new Vector2(1000);

            EdgeGame.GameSpeed = 1;

            EdgeGame.ClearColor = Color.Black;

            DebugText debug = new DebugText("Impact-20", Vector2.Zero) { Color = Color.White };
            debug.AddToGame();
             
            ParticleEmitter Fire = new ParticleEmitter("Fire", Vector2.One * 500)
            {
                BlendState = BlendState.Additive,
                Life = 5000,

                MinVelocity = new Vector2(-3),
                MaxVelocity = new Vector2(3),

                MinScale = new Vector2(5),
                MaxScale = new Vector2(7),

                MinColorIndex = new ColorChangeIndex(700, Color.Purple, Color.Magenta, Color.Purple, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(700, Color.White, Color.OrangeRed, Color.DarkOrange, Color.Transparent)
            };
            //Fire.AddToGame();

            Sprite3D sprite = new Sprite3D(Vector3.Zero, new GeneratedModel("p1_wedge"));
            sprite.Scale = new Vector3(0.01f);
            sprite.AddToGame();
        }

        public static void game_OnLoadContent()
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
            EdgeGame.LoadModel("Models/p1_wedge");
            EdgeGame.LoadTexturesInSpritesheet("SpaceSheet", "SpaceSheet");
            EdgeGame.LoadTexturesInSpritesheet("ButtonSheet", "ButtonSheet");
            EdgeGame.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");
        }

    }
#endif
}

/* Cube:
    List<VertexPositionColor> vertices = new List<VertexPositionColor>();
    vertices.Add(new VertexPositionColor(new Vector3(-5, -5, 5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(-5, 5, 5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(5, 5, 5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(5, -5, 5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(-5, -5, -5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(-5, 5, -5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(5, 5, -5), Color.Transparent));
    vertices.Add(new VertexPositionColor(new Vector3(5, -5, -5), Color.Transparent));

    Color lineColor = Color.White;
    List<VertexPositionColor> lineVertices = new List<VertexPositionColor>();
    lineVertices.Add(new VertexPositionColor(new Vector3(-5, -5, 5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(-5, 5, 5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(5, 5, 5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(5, -5, 5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(-5, -5, -5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(-5, 5, -5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(5, 5, -5), lineColor));
    lineVertices.Add(new VertexPositionColor(new Vector3(5, -5, -5), lineColor));

    //Back
    // 5 6
    // 4 7

    //Front
    // 1 2
    // 0 3

    List<short> vertexIndexes = new List<short>();
    // Filled Cube
    //Front
    vertexIndexes.AddRange(new short[] { 0, 1, 2 });
    vertexIndexes.AddRange(new short[] { 2, 3, 0 });
    //Back
    vertexIndexes.AddRange(new short[] { 7, 6, 5 });
    vertexIndexes.AddRange(new short[] { 5, 4, 7 });
    //Left
    vertexIndexes.AddRange(new short[] { 4, 5, 1 });
    vertexIndexes.AddRange(new short[] { 1, 0, 4 });
    //Right
    vertexIndexes.AddRange(new short[] { 3, 2, 6 });
    vertexIndexes.AddRange(new short[] { 6, 7, 3 });
    //Top
    vertexIndexes.AddRange(new short[] { 1, 5, 6 });
    vertexIndexes.AddRange(new short[] { 6, 2, 1 });
    //Bottom
    vertexIndexes.AddRange(new short[] { 4, 0, 3 });
    vertexIndexes.AddRange(new short[] { 3, 7, 4 });


    //Line Cube
    List<short> vertexLineIndexes = new List<short>();
    vertexLineIndexes.AddRange(new short[] { 0, 1 });
    vertexLineIndexes.AddRange(new short[] { 1, 2 });
    vertexLineIndexes.AddRange(new short[] { 2, 3 });
    vertexLineIndexes.AddRange(new short[] { 3, 0 });

    vertexLineIndexes.AddRange(new short[] { 4, 5 });
    vertexLineIndexes.AddRange(new short[] { 5, 6 });
    vertexLineIndexes.AddRange(new short[] { 6, 7 });
    vertexLineIndexes.AddRange(new short[] { 7, 4 });

    vertexLineIndexes.AddRange(new short[] { 1, 5 });
    vertexLineIndexes.AddRange(new short[] { 2, 6 });
    vertexLineIndexes.AddRange(new short[] { 0, 4 });
    vertexLineIndexes.AddRange(new short[] { 3, 7 });
*/