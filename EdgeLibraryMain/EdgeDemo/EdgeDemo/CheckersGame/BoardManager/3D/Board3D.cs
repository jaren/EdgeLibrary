using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EdgeDemo.CheckersGame
{
    public class Board3D : Board
    {
        Sprite3D cubeSprite;
        Sprite3D cubeSpriteOutline;

        //Most of the arguments passed to the base constructor SHOULD still work when switched with any value because they're not used in a 3D class
        public Board3D(int size, float squareSize, float squareDistance, Color color1, Color color2, float borderSize, float boardHeight, Color borderColor, Vector3 pieceScale, Color pieceColor1, Color pieceColor2)
            : base("", Vector2.Zero, size, squareSize, squareDistance, color1, color2, borderSize, borderColor, "", 0, pieceColor1, pieceColor2)
        {
            #region Cube
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();
            List<short> vertexIndexes = new List<short>();
            vertices.Add(new VertexPositionColor(new Vector3(-squareSize, -squareSize, squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(-squareSize, squareSize, squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(squareSize, squareSize, squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(squareSize, -squareSize, squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(-squareSize, -squareSize, -squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(-squareSize, squareSize, -squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(squareSize, squareSize, -squareSize), color1));
            vertices.Add(new VertexPositionColor(new Vector3(squareSize, -squareSize, -squareSize), color1));

            Color lineColor = Color.Black;
            List<VertexPositionColor> lineVertices = new List<VertexPositionColor>();
            lineVertices.Add(new VertexPositionColor(new Vector3(-squareSize, -squareSize, squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(-squareSize, squareSize, squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(squareSize, squareSize, squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(squareSize, -squareSize, squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(-squareSize, -squareSize, -squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(-squareSize, squareSize, -squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(squareSize, squareSize, -squareSize), lineColor));
            lineVertices.Add(new VertexPositionColor(new Vector3(squareSize, -squareSize, -squareSize), lineColor));

            //Back   Front
            // 5 6   1 2
            // 4 7   0 3

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

            //Cube Outline
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
            #endregion

            PrimitiveModel cube = new PrimitiveModel(vertices, vertexIndexes);
            PrimitiveModel cubeOutline = new PrimitiveModel(lineVertices, vertexLineIndexes);
            cubeSprite = new Sprite3D(Vector3.Zero, cube);
            cubeSpriteOutline = new Sprite3D(Vector3.Zero, cubeOutline);
        }

        public override void Draw(GameTime gameTime)
        {
            cubeSprite.Draw(gameTime);
            cubeSpriteOutline.Draw(gameTime);
        }
    }
}
