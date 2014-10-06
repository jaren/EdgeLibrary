﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public static class Config
    {
        public static string PieceTexture = "Checkers";
        public static string SquareTexture = "Pixel";

        public static string StatusFont = "ComicSans-20";
        public static string DebugFont = "Impact-20";

        public static string SelectSquare1Message = "Please select a square to move from";
        public static string SelectSquare2Message = "Please select a valid square to move to";

        public static string TopTeamName = "Top Team";
        public static string BottomTeamName = "Bottom Team";

        public static Keys MoveCancelKey = Keys.Escape;

        public static int SquareSize = 64;
        public static int BoardSize = 8;

        public static int PieceSize = 54;

        public static int BorderSize = 5;
        public static Color BorderColor = Color.Goldenrod;
        public static int SquareDistance = 0;

        public static Color Color1 = Color.SaddleBrown;
        public static Color Color2 = Color.Tan;

        public static Color TopColor = Color.Gray;
        public static Color BottomColor = Color.DarkGray;
    }
}
