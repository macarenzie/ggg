using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and did all level texture loading, assisted in level design,
    ///     freeze mechanics, level switching, and code optimization
    /// Hui Lin: worked on enums and current state game state stuff
    /// Ariel: finalized UI fsm and implimented level switching / design, collectible mechanics,
    ///     player freeze mechanics, win/lose conditions, gameplay testing, and code optimization
    /// Jamie: edge collision so the player does not go over obstacles that the player
    ///     cannot go through and Hide()
    /// Julia: Edited comments and a bit of code optimization
    /// </summary>
    #region Enums

    //for the general state of the game
    public enum GameState
    {
        Menu,
        Options,
        Game,
        GameLose,
        GameWin,
        Debug
    }

    //the state of the player
    public enum PlayerState
    {
        Front,
        Back,
        Left,
        Right,
        PlayDead
    }

    //the game screen / "level"
    public enum GameScreen
    {
        One,
        Two,
        Three
    }
    #endregion

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region Buttons
        // start menu and buttons
        private Texture2D startButtonBase2D;
        private Texture2D startButtonRollOver;
        private Button startButton;

        // options button
        private Texture2D optionsButtonBase;
        private Texture2D optionsButtonRollOver;
        private Button optionsButton;

        // try again button
        private Texture2D tryAgainBase;
        private Texture2D tryAgainRollOver;
        private Button tryAgainButton;

        // quit button
        private Texture2D quitBase;
        private Texture2D quitRollOver;
        private Button quitButton;

        private Texture2D environmentalArt;

        // debug mode 
        private Texture2D debugModeBase;
        private Texture2D debugModeRollOver;
        private Button debugModeButtonOff;
        private Button debugModeButtonOn;

        //exit button
        private Texture2D exitBase;
        private Texture2D exitRollOver;
        private Button exitButton;

        //tiny button for exit
        private Texture2D xMarkTexture;
        private Button xMarkButton;

        // play again
        private Texture2D playAgainBase;
        private Texture2D playAgainRollOver;
        private Button playAgainButton;
        private Button exitWinButton;

        #endregion

        #region Collectibles
        //Textures for collectible objects
        private Texture2D collectibleBurger;
        private Texture2D collectibleCandy;
        private Texture2D collectibleChips;
        private List<Texture2D> collectibleTextures;

        //Records how much food is remaining
        private int foodLeft;
        #endregion

        #region Player
        private Texture2D pSprite;
        private Player player;
        #endregion

        #region KBState
        //Records current and previous state of keyboard
        private KeyboardState kbstate;
        private KeyboardState previousKbState;
        #endregion

        //Font fields
        private SpriteFont comicsans30;

        //Game window
        private int windowWidth;
        private int windowHeight;

        //Screens for non game states
        private Texture2D menuScreen;
        private Texture2D optionScreen;
        private Texture2D winScreen;
        private Texture2D loseScreen;

        //Current state of the game
        private GameState currentState;

        //Texture of obstacles
        private Texture2D obstacleTexture;

        //Texture of enemies
        private Texture2D enemyTexture;

        #region Level
        //Level lists
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;

        //Level strings
        private string level1;
        private string level2;
        private string level3;
        private List<string> levelStrings;

        //Level objects
        private Level lvl1;
        private Level lvl2;
        private Level lvl3;
        private List<Level> lvls;

        //Level conditions
        private int levelCount;
        private double timer;
        Stopwatch frozenTimer;
        #endregion

        //Debug mode
        private bool debug;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Changes window size to 900 x 900
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 900;
        }

        protected override void Initialize()
        {
            //Stores the dimensions of the window size
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            //Sets the initial game state to menu
            currentState = GameState.Menu;

            //Initializing timer
            timer = 0;

            //Debug mode set fo false by default
            debug = false;

            //Initializes the collectible texture list
            collectibleTextures = new List<Texture2D>();

            //Initializes level status
            levelCount = -1;
            level1 = "levelScreen1";
            level2 = "levelScreen2";
            level3 = "levelScreen3";
            levelStrings = new List<string>();
            levelStrings.Add(level1);
            levelStrings.Add(level2);
            levelStrings.Add(level3);

            //Creates the timer for freezing the player
            frozenTimer = new Stopwatch();

            base.Initialize(); 
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Button
            // start button
            startButtonBase2D =
                Content.Load<Texture2D>("startButtonBase");
            startButtonRollOver =
                Content.Load<Texture2D>("startButtonRollOver");
            startButton = new Button(
                startButtonBase2D,      // initial button texture
                new Rectangle(
                    (windowWidth / 2) - (startButtonBase2D.Width / 4),          // x value
                    (windowHeight / 2) - (startButtonBase2D.Height / 4),        // y value
                    startButtonBase2D.Width / 2,                                // width of button
                    startButtonBase2D.Height / 2),                              // height of button
                startButtonRollOver);                                           // rollover button texture

            //quit button
            quitBase =
                Content.Load<Texture2D>("quitBase");
            quitRollOver =
                Content.Load<Texture2D>("quitRollOver");
            quitButton = new Button(
                quitBase,
                new Rectangle(
                    ((windowWidth / 2) + 200) - (quitBase.Width / 4),                   //x value
                    ((windowHeight / 2) + 150) - (quitBase.Height / 4),                 //y value
                    quitBase.Width / 2,     //width
                    quitBase.Height / 2     //height
                    ),
                quitRollOver
                );


            //try again button
            tryAgainBase =
                Content.Load<Texture2D>("tryAgainBase");
            tryAgainRollOver =
                Content.Load<Texture2D>("tryAgainRollOver");
            tryAgainButton = new Button(
                tryAgainBase,
                new Rectangle(
                    ((windowWidth / 2) - 200) - (tryAgainBase.Width / 4),                   //x value
                    ((windowHeight / 2) + 150) - (tryAgainBase.Height / 4),                 //y value
                    tryAgainBase.Width / 2,     //width
                    tryAgainBase.Height / 2     //height
                    ),
                tryAgainRollOver
                );


            // option button
            optionsButtonBase =
                Content.Load<Texture2D>("optionButtonBase");
            optionsButtonRollOver =
                Content.Load<Texture2D>("optionButtonRollOver");
            optionsButton = new Button(
                optionsButtonBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4),
                    (windowHeight / 2) + (optionsButtonBase.Height / 4) + 50,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                optionsButtonRollOver
                );

            //debug mode button
            debugModeBase =
                Content.Load<Texture2D>("debugModeBase");
            debugModeRollOver =
                Content.Load<Texture2D>("debugModeRollOver");
            debugModeButtonOn = new Button(
                debugModeBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) - 160,
                    (windowHeight / 2) + (optionsButtonBase.Height / 4) + 190,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                debugModeBase
                );
            debugModeButtonOff = new Button(
                debugModeRollOver,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) - 160,
                    (windowHeight / 2) + (optionsButtonBase.Height / 4) + 190,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                debugModeRollOver
                );

            //exit from main menu button
            exitBase =
                Content.Load<Texture2D>("exitButtonBase");
            exitRollOver =
                Content.Load<Texture2D>("exitButtonRollOver");
            exitButton = new Button(
                 exitBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4),
                    (windowHeight / 2) + (optionsButtonBase.Height / 4) + 175,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                exitRollOver
                );

            //x mark from options
            xMarkTexture =
                Content.Load<Texture2D>("xMark");
            xMarkButton = new Button(
                xMarkTexture,
                new Rectangle(
                    90,
                    90,
                    xMarkTexture.Width / 2,
                    xMarkTexture.Height / 2
                    ),
                xMarkTexture
                );

            //win play again
            playAgainBase =
                Content.Load<Texture2D>("playAgainBase");
            playAgainRollOver =
                Content.Load<Texture2D>("playAgainRollOver");
            playAgainButton = new Button(
                playAgainBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) - 200,
                    150,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                playAgainRollOver
                );

            //win exit
            exitWinButton = new Button(
                exitBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) + 200,
                    150,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                exitRollOver
    );

            #endregion

            //background art
            environmentalArt = Content.Load<Texture2D>("environmentalArt");

            // player sprite
            pSprite = Content.Load<Texture2D>("playerSprite");

            // creates player object
            player = new Player(
                pSprite,
                new Rectangle(10, 10, pSprite.Width / 4, pSprite.Height / 4));

            #region Collectibles
            //Loads collectible textures
            collectibleBurger = Content.Load<Texture2D>("colBurger");
            collectibleCandy = Content.Load<Texture2D>("colCandy");
            collectibleChips = Content.Load<Texture2D>("colChips");

            //Adds textures to texture list for collectible generation
            collectibleTextures.Add(collectibleBurger);
            collectibleTextures.Add(collectibleCandy);
            collectibleTextures.Add(collectibleChips);
            #endregion

            #region Game Screens
            //menu Screen
            menuScreen = Content.Load<Texture2D>("startScreen");

            //option Screen
            optionScreen = Content.Load<Texture2D>("optionsScreenFixedAgain");

            //win Screen
            winScreen = Content.Load<Texture2D>("winScreen");

            //lose Screen
            loseScreen = Content.Load<Texture2D>("gameOverScreen");
            #endregion

            // temporary font
            comicsans30 = Content.Load<SpriteFont>("comicsans30");

            // obstacle
            obstacleTexture = Content.Load<Texture2D>("obstacleTexture");

            // enemy
            enemyTexture = Content.Load<Texture2D>("enemyTexture");

            // level loading
            lvl1 = new Level(
                level1,
                collectibleTextures,       // collectible texture
                obstacleTexture,         // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture
            lvl2 = new Level(
                level2,
                collectibleTextures,       // collectible texture
                obstacleTexture,         // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture
            lvl3 = new Level(
                level3,
                collectibleTextures,       // collectible texture
                obstacleTexture,         // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture

            lvls = new List<Level>();
            lvls.Add(lvl1);
            lvls.Add(lvl2);
            lvls.Add(lvl3);

            //Determines the amount of food the player has to collect
            foreach (Level l in lvls)
            {
                foodLeft += l.CollectiblesList.Count;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Gets current keyboard state
            kbstate = Keyboard.GetState();

            switch (currentState)
            {
                #region MENU SCREEN ---------------------------------------------------------------
                case GameState.Menu:

                    // starts the game
                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
                        //resetting game
                        timer = 100;
                        NextLevel();

                        currentState = GameState.Game;
                    }

                    // options screen
                    if (optionsButton.MouseClick() && optionsButton.MouseContains())
                    {
                        currentState = GameState.Options;
                    }

                    //to exit the game from menu
                    if (exitButton.MouseClick() && exitButton.MouseContains())
                    {
                        Exit();
                    }
                    break;
                #endregion
                #region OPTIONS SCREEN ------------------------------------------------------------
                case GameState.Options:

                    //toggles debug mode when U is pressed
                    if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == false)
                    {
                        debug = true;
                    }
                    else if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == true)
                    {
                        debug = false;
                    }

                    // return to main menu
                    if (xMarkButton.MouseClick() && xMarkButton.MouseContains())
                    {
                        currentState = GameState.Menu;
                    }
                    break;
                #endregion
                #region GAMEPLAY SCREEN ------------------------------------------------------------
                case GameState.Game:

                    //Advances to next level when player reaches exit
                    if (player.Y + player.Rect.Height < 0   // player goes to the exit
                        && levelCount < lvls.Count          // there are more levels left
                        && collectiblesList.Count == 0)     // all collectibles on the
                                                            //     screen are collected
                    {
                        ResetLevel();
                        NextLevel();
                    }

                    // stops the player from leaving the screen if all
                    // collectibles haven't been collected
                    if (collectiblesList.Count != 0 && player.Y <= 0)
                    {
                        player.Y = 0;
                    }

                    //General game functions

                    //Updates player
                    player.Update(gameTime);

                    //Updates collectibles
                    CollectibleCollision();

                    //Updates each enemy
                    foreach (Enemy e in enemyList)
                    {
                        e.Update(gameTime);
                    }

                    //Checks for debug mode, runs debug specific functions if so
                    if (debug)
                    {
                        //Checks if the game should still be running
                        if (foodLeft >= 0 && levelCount != lvls.Count)
                        {
                            //Runs hide/unhide on each obstacle to check for player input to hide
                            foreach (Obstacle obstacle in obstaclesList)
                            {
                                if (player.IsHiding)
                                {
                                    UnHide(previousKbState, kbstate, obstacle, player);

                                }
                                else
                                {
                                    Hide(previousKbState, kbstate, obstacle, player);
                                }
                            }

                            //Runs collision checks for each enemy
                            foreach (Enemy e in enemyList)
                            {
                                e.EnemyObstacleCollision(obstaclesList);
                            }
                        }

                        //Ends the game when all food is collected and player is on level 3
                        else if (foodLeft == 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameWin;
                        }
                    }

                    //Runs the game in non debug state
                    if (!debug)
                    {
                        //Decreases the timer
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        //Checks for win/lose conditions--how much food is left, and current time on timer
                        //Transitions to lose screen if time runs out 
                        if (timer <= 0 && (foodLeft != 0 || foodLeft == 0) && levelCount <= lvls.Count)
                        {
                            currentState = GameState.GameLose;
                        }
                        //Transitions to win screen if all food has been collected
                        else if (timer >= 0 && foodLeft == 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameWin;
                        }

                        //While the game is ongoing
                        else
                        {
                            //Enemy behavior. Loops for each existing enemy
                            foreach (Enemy e in enemyList)
                            {
                                //Checks for any collision with any obstacles
                                e.EnemyObstacleCollision(obstaclesList);

                                // create immunity buff after player escapes play dead state
                                if (player.IsImmune)
                                {
                                    //starts timer for player immunity
                                    frozenTimer.Start();

                                    //ends player immunity state
                                    if (frozenTimer.Elapsed.TotalSeconds > 3)
                                    {
                                        player.IsImmune = false;
                                        frozenTimer.Stop();
                                        frozenTimer.Reset();
                                    }
                                }

                                //When player can be frozen by the enemy
                                else
                                {
                                    //continue checking collisions
                                    if (player.IndividualCollision(e.Rect))
                                    {
                                        e.intersectsPlayer(player.Rect);
                                        player.intersectsEnemy(e.Rect);
                                    }
                                    player.IsImmune = false;
                                }
                            }

                            //Checks for player collision with obstacles
                            if (!player.IsHiding)
                            {
                                CheckObstacleCollision(player, obstaclesList);    //edge collision
                            }

                            //hides/unhides player based on current hide status and keyboard input
                            foreach (Obstacle obstacle in obstaclesList)
                            {
                                //check for hide attempts
                                if (!player.IsHiding)
                                {
                                    Hide(previousKbState, kbstate, obstacle, player);
                                }
                                //check for unhiding attempts
                                else
                                {
                                    UnHide(previousKbState, kbstate, obstacle, player);
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region GAME LOSE SCREEN ----------------------------------------------------------
                case GameState.GameLose:

                    //go back to menu
                    if (tryAgainButton.MouseClick() && tryAgainButton.MouseContains())
                    {
                        ResetLevel();
                        ResetGame();
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameLose
                    if (quitButton.MouseClick() && quitButton.MouseContains())
                    {
                        Exit();
                    }
                    break;
                #endregion
                #region GAME WIN SCREEN -----------------------------------------------------------
                case GameState.GameWin:
                    
                    //Moves to menu
                    if (playAgainButton.MouseClick() && playAgainButton.MouseContains())
                    {
                        ResetLevel();
                        ResetGame();
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameWin
                    if (exitWinButton.MouseClick() && exitWinButton.MouseContains())
                    {
                        Exit();
                    }
                    break;
                    #endregion
            }

            // updates the previous keyboard state
            previousKbState = kbstate;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            _spriteBatch.Begin();
            switch (currentState)
            {
                #region MENU SCREEN ---------------------------------------------------------------
                case GameState.Menu:

                    // draw the main menu screen graphic
                    _spriteBatch.Draw(menuScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    //drawing of the buttons
                    startButton.Draw(_spriteBatch);
                    optionsButton.Draw(_spriteBatch);
                    exitButton.Draw(_spriteBatch);

                    break;
                #endregion
                #region OPTIONS SCREEN ------------------------------------------------------------
                case GameState.Options:

                    // draw the options screen graphic
                    _spriteBatch.Draw(optionScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    // draw the exit button
                    xMarkButton.Draw(_spriteBatch);

                    // draw text for debug mode
                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("Press 'U' to toggle erin mode!\nErin mode: " + debug),
                        new Vector2(90, 700),
                        Color.LightSteelBlue);

                    break;
                #endregion
                #region GAMEPLAY SCREEN ------------------------------------------------------------
                case GameState.Game:

                    if (debug)
                    {
                        // DRAW ORDER: obstacle, collectibles, enemy, player

                        // background art
                        _spriteBatch.Draw(environmentalArt, new Rectangle(0, 0, 900, 900), Color.White);

                        // draw each obstacle
                        foreach (Obstacle obstacle in obstaclesList)
                        {
                            if (IsInRange(obstacle.Rect, player) && obstacle.IsHideable)
                            {
                                obstacle.Draw(_spriteBatch, Color.LightSteelBlue);
                            }
                            else
                            {
                                obstacle.Draw(_spriteBatch, Color.White);
                            }
                        }

                        // draw each collectible
                        foreach (IGameObject collectible in collectiblesList)
                        {
                            collectible.Draw(_spriteBatch, Color.White);
                        }

                        // draw each enemy
                        foreach (IGameObject enemy in enemyList)
                        {
                            enemy.Draw(_spriteBatch, Color.White);
                        }

                        // draw the player based on collision
                        foreach (Enemy enemy in enemyList)
                        {
                            bool collide = player.IndividualCollision(enemy.Rect);

                            if (collide)
                            {
                                player.Draw(_spriteBatch, Color.Red);
                                break;
                            }

                            player.Draw(_spriteBatch, Color.White);
                        }
                    }
                    else
                    {
                        // DRAW ORDER: collectibles, player, obstacle, enemy

                        // background art
                        _spriteBatch.Draw(environmentalArt, new Rectangle(0, 0, 900, 900), Color.White);

                        // draw each collectible
                        foreach (IGameObject collectible in collectiblesList)
                        {
                            collectible.Draw(_spriteBatch, Color.White);
                        }

                        // draw the player
                        player.Draw(_spriteBatch, Color.White);

                        // draw each obstacle
                        foreach (Obstacle obstacle in obstaclesList)
                        {
                            if (IsInRange(obstacle.Rect, player) && obstacle.IsHideable)
                            {
                                obstacle.Draw(_spriteBatch, Color.LightSteelBlue);
                            }
                            else
                            {
                                obstacle.Draw(_spriteBatch, Color.White);
                            }
                        }

                        // draw each enemy
                        foreach (IGameObject enemy in enemyList)
                        {
                            enemy.Draw(_spriteBatch, Color.White);
                        }

                        // draw the player if it is playing dead
                    }

                    //drawing the timer to the screen
                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("Time left: {0:0}", timer),
                        new Vector2(0, 5),
                        Color.White);

                    break;
                #endregion
                #region GAME LOSE SCREEN ----------------------------------------------------------
                case GameState.GameLose:

                    // draw the lose screen
                    _spriteBatch.Draw(
                        loseScreen,
                        new Vector2(0, 0),
                        Color.White);

                    // draw each button
                    quitButton.Draw(_spriteBatch);
                    tryAgainButton.Draw(_spriteBatch);

                    break;
                #endregion
                #region GAME WIN SCREEN -----------------------------------------------------------
                case GameState.GameWin:

                    _spriteBatch.Draw(winScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    playAgainButton.Draw(_spriteBatch);
                    exitWinButton.Draw(_spriteBatch);
                    break;
                    #endregion
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // HELPER METHODS -------------------------------------------------------------------------

        /// <summary>
        /// checks for single key press
        /// </summary>
        /// <param name="key"> a keyboard key </param>
        /// <param name="currentState"> current kb state </param>
        /// <param name="previousState">prev kb state </param>
        /// <returns></returns>
        private bool SingleKeyPress (
            Keys key, KeyboardState currentState, KeyboardState previousState)
        {
            if (currentState.IsKeyDown(key) && previousState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks for collectible collision
        /// worked on by ariel cthwe
        /// </summary>
        void CollectibleCollision()
        {
            //collectible collision
            //remove if shift key is pressed and decrease food collected count
            for (int i = 0; i < collectiblesList.Count; i++)
            {
                bool collide = player.IndividualCollision(collectiblesList[i].Rect);

                if (collide && 
                    (SingleKeyPress(Keys.LeftShift, kbstate, previousKbState) 
                    || SingleKeyPress(Keys.RightShift, kbstate, previousKbState))
                    && player.PlayerState != PlayerState.PlayDead)
                {
                    collectiblesList.Remove(collectiblesList[i]);
                    foodLeft--;
                    i--;
                }
            }
        }

        /// <summary>
        /// Checks if the player is overlapping with any Obstacle object
        /// If the player is overlapping, then the player's position will be adjusted 
        /// so that only their edges are touching
        /// Worked on by Jamie Zheng
        /// </summary>
        /// <param name="player">The Player object</param>
        /// <param name="obstaclesList">A list of all the Obstacles in this game, 
        /// should exist in the Level class</param>
        void CheckObstacleCollision(Player player, List<Obstacle> obstaclesList)
        {
            //this only matters if the player is not hiding.
            //otherwise without this bool the player will just clip out of the box
            //because every obstacle cannot be overlapped
            if (!player.IsHiding)
            {
                //Going through each obstacle in the list of obstacles to check for intersection
                foreach (Obstacle obstacle in obstaclesList)
                {
                    //If the intersection returns true
                    if (player.Rect.Intersects(obstacle.Rect))
                    {
                        //Get the intersection area
                        Rectangle intersectionArea = Rectangle.Intersect(
                            player.Rect, obstacle.Rect);

                        //If the width is less than the height, adjust the X position
                        if (intersectionArea.Width < intersectionArea.Height)
                        {
                            //LEFT side of obstacle
                            //player.X is less than the midpoint of the obstacle's width
                            if (player.X < (obstacle.Rect.X + obstacle.Rect.Width / 2))
                            {
                                player.X -= intersectionArea.Width;

                            }
                            //RIGHT
                            else
                            {
                                player.X += intersectionArea.Width;

                            }

                        }

                        //If the height it less than the width, adjust the Y position
                        else if (intersectionArea.Height < intersectionArea.Width)
                        {
                            //TOP side of the obstacle;
                            //If player.Y is less than the obstacle's Height midpoint
                            if (player.Y < (obstacle.Rect.Y + obstacle.Rect.Height / 2))
                            {
                                player.Y -= intersectionArea.Height;

                            }
                            //BOTTOM
                            else
                            {
                                player.Y += intersectionArea.Height;

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checking if another object is in range
        /// Used to determine if a food collectible is in range
        /// or a hiding spot is in range to collect or use
        /// Worked on by Jamie Zheng
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns>A bool on is the object is in range. In draw 
        /// there should be a visual indication on applications</returns>
        bool IsInRange(Rectangle otherObject, Player player)
        {

            //the distance in the x and y direction.
            //Half of player's width/ height + half of the other objects width/ height
            float dx = Math.Abs((player.Rect.Width / 2) + (otherObject.Width / 2));
            float dy = Math.Abs((player.Rect.Height / 2) + (otherObject.Height / 2));

            //player mid point coordinates
            float pMidX = player.X + player.Rect.Width / 2;
            float pMidY = player.Y + player.Rect.Height / 2;

            //otherObject midpoint coordinates
            float oMidX = otherObject.X + otherObject.Width / 2;
            float oMidY = otherObject.Y + otherObject.Height / 2;

            if (
                //distance needs to be less than or equal to when they touch + 50 pixels
                (dx + 50) >= Math.Abs(pMidX - oMidX)
                && (dy + 50) >= Math.Abs(pMidY - oMidY)
                )
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Will change the player's position to be overlapping with the hideable obstacle
        /// Check if IsInRange is true
        /// Press and release space bar. 
        /// Worked on by Jamie Zheng
        /// </summary>
        /// <param name="prevState">previous keyboard state</param>
        /// <param name="curState">current keyboard state</param>
        /// <param name="otherObstacle">the obstacle you want to check is hideable</param>
        /// <param name="player">Player object</param>
        void Hide(KeyboardState prevState,
            KeyboardState curState, Obstacle otherObstacle, Player player)
        {
            //get mid points lined up
            if (IsInRange(otherObstacle.Rect, player)               //obstacle in range
                && otherObstacle.IsHideable                         //check for hideability
                && !player.IsHiding                                 //is player not hiding
                && SingleKeyPress(Keys.Space, curState, prevState)  //check for space bar
                && player.PlayerState != PlayerState.PlayDead)      //Player isn't frozen
            {
                //Centers the player with the obstacle
                player.X = (otherObstacle.Rect.X + (otherObstacle.Rect.Width / 2))
                    - (player.Rect.Width / 2);

                player.Y = (otherObstacle.Rect.Y + (otherObstacle.Rect.Height / 2))
                    - (player.Rect.Height / 2);

                player.IsHiding = true;
            }
        }

        /// <summary>
        /// Press either WASD to exit the object the player was previously hiding in. 
        /// Assuming there is no obstacle blocking the player's path. Then do no move
        /// Written by Jamie Zheng
        /// </summary>
        /// <param name="prevState">previous keyboard state</param>
        /// <param name="curState">current keyboard state</param>
        /// <param name="otherObstacle">the obstacle you want to check against</param>
        /// <param name="player">Player object</param>
        void UnHide(KeyboardState prevState,
            KeyboardState curState, Obstacle otherObstacle, Player player)
        {
            Rectangle potentialPlayer = player.Rect;

            //if the space bar is pressed again then unhide
            //Also check if the direction the player wants to unhide from is valid
            if (player.IsHiding)
            {
                //W; Up direction
                if (SingleKeyPress(Keys.W, curState, prevState))
                {
                    //adjust to potential coordinates
                    potentialPlayer.Y -= potentialPlayer.Height;

                    //check for intersection. If intersection is false,
                    //then do no move in that direction and go back to hiding
                    if (!potentialPlayer.Intersects(otherObstacle.Rect))
                    {
                        player.IsHiding = false; //change bool

                        //position changing logic
                        player.Y -= player.Rect.Width;
                    }

                }

                //A; Left Direction
                if (SingleKeyPress(Keys.A, curState, prevState))
                {
                    potentialPlayer.X -= potentialPlayer.Width;

                    if (!potentialPlayer.Intersects(otherObstacle.Rect))
                    {
                        player.IsHiding = false;

                        player.X -= player.Rect.Width;
                    }
                }

                //S; Down Direction
                if (SingleKeyPress(Keys.S, curState, prevState))
                {
                    potentialPlayer.Y += potentialPlayer.Height;

                    if (!potentialPlayer.Intersects(otherObstacle.Rect))
                    {
                        player.IsHiding = false;

                        player.Y += player.Rect.Height;
                    }
                }

                //D; Right Direction
                if (SingleKeyPress(Keys.D, curState, prevState))
                {
                    potentialPlayer.X += potentialPlayer.Width;

                    if (!potentialPlayer.Intersects(otherObstacle.Rect))
                    {
                        player.X += player.Rect.Height;
                        player.IsHiding = false;
                    }
                }
            }
        }

        /// <summary>
        /// transitions the current level to the next level
        /// worked on by mckenzie lam
        /// </summary>
        void NextLevel()
        {
            // increase the level count
            levelCount++;

            // continue the game if there are still levels left
            if (levelCount < lvls.Count)
            {
                player.Rect = lvls[levelCount].Player.Rect;
                collectiblesList = lvls[levelCount].CollectiblesList;
                obstaclesList = lvls[levelCount].ObstacleList;
                enemyList = lvls[levelCount].EnemyList;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// resets the level by clearing all game object lists
        /// worked on by mckenzie lam
        /// </summary>
        void ResetLevel()
        {
            collectiblesList.Clear();
            obstaclesList.Clear();
            enemyList.Clear();
        }

        /// <summary>
        /// resets the entire game
        /// worked on my mckenzie lam and ariel cthwe
        /// </summary>
        void ResetGame()
        {
            // reset the level counter
            levelCount = -1;

            // reload the level content into the corresponding level objects
            for (int i = 0; i < lvls.Count; i++)
            {
                lvls[i].LoadLevel(levelStrings[i]);
            }

            // reload the food counter
            foreach (Level l in lvls)
            {
                foodLeft += l.CollectiblesList.Count;
            }
        }
    }
}