using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EdgeDemo.CheckersService;

namespace EdgeDemo.CheckersGame
{
    /// <summary>
    /// This class manages the board and updates it
    /// </summary>
    public class BoardManager : Sprite
    {
        //Game components to update and draw
        public static List<GameComponent> Components { get; private set; }

        //The main board - static so other classes can access it
        public static Board Board { get; private set; }

        //If set to true, will reset the game when created by GameMenu
        public static bool ResetGame = true;

        //Displays which team should move and the move state
        public TextSprite StatusSprite;

        //Displays how many captures each team has
        public static TextSprite CaptureSprite;

        //The sprite which displays when there is an important message
        public static ColorTextSprite MessageSprite;

        //Displays debug info
        public TextSprite DebugSprite;

        //Is it player 1's turn
        public static bool Player1Turn;

        //The players in the game
        Player Player1;
        Player Player2;

        //Text for the current team
        private string TeamText;
        public int TurnsCount;

        CheckersServiceClient ServiceClient = new CheckersServiceClient();

        public BoardManager()
            : base("", Vector2.Zero)
        {
            //Initializing the board
            Components = new List<GameComponent>();
            Board = new Board(Config.SquareTexture, EdgeGame.WindowSize / 2, Config.BoardSize, Config.SquareSize, Config.SquareDistance, Config.SquareColor1, Config.SquareColor2, Config.BorderSize, Config.BorderColor, Config.PieceTexture, Config.PieceSize, Config.TopColor, Config.BottomColor);
            Components.Add(Board);

            //Initializing the debug sprite
            DebugSprite = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 250)) { Color = Color.Goldenrod, CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false, Include3D = false };
            Components.Add(DebugSprite);

            //Initializing the teamtext
            TeamText = Config.Player1Name + ": ";

            //Initializing status sprite
            StatusSprite = new TextSprite(Config.StatusFont, Config.Player1Name + "'s Turn", Vector2.Zero) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            Components.Add(StatusSprite);

            //Initializing message sprite
            MessageSprite = new ColorTextSprite(Config.BigStatusFont, EdgeGame.WindowSize / 2, new ColorChangeIndex(1000, Color.Transparent, Color.Red, Color.Transparent));
            Components.Add(MessageSprite);

            //Initializing capture sprite
            CaptureSprite = new TextSprite(Config.StatusFont, "Top Team Captures: 0\nBottom Team Captures: 0", new Vector2(0, 50)) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            Components.Add(CaptureSprite);

            //Subscribes to the player events
            Player1.OnRunMove += Player1_OnRunMove;
            Player2.OnRunMove += Player2_OnRunMove;
            TurnsCount = 0;

            //Starts the game off with player 1 moving first
            Player1.ReceivePreviousMove(null, MovementManager.GeneratePlayerMoves(Player1Turn));
        }

        //Necessary override to not draw the BoardManager
        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (DrawableGameComponent component in Components.OfType<DrawableGameComponent>())
                {
                    component.Draw(gameTime);
                }

                Player1.Draw(gameTime);
                Player2.Draw(gameTime);
            }
        }

        //Updates the board
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(GameComponent component in Components)
            {
                component.Update(gameTime);
            }

            Player1.Update(gameTime);
            Player2.Update(gameTime);

            if (Board.mousedOverSquare != null)
            {
                DebugSprite.Text += "Last Moused Over Square: " + Board.mousedOverSquare.X + ", " + Board.mousedOverSquare.Y;
            }

            //TODO - Possibly move because of Players
            if (Config.ThisGameType == Config.GameType.Online && ServiceClient.GetAllGames().ElementAt(Config.ThisGameID).State == GameManager.GameState.WaitingForPlayers)
            {
                System.Windows.Forms.MessageBox.Show("ToDo: Waiting for Players Screen\n(To have this dialog stop appearing, set the current game state to something besides WaitingForPlayers)\n\nThis Game ID: " + Config.ThisGameID);
            }
        }

        void Player1_OnRunMove(Move move)
        {
            RunMove(move);

            StatusSprite.Text = "It is " + Config.Player2Name + "'s Turn";

            Player1Turn = false;
            Player2.ReceivePreviousMove(move, MovementManager.GeneratePlayerMoves(Player1Turn));
        }

        void Player2_OnRunMove(Move move)
        {
            RunMove(move);

            StatusSprite.Text = "It is " + Config.Player1Name + "'s Turn";

            Player1Turn = true;
            Player1.ReceivePreviousMove(move, MovementManager.GeneratePlayerMoves(Player1Turn));
        }

        public void RunMove(Move move)
        {
            foreach (Square square in move.JumpedSquares)
            {
                CapturePiece(square.OccupyingPiece);
            }
        }

        public void CapturePiece(Piece piece)
        {
            //Captures the piece and updates the capture sprite
            Board.CapturePiece(piece);
            CaptureSprite.Text = "Player 1 Captures: " + Board.TopTeamCaptures + "\nPlayer 2 Captures: " + Board.BottomTeamCaptures;
        }

        //Checks if the game should end
        public bool CheckEndGame()
        {
            bool player1HasPieces = false;
            bool player2HasPieces = false;
            foreach (Square square in Board.Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    if (square.OccupyingPiece.Player1)
                    {
                        player1HasPieces = true;
                        continue;
                    }
                    player2HasPieces = true;
                }
            }

            return !(player1HasPieces && player2HasPieces);
        }

        //Ends the game
        public void EndGame()
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Somebody won the game... congratulations. Somebody also lost. (" + (Player1Turn ? Config.Player1Name : Config.Player2Name) + ")", "Somebody lost", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.MessageBox.Show("That is true.", "A false statement", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("That is not true", "A true statement", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
            }
        }
    }
}
