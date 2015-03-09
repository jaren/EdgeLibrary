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
        public static string BigStatusFont = "ComicSans-40";
        public static string SquareFont = "ComicSans-20";
        public static string DebugFont = "Impact-20";

        public static string MenuTitleFont = "Georgia-50";
        public static string MenuSubtitleFont = "Georgia-20";
        public static string MenuButtonTextFont = "Georgia-20";
        public static Color MenuButtonColor = Color.DarkTurquoise;
        public static Color MenuTextColor = Color.White;

        public static string ButtonNormalTexture = "grey_button03";
        public static string ButtonMousedOverTexture = "grey_button02";
        public static string ButtonClickTexture = "grey_button01";

        public static string Player1Name = "Top Team";
        public static string Player2Name = "Bottom Team";

        public static float CameraZoomSpeed = 1000f;
        public static float CameraMaxZoom = 10f;
        public static float CameraScrollSpeed = 10f;

        public static Keys BackKey = Keys.Escape;

        public static int SquareSize = 64;
        public static int BoardSize = 8;

        public static int PieceSize = 54;

        public static int BorderSize = 5;
        public static Color BorderColor = Color.DarkGoldenrod;
        public static int SquareDistance = 0;

        public static float CheckerMoveSpeed = 5f;

        public static float CheckerFadeOutSpeed = 1000f;

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

        public static PlayerType Player1Type = PlayerType.Normal;
        public static PlayerType Player2Type = PlayerType.Normal;

        public static GameType GetGameType()
        {
            Type Player1Type = BoardManager.Player1.GetType();
            Type Player2Type = BoardManager.Player2.GetType();

            if (Player1Type == typeof(NormalPlayer) && Player2Type == typeof(NormalPlayer))
            {
                return GameType.Hotseat;
            }
            else if (Player1Type == typeof(WebPlayer) || Player2Type == typeof(WebPlayer))
            {
                return GameType.Online;
            }
            else if (Player1Type == typeof(ComputerPlayer) && Player2Type == typeof(ComputerPlayer))
            {
                return GameType.AIOnly;
            }
            else /*if((Player1Type == typeof(NormalPlayer) && Player2Type == typeof(ComputerPlayer)) || (Player1Type == typeof(ComputerPlayer) && Player2Type == typeof(NormalPlayer)))*/
            {
                return GameType.Singleplayer;
            }
        }

        public static Player GetWebPlayer()
        {
            if(BoardManager.Player1 is WebPlayer)
            {
                return BoardManager.Player1;
            }
            else
            {
                return BoardManager.Player2;
            }
        }

        public static bool IsHost()
        {
            if (BoardManager.Player1 is WebPlayer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public enum PlayerType
        {
            Normal, 
            Web,
            Computer
        }

        public enum GameType
        {
            Hotseat,
            Online,
            Singleplayer,
            AIOnly
        }
    }
}
