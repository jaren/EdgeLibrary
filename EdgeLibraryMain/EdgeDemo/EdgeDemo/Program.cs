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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace EdgeDemo
{
    /// <summary>
    /// TODO:
    /// -Fix HeightMap
    /// -Add scaling into physics bodies
    /// 
    /// BUGS:
    /// -Fix SpriteBatch needed to be restart for 2D objects combined with 3D
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS
    static class Program
    {
        static ParticleEmitter Fire;

        static void Main(string[] args)
        {
            EdgeGame.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            EdgeGame.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            EdgeGame.OnUpdate += new EdgeGame.EdgeGameUpdateEvent(EdgeGame_OnUpdate);
            EdgeGame.Start();
        }

        static int speed = 10;
        static void EdgeGame_OnUpdate(GameTime gameTime)
        {
            float actualSpeed = speed / EdgeGame.Camera.Scale;

            if (Input.IsLeftClicking())
            {
                Fire.Position = Input.MousePosition;
                Fire.ShouldEmit = true;
            }
            else
            {
                Fire.ShouldEmit = false;
            }

            if (Input.IsKeyDown(Keys.Up))
            {
                EdgeGame.Camera.Position += new Vector2(0, -actualSpeed);
            }
            if (Input.IsKeyDown(Keys.Down))
            {
                EdgeGame.Camera.Position += new Vector2(0, actualSpeed);
            }
            if (Input.IsKeyDown(Keys.Right))
            {
                EdgeGame.Camera.Position += new Vector2(actualSpeed, 0);
            }
            if (Input.IsKeyDown(Keys.Left))
            {
                EdgeGame.Camera.Position += new Vector2(-actualSpeed, 0);
            }

            if (Input.IsKeyDown(Keys.Q))
            {
                EdgeGame.Camera.Scale *= 0.95f;
            }
            if (Input.IsKeyDown(Keys.W))
            {
                EdgeGame.Camera.Scale *= 1.05f;
            }
        }

        static void game_OnInit()
        {

            EdgeGame.InitializeWorld(new Vector2(0, 9.8f));

            EdgeGame.WindowSize = new Vector2(1000);

            EdgeGame.GameSpeed = 1;

            EdgeGame.ClearColor = Color.Black;

            DebugText debug = new DebugText("Impact-20", new Vector2(5)) { Color = Color.White };
            debug.FollowsCamera = true;
            debug.ScaleWithCamera = true;
            debug.AddToGame();
             
            Fire = new ParticleEmitter("meteorSmall", new Vector2(500))
            {
                BlendState = BlendState.AlphaBlend,
                Life = 6000,

                EmitPositionVariance = new Vector2(0, 0),

                MinVelocity = new Vector2(-80),
                MaxVelocity = new Vector2(80),

                MinScale = new Vector2(0.5f),
                MaxScale = new Vector2(3),

                ColorIndex = new ColorChangeIndex(6000, Color.White, Color.Transparent), //MinColorIndex = new ColorChangeIndex(700, Color.Purple, Color.Magenta, Color.Purple, Color.Transparent),
                                                                                         //MaxColorIndex = new ColorChangeIndex(700, Color.White, Color.OrangeRed, Color.DarkOrange, Color.Transparent),
                EmitWait = 10,

                ShouldEmit = false
            };
            Fire.OnEmit += new ParticleEmitter.ParticleEventHandler(Fire_OnEmit);
            Fire.AddToGame();

            Sprite platform = new Sprite("Pixel", new Vector2(500, 1000)) { Scale = new Vector2(1000, 10), Color = Color.Gray };
            platform.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (platform.Width * platform.Scale.X).ToSimUnits(), (platform.Height * platform.Scale.Y).ToSimUnits(), 1));
            platform.Body.BodyType = BodyType.Static;
            platform.Body.CollisionCategories = Category.All;
            platform.AddToGame();

            Sprite platformTop = (Sprite)platform.Clone();
            platformTop.Position = new Vector2(500, 0);
            platformTop.AddToGame();

            Sprite platformLeft = (Sprite)platform.Clone();
            platformLeft.Position = new Vector2(0, 500);
            platformLeft.Body.Rotation = 90.ToRadians();
            platformLeft.AddToGame();

            Sprite platformRight = (Sprite)platformLeft.Clone();
            platformRight.Position = new Vector2(1000, 500);
            platformRight.AddToGame();
        }

        static void Fire_OnEmit(ParticleEmitter sender, Sprite particle, GameTime gameTime)
        {
            particle.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (particle.Width / 2 * particle.Scale.X).ToSimUnits(), 1));
            particle.Body.BodyType = BodyType.Dynamic;
            particle.Body.Restitution = 0.5f;
            if (particle.Body.Mass < 0.5f)
            {
                particle.Body.Mass = 0.5f;
            }
            particle.Body.CollisionCategories = Category.Cat1;
            particle.Body.CollidesWith = Category.All;

            Vector2 force = new Vector2(float.Parse(particle.Data["Velocity"].Split(',')[0]), float.Parse(particle.Data["Velocity"].Split(',')[1]));
            particle.Body.ApplyForce(force, particle.Body.WorldCenter);
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