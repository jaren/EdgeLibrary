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
    /// -Fix Text not drawing and 3D bugs
    /// 
    /// BUGS:
    /// 
    /// IMPROVEMENTS:
    /// -In SpriteModel, only set Effect.View and Effect.Projection when the Camera's View or Projection changes
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
            EdgeGame.OnDraw += new EdgeGame.EdgeGameDrawEvent(EdgeGame_OnDraw);
            EdgeGame.Start();
        }

        static void EdgeGame_OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            EdgeGame.Game.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace, FillMode = FillMode.WireFrame };

            EdgeGame.Camera3D.Position = new Vector3(0, 0, Math.Abs(Input.MouseWheelValue) / 100 + 10);
        }

        static void game_OnInit()
        {
            EdgeGame.WindowSize = new Vector2(1000);

            EdgeGame.GameSpeed = 1f;

            EdgeGame.ClearColor = Color.Olive;

            
            #region CUBE
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();
            vertices.Add(new VertexPositionColor(new Vector3(-5, -5, 5), Color.Green));
            vertices.Add(new VertexPositionColor(new Vector3(-5, 5, 5), Color.Purple));
            vertices.Add(new VertexPositionColor(new Vector3(5, 5, 5), Color.Orange));
            vertices.Add(new VertexPositionColor(new Vector3(5, -5, 5), Color.White));
            vertices.Add(new VertexPositionColor(new Vector3(-5, -5, -5), Color.Blue));
            vertices.Add(new VertexPositionColor(new Vector3(-5, 5, -5), Color.Red));
            vertices.Add(new VertexPositionColor(new Vector3(5, 5, -5), Color.Yellow));
            vertices.Add(new VertexPositionColor(new Vector3(5, -5, -5), Color.Black));

            Color lineColor = Color.Black;
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

            PrimitiveModel cubeModel = new PrimitiveModel(vertices, vertexIndexes) { DrawType = PrimitiveType.TriangleList };
            PrimitiveModel cubeOutlineModel = new PrimitiveModel(lineVertices, vertexLineIndexes) { DrawType = PrimitiveType.LineList };

            Sprite3D cube = new Sprite3D(Vector3.Zero, cubeModel);
            cube.AddAction(new ARotate3D(Vector3.One / 100f));
            cube.Scale = Vector3.One /2f;
            //cube.AddToGame();

            Sprite3D cubeOutline = new Sprite3D(Vector3.Zero, cubeOutlineModel);
            cubeOutline.AddAction(new ARotate3D(Vector3.One / 100f));
            cubeOutline.Scale = Vector3.One / 2f;
            //cubeOutline.AddToGame();

            for (int i = 0; i < vertices.Count; i++)
            {
                cube.AddAction(new AColorChange3D(vertices[i], i, new InfiniteColorChangeIndex(Color.White, Color.Transparent, 1, 10000)));
            }
             
            #endregion

            ParticleEmitter Fire = new ParticleEmitter("Fire", Vector2.One * 500)
            {
                BlendState = BlendState.Additive,
                Life = 10000,

                MinScale = new Vector2(1.5f),
                MaxScale = new Vector2(2),

                MinColorIndex = new ColorChangeIndex(1000, Color.Magenta, Color.Orange, Color.Red, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(1000, Color.DarkMagenta, Color.DarkOrange, Color.OrangeRed, Color.Transparent)
            };
            Fire.AddToGame();
            DebugText debug = new DebugText("Impact-20", Vector2.Zero);
            debug.Color = Color.Black;
            debug.AddToGame();
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
            EdgeGame.LoadTexturesInSpritesheet("SpaceSheet", "SpaceSheet");
            EdgeGame.LoadTexturesInSpritesheet("ButtonSheet", "ButtonSheet");
            EdgeGame.LoadTexturesInSpritesheet("ParticleSheet", "ParticleSheet");
        }

    }
#endif
}
