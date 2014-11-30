using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using EdgeLibrary;

namespace EdgeDemo.CheckersGame
{
    public static class Config
    {
        public static string PieceTexture = "Checkers";
        public static string KingTexture = "Crown";
        public static string SquareTexture = "Pixel";
        public static string XTexture = "X";

        public static string StatusFont = "ComicSans-20";
        public static string SquareFont = "ComicSans-20";
        public static string DebugFont = "Impact-20";

        public static string SelectSquare1Message = "Please select a square to move from";
        public static string SelectSquare1MessageFailed = "Please select a valid square to move from";
        public static string SelectSquare2Message = "Please select a square to move to";
        public static string SelectSquare2MessageFailed = "Please select a valid square to move to";
        public static string PassMessage = "You have been forced to pass because you have no possible moves";

        public static string TopTeamName = "Top Team";
        public static string BottomTeamName = "Bottom Team";

        public static float CameraZoomSpeed = 1000f;
        public static float CameraMaxZoom = 10f;
        public static float CameraScrollSpeed = 10f;

        public static Keys MoveCancelKey = Keys.Escape;

        public static int SquareSize = 64;
        public static int BoardSize = 8;

        public static int PieceSize = 54;

        public static int BorderSize = 5;
        public static Color BorderColor = Color.DarkGoldenrod;
        public static int SquareDistance = 0;

        public static float CheckerMoveSpeed = 5f;

        public static float CheckerFadeOutSpeed = 1f;
        public static float CheckerFadeInSpeed = 1f;

        public static float XScale = 0.3f;

        public static Color SquareNumberColor = Color.OrangeRed;
        public static Color SquarePathColor = Color.Gray;

        public static Color SquareColor1 = Color.SaddleBrown;
        public static Color SquareColor2 = Color.Tan;

        public static Color SquareLineColor = Color.DarkRed;
        public static float SquareLineThickness = 7;

        public static Color TopColor = Color.White;
        public static Color BottomColor = Color.DarkGray;

        public static Color Square1SelectColor = Color.Goldenrod;
        public static Color Square2SelectColor = Color.DarkGoldenrod;
    }
}
