using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class GameMenu : MenuBase
    {
        public static Level CurrentLevel;
        public static Difficulty Difficulty;
        public static bool ShouldReset;
        public RoundManager RoundManager;

        public int Lives
        {
            get { return lives; }
            set { lives = value; LivesNumber.Text = lives.ToString(); }
        }
        private int lives;
        public int Money
        {
            get { return money; }
            set { money = value; MoneyNumber.Text = money.ToString(); }
        }
        private int money;

        public TextSprite RoundNumber;
        public TextSprite RoundText;
        public TextSprite LivesNumber;
        public TextSprite LivesText;
        public TextSprite MoneyNumber;
        public TextSprite MoneyText;

        public Button NextRoundButton;

        public List<Button> TowerButtons;
        public List<Sprite> TowerSprites;
        public List<TextSprite> TowerCostSprites;
        public TextSprite TowerInfoSprite;

        public List<Tower> Towers;
        TowerPanel TowerPanel;

        public List<Enemy> Enemies;
        private List<Enemy> EnemiesToRemove;

        public Button FloatingTower;
        public TowerData SelectedTower;
        public Sprite FloatingRange;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
            ShouldReset = false;
        }

        public override void SwitchTo()
        {
            if (ShouldReset)
            {
                ShouldReset = false;

                Components.Clear();

                RoundManager = new RoundManager(Config.RoundList);
                RoundManager.OnEmitEnemy += RoundManager_OnEmitEnemy;
                RoundManager.OnFinish += RoundManager_OnFinish;

                Vector2 CommonRatio = new Vector2(0.85f);

                Towers = new List<Tower>();

                Enemies = new List<Enemy>();
                EnemiesToRemove = new List<Enemy>();

                CurrentLevel.Position = new Vector2(EdgeGame.WindowSize.X * 0.5f * CommonRatio.X, EdgeGame.WindowSize.Y * 0.5f * CommonRatio.Y);
                CurrentLevel.ResizeLevel(EdgeGame.WindowSize * CommonRatio);
                Components.Add(CurrentLevel);

                RoundText = new TextSprite(Config.MenuSubtitleFont, "ROUND", new Vector2(EdgeGame.WindowSize.X * (CommonRatio.X + (1f - CommonRatio.X) / 2f), EdgeGame.WindowSize.Y * 0.05f));
                Components.Add(RoundText);

                RoundNumber = new TextSprite("Georgia-60", "0", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.09f));
                Components.Add(RoundNumber);

                LivesText = new TextSprite(Config.MenuSubtitleFont, "LIVES", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.2f));
                Components.Add(LivesText);

                LivesNumber = new TextSprite("Georgia-60", Lives.ToString(), new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.24f));
                Components.Add(LivesNumber);

                MoneyText = new TextSprite(Config.MenuSubtitleFont, "MONEY", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.35f));
                Components.Add(MoneyText);

                MoneyNumber = new TextSprite("Georgia-60", Money.ToString(), new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.39f));
                Components.Add(MoneyNumber);

                NextRoundButton = new Button("ShadedDark25", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.6f)) { Color = Color.White, Scale = new Vector2(1f) };
                NextRoundButton.OnRelease += (x, y) =>
                {
                    if (!RoundManager.RoundRunning && Enemies.Count == 0) 
                    {
                        RoundManager.StartRound(); 
                    }
                };
                NextRoundButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
                NextRoundButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
                NextRoundButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
                NextRoundButton.Style.AllColors = Color.White;
                Components.Add(NextRoundButton);

                FloatingRange = new Sprite("Circle", Vector2.Zero);
                Components.Add(FloatingRange);
                FloatingRange.Visible = false;
                FloatingRange.Enabled = false;

                FloatingTower = new Button("Pixel", Vector2.Zero);
                FloatingTower.OnClick += FloatingTower_OnClick;
                Components.Add(FloatingTower);
                FloatingTower.Visible = false;
                FloatingTower.Enabled = false;

                TowerButtons = new List<Button>();
                TowerSprites = new List<Sprite>();
                TowerCostSprites = new List<TextSprite>();

                Vector2 StartPosition = new Vector2(EdgeGame.WindowSize.X * 0.075f, EdgeGame.WindowSize.Y * (CommonRatio.Y + (1f - CommonRatio.Y) / 2f));
                float xStep = 0.1f;
                float towerYAdd = -15;
                float towerYMin = 20;
                for (int i = 0; i < Config.Towers.Count; i++)
                {
                    Button towerButton = new Button("panelInset_beige", new Vector2(StartPosition.X + EdgeGame.WindowSize.X * (xStep * i), StartPosition.Y)) { Scale = new Vector2(1f) };
                    towerButton.ID = String.Format("{0}_TowerButton", i);
                    towerButton.Style.NormalTexture = EdgeGame.GetTexture("panelInset_beige");
                    towerButton.Style.MouseOverTexture = EdgeGame.GetTexture("panelInset_beige");
                    towerButton.Style.ClickTexture = EdgeGame.GetTexture("panelInset_beige");
                    towerButton.Style.AllColors = Color.White;
                    towerButton.OnMouseOver += towerButton_OnMouseOver;
                    towerButton.OnMouseOff += towerButton_OnMouseOff;
                    towerButton.OnClick += towerButton_OnClick;
                    TowerButtons.Add(towerButton);

                    Sprite towerSprite = new Sprite(Config.Towers[i].Texture, new Vector2(towerButton.Position.X, towerButton.Position.Y + towerYAdd));
                    TowerSprites.Add(towerSprite);

                    TextSprite towerCostSprite = new TextSprite(Config.MenuButtonTextFont, Config.Towers[i].Cost.ToString(), new Vector2(towerButton.Position.X, towerButton.Position.Y + towerYMin));
                    TowerCostSprites.Add(towerCostSprite);
                }

                TowerInfoSprite = new TextSprite(Config.MenuButtonTextFont, "Description:\nNONE", new Vector2(EdgeGame.WindowSize.X * (CommonRatio.X + (1f - CommonRatio.X) / 2f) - EdgeGame.WindowSize.X * 0.3f, EdgeGame.WindowSize.Y * (CommonRatio.Y + (1f - CommonRatio.Y) / 2f)));
                Components.Add(TowerInfoSprite);

                TowerPanel = new TowerPanel();
                Components.Add(TowerPanel);

                //Must be initialized after the text, otherwise they will be null
                Lives = Config.LivesNumber[(int)Difficulty];
                Money = Config.StartingMoneyNumber[(int)Difficulty];
            }

            EdgeGame.ClearColor = Color.Gray;

            base.SwitchTo();
        }

        public void LoseGame()
        {
            //Add lose game stuff here
        }

        public void WinGame()
        {
            //Add win game stuff here
        }

        void RoundManager_OnFinish(Round round)
        {
            WinGame();
        }

        void RoundManager_OnEmitEnemy(Round round, EnemyData enemyData)
        {
            Waypoint randomStartingWaypoint = CurrentLevel.Waypoints.GetRandomStartingWaypoint();
            Enemy enemy = new Enemy(enemyData, randomStartingWaypoint.Position);
            enemy.OnReachWaypoint += enemy_OnReachWaypoint;
            if (enemy.EnemyData.SpecialActionsOnCreate != null)
            {
                enemy.EnemyData.SpecialActionsOnCreate(enemy);
            }
            //Sets the next waypoint
            enemy_OnReachWaypoint(enemy, randomStartingWaypoint);
            Enemies.Add(enemy);
        }

        void enemy_OnReachWaypoint(Enemy enemy, Waypoint waypoint)
        {
            List<Waypoint> nextWaypoints = CurrentLevel.Waypoints.NextWaypoint(waypoint);

            if (waypoint.Type == 2)
            {
                Lives -= enemy.EnemyData.LivesTaken;
                EnemiesToRemove.Add(enemy);
                if (Lives <= 0)
                {
                    LoseGame();
                }
            }
            else
            {
                enemy.CurrentWaypoint = nextWaypoints[RandomTools.RandomInt(nextWaypoints.Count)];
            }
        }

        void FloatingTower_OnClick(Button sender, GameTime gameTime)
        {
            //Checks for collision with all the restrictions - will need to add specific check for water, path, etc. later
            if (CurrentLevel.BoundingBox.Contains(FloatingTower.BoundingBox))
            {
                foreach (Restriction restriction in CurrentLevel.Restrictions)
                {
                    if (restriction.IntersectsWith(FloatingTower.BoundingBox))
                    {
                        return;
                    }
                }
                foreach(Tower tower in Towers)
                {
                    if (tower.BoundingBox.Intersects(FloatingTower.BoundingBox))
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }

            if (Money >= SelectedTower.Cost)
            {
                Money -= SelectedTower.Cost;
                Tower tower = new Tower(SelectedTower, Input.MousePosition);
                if (tower.TowerData.SpecialActionsOnCreate != null)
                {
                    tower.TowerData.SpecialActionsOnCreate(tower);
                }
                Towers.Add(tower);

                FloatingTower.Visible = false;
                FloatingTower.Enabled = false;
                FloatingRange.Visible = false;
                FloatingRange.Enabled = false;
            }
        }

        void towerButton_OnClick(Button sender, GameTime gameTime)
        {
            int numberID = Convert.ToInt32(sender.ID.Split('_')[0]);

            if (Money >= Config.Towers[numberID].Cost)
            {
                FloatingTower.Visible = true;
                FloatingTower.Enabled = true;
                FloatingRange.Visible = true;
                FloatingRange.Enabled = true;
                FloatingTower.Style.AllTextures = EdgeGame.GetTexture(Config.Towers[numberID].Texture);
                FloatingRange.Scale = new Vector2(Config.Towers[numberID].Range / 500f);
                SelectedTower = Config.Towers[numberID];
            }
        }

        void towerButton_OnMouseOff(Button sender, GameTime gameTime)
        {
            int numberID = Convert.ToInt32(sender.ID.Split('_')[0]);

            TowerInfoSprite.Text = "Description:\nNONE";
        }

        void towerButton_OnMouseOver(Button sender, GameTime gameTime)
        {
            int numberID = Convert.ToInt32(sender.ID.Split('_')[0]);

            TowerInfoSprite.Text = "Description:\n" + Config.Towers[numberID].Description;
        }

        public override void SwitchOut()
        {
            EdgeGame.ClearColor = Color.Black;

            base.SwitchOut();
        }

        public override void UpdateObject(GameTime gameTime)
        {
            RoundManager.Update(gameTime);

            if (FloatingTower.Enabled)
            {
                FloatingTower.Position = Input.MousePosition;
                FloatingRange.Position = Input.MousePosition;

                Color changedColor = new Color(25, 25, 25, 150);
                bool hasChanged = false;
                if (CurrentLevel.BoundingBox.Contains(FloatingTower.BoundingBox))
                {
                    foreach (Restriction restriction in CurrentLevel.Restrictions)
                    {
                        if (restriction.IntersectsWith(FloatingTower.BoundingBox))
                        {
                            FloatingTower.Color = changedColor;
                            hasChanged = true;
                            break;
                        }
                    }
                    foreach(Tower tower in Towers)
                    {
                        if (tower.BoundingBox.Intersects(FloatingTower.BoundingBox))
                        {
                            FloatingTower.Color = changedColor;
                            hasChanged = true;
                            break;
                        }
                    }
                }
                else
                {
                    FloatingTower.Color = changedColor;
                    hasChanged = true;
                }

                if (!hasChanged)
                {
                    FloatingTower.Color = Color.White;
                }
            }

            foreach(Button button in TowerButtons)
            {
                button.Update(gameTime);
            }

            foreach(Sprite sprite in TowerSprites)
            {
                sprite.Update(gameTime);
            }
            foreach(TextSprite textSprite in TowerCostSprites)
            {
                textSprite.Update(gameTime);
            }

            foreach (Enemy enemy in Enemies)
            {
                enemy.Update(gameTime);
            }
            foreach(Enemy enemy in EnemiesToRemove)
            {
                Enemies.Remove(enemy);
            }
            EnemiesToRemove.Clear();

            foreach(Tower tower in Towers)
            {
                tower.Update(gameTime);
                tower.UpdateTower(Enemies);

                if (tower.BoundingBox.Contains(new Point((int)Input.MousePosition.X, (int)Input.MousePosition.Y)) && Input.JustLeftClicked())
                {
                    TowerPanel.ShowWithTower(tower);
                }
            }

            base.UpdateObject(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            foreach (Button button in TowerButtons)
            {
                button.Draw(gameTime);
            }
            foreach (Sprite sprite in TowerSprites)
            {
                sprite.Draw(gameTime);
            }
            foreach (TextSprite textSprite in TowerCostSprites)
            {
                textSprite.Draw(gameTime);
            }
            foreach (Tower tower in Towers)
            {
                tower.Draw(gameTime);
            }
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(gameTime);
            }

            base.DrawObject(gameTime);
        }

        void Input_OnKeyRelease(Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !MenuManager.InputEventHandled)
            {
                if (FloatingTower.Enabled == true)
                {
                    FloatingTower.Visible = false;
                    FloatingTower.Enabled = false;
                    FloatingRange.Visible = false;
                    FloatingRange.Enabled = false;
                }
                else if (TowerPanel.Enabled == true)
                {
                    TowerPanel.Visible = false;
                    TowerPanel.Enabled = false;
                }
                else
                {
                    MenuManager.SwitchMenu("OptionsMenu");
                    MenuManager.InputEventHandled = true;
                }
            }

            if (key == Keys.PageDown)
            {
                Money = Int32.MaxValue;
            }
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
