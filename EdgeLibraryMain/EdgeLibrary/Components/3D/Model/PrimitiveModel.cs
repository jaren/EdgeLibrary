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
    /// <summary>
    /// A simple colored shape
    /// </summary>
    public class PrimitiveModel : SpriteModel
    {
        //Used for rendering the model
        protected VertexBuffer VertexBuffer;
        protected IndexBuffer IndexBuffer;

        public PrimitiveType DrawType;

        public List<VertexPositionColor> Vertices { get { return _vertices; } protected set { _vertices = value; ReloadVerticesArray(); } }
        private List<VertexPositionColor> _vertices;

        public List<short> VertexIndexes { get { return _vertexIndexes; } protected set { _vertexIndexes = value; ReloadVerticesArray(); } }
        private List<short> _vertexIndexes;

        protected VertexPositionColor[] VertexArray;
        protected short[] VertexIndexArray;

        public PrimitiveModel(List<VertexPositionColor> vertices, List<short> vertexIndexes)
            : base()
        {
            SetVertices(vertices, vertexIndexes);
            DrawType = PrimitiveType.TriangleList;
        }

        public PrimitiveModel(List<VertexPositionColor> vertices)
            : base()
        {
            SetVertices(vertices);
            DrawType = PrimitiveType.TriangleList;
        }

        /// <summary>
        /// Sets the vertices using a list of vertices and a list of indexes to access the vertices at
        /// </summary>
        public void SetVertices(List<VertexPositionColor> vertices, List<short> vertexIndexes)
        {
            _vertices = new List<VertexPositionColor>(vertices);
            _vertexIndexes = new List<short>(vertexIndexes);

            ReloadVerticesArray();

            VertexBuffer = new VertexBuffer(EdgeGame.Game.GraphicsDevice, typeof(VertexPositionColor), Vertices.Count, BufferUsage.None);
            VertexBuffer.SetData(VertexArray);

            IndexBuffer = new IndexBuffer(EdgeGame.Game.GraphicsDevice, IndexElementSize.SixteenBits, VertexIndexes.Count, BufferUsage.None);
            IndexBuffer.SetData(VertexIndexArray);
        }

        /// <summary>
        /// Sets a single vertex
        /// </summary>
        public void SetVertexAt(VertexPositionColor vertex, int index)
        {
            _vertices[index] = vertex;
            ReloadVerticesArray();
            VertexBuffer.SetData(VertexArray);
            IndexBuffer.SetData(VertexIndexArray);
        }

        /// <summary>
        /// Sets the vertices using a list of vertices; generates a list of indexes
        /// </summary>
        public void SetVertices(List<VertexPositionColor> vertices)
        {
            List<short> indexes = new List<short>();
            for (short i = 0; i < vertices.Count; i++)
            {
                indexes.Add(i);
            }
            SetVertices(vertices, indexes);
        }

        /// <summary>
        /// Sets the vertex arrays to the vertex lists
        /// </summary>
        private void ReloadVerticesArray()
        {
            VertexArray = Vertices.ToArray();
            VertexIndexArray = VertexIndexes.ToArray();
        }

        /// <summary>
        /// Gets the number of primitives to draw
        /// </summary>
        private int GetPrimitiveCount()
        {
            switch (DrawType)
            {
                case PrimitiveType.TriangleList:
                    return VertexIndexes.Count / 3;
                case PrimitiveType.TriangleStrip:
                    return 1 + ((VertexIndexes.Count - 3) / 2);
                case PrimitiveType.LineList:
                    return VertexIndexes.Count / 2;
                case PrimitiveType.LineStrip:
                    return VertexIndexes.Count - 1;
            }
            return 0;
        }

        /// <summary>
        /// Draws the model
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            EdgeGame.Game.GraphicsDevice.SetVertexBuffer(VertexBuffer);
            EdgeGame.Game.GraphicsDevice.Indices = IndexBuffer;

            //Loops through the passes in the effect and draws to the graphics device
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                EdgeGame.Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(DrawType, VertexArray, 0, Vertices.Count, VertexIndexArray, 0, GetPrimitiveCount());
            }
        }

        public override object Clone()
        {
            PrimitiveModel clone = (PrimitiveModel)base.Clone();
            clone.VertexBuffer = new VertexBuffer(EdgeGame.Game.GraphicsDevice, typeof(VertexPositionColor), Vertices.Count, BufferUsage.None);
            clone.VertexBuffer.SetData(VertexArray);

            clone.IndexBuffer = new IndexBuffer(EdgeGame.Game.GraphicsDevice, IndexElementSize.SixteenBits, VertexIndexes.Count, BufferUsage.None);
            clone.IndexBuffer.SetData(VertexIndexArray);
            return clone;
        }
    }
}
