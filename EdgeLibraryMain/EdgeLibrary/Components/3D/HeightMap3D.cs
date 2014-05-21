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

namespace EdgeLibrary
{
    public class HeightMap3D : Sprite3D
    {
        public Vector2 SquareSize { get { return _squareSize; } set { _squareSize = value; ReloadModel(); } }
        private Vector2 _squareSize;
        public float[,] HeightMap { get { return _heightMap; } set { _heightMap = value; ReloadModel(); } }
        private float[,] _heightMap;
        public Color[,] ColorMap { get { return _colorMap; } set { _colorMap = value; ReloadModel(); } }
        private Color[,] _colorMap;

        private int Width;
        private int Length;

        private List<VertexPositionColor> vertices;
        private List<short> indices;

        public HeightMap3D(Vector3 position, Vector2 squareSize, int width, int length) : base(position, new SpriteModel())
        {
            _squareSize = squareSize;
            _heightMap = new float[width, length];
            _colorMap = new Color[width, length];
            Width = width;
            Length = length;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    HeightMap[x, z] = 0;
                    ColorMap[x, z] = Color.White;
                }
            }

            vertices = new List<VertexPositionColor>();
            indices = new List<short>();

            ReloadModel();
        }

        protected void ReloadModel()
        {
            if (HeightMap.Length != Width*Length)
            {
                throw new InvalidOperationException("The size of the arrays were changed");
            }

            vertices.Clear();
            indices.Clear();

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    vertices.Add(new VertexPositionColor(new Vector3(x * SquareSize.X, HeightMap[x, z], - z * SquareSize.Y), ColorMap[x, z]));
                }
            }

            for (int x = 0; x < Width - 1; x++)
            {
                for (int z = 0; z < Length - 1; z++)
                {
                    short lowerLeft = (short)(x + z * Width);
                    short topLeft = (short)((x + 1) + z * Width);
                    short lowerRight = (short)(x + (z + 1) * Width);
                    short topRight = (short)((x + 1) + (z + 1) * Width);

                    indices.AddRange(new short[]{ lowerLeft, topLeft, topRight, topRight, lowerRight, lowerLeft });
                }
            }

            Model = new PrimitiveModel(vertices, indices);
        }
    }
}
