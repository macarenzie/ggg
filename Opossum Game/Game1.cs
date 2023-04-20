using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and started loading in content, worked on 
    ///    temp fsm for update and draw, did all data driven level loading,
    ///    helped with collision handling in update and draw for each game object
    /// Hui Lin: worked on enums and current state game state stuff
    /// Ariel: finalized UI fsm and implimented camera movement fsm
    /// Jamie: edge collision so the player does not go over obstacles that the player
    /// cannot go through & Hide()
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

        #endregion

        #region Collectibles
        private Texture2D collectibleBurger;
        private Texture2D collectibleCandy;
        private Texture2D collectibleChips;
        private List<Texture2D> collectibleTextures;
        #endregion

        #region Player
        private Texture2D pSprite;
        //private Texture2D pSpriteSide;
        private Player player;
        #endregion

        #region Level
        // "camera" textures
        private Texture2D gameScreen1;
        private Texture2D gameScreen2;
        private Texture2D gameScreen3;

        //the game screen
        private GameScreen currentScreen;

        //the timer
        private double timer;
        #endregion

        #region KBState
        private KeyboardState kbstate;
        private KeyboardState previousKbState;
        #endregion

        // font fields
        private SpriteFont comicsans30;

        //literal window
        private int windowWidth;
        private int windowHeight;

        //all general window screens
        private Texture2D menuScreen;
        private Texture2D optionScreen;
        private Texture2D winScreen;
        private Texture2D loseScreen;

        private GameState currentState;

        //List of interactible objects
        private List<InteractibleObject> objects;

        //Obstacle test. Texture and rectangle and obstacle list
        private Texture2D obstacleTexture;
        private Rectangle obstacleDimensions;
        private Obstacle hideableTestObstacle;
        private Obstacle nonHideableTestObstacle;
        private bool isColliding;

        //Placeholder light
        private Texture2D lightTexture;
        private Rectangle lightDimensions;
        private bool isCollidingLight;

        // enemy
        private Texture2D enemyTexture;
        private Rectangle enemyDimensions;

        #region LevelLoading
        private StreamReader reader;
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;
        private Level lvl1;
        private Level lvl2;
        private Level lvl3;
        private List<Level> lvls;
        private string level1;
        private string level2;
        private string level3;
        private List<string> levels;
        private int levelCount;
        #endregion

        // DEBUG MODE
        private bool debug;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //change window size
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 900;

        }

        protected override void Initialize()
        {
            // store the dimesnions of the game window into a variable
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            // start the game state in menu
            currentState = GameState.Menu;

            //start the first game level at one
            currentScreen = GameScreen.One;

            //Test for list collision
            obstaclesList = new List<Obstacle>();

            //Initializing timer
            timer = 200; //15 seconds

            // level loading
            level1 = "levelScreen1";
            level2 = "levelScreen2";
            level3 = "levelScreen3";

            levels = new List<string>();
            levels.Add(level1);
            levels.Add(level2);
            levels.Add(level3);

            // debug mode
            debug = false;

            // initialize the collectible texture list
            collectibleTextures = new List<Texture2D>();

            // level shit
            levelCount = 0;

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

            #endregion

            //background art
            environmentalArt = Content.Load<Texture2D>("environmentalArt");

            // player sprite
            pSprite = Content.Load<Texture2D>("playerSprite");
            //pSpriteSide = Content.Load<Texture2D>("playerSpriteSide");


            // player initialization
            player = new Player(
                pSprite,
                new Rectangle(10, 10, pSprite.Width / 4, pSprite.Height / 4));
                //pSpriteSide,
                //new Rectangle(10, 10, pSpriteSide.Width/4, pSpriteSide.Height/4));

            #region Collectibles
            collectibleBurger = Content.Load<Texture2D>("colBurger");
            collectibleCandy = Content.Load<Texture2D>("colCandy");
            collectibleChips = Content.Load<Texture2D>("colChips");
            collectibleTextures.Add(collectibleBurger);
            collectibleTextures.Add(collectibleCandy);
            collectibleTextures.Add(collectibleChips);
            #endregion

            #region Game Screens
            //menuScreen
            menuScreen = Content.Load<Texture2D>("startScreen");

            //optionScreen
            optionScreen = Content.Load<Texture2D>("optionsScreenFixed");

            //winScreen

            //loseScreen
            loseScreen = Content.Load<Texture2D>("gameOverScreen");

            //gameScreen1
            //gameScreen2
            //gameScreen3
            #endregion

            // temporary font
            comicsans30 = Content.Load<SpriteFont>("comicsans30");

            // obstacle
            obstacleTexture = Content.Load<Texture2D>("obstacleTexture");
            /*
            obstacleDimensions = new Rectangle(400, 400, 200, 200);
            hideableTestObstacle = new Obstacle(
                obstacleTexture, 
                obstacleDimensions, 
                true); //isHidable
            nonHideableTestObstacle = new Obstacle(
                obstacleTexture, 
                obstacleDimensions); //non-hideable
            */

            isColliding = false;

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

            // pass in the fields from the level class to the game1 class
            player = lvl1.Player;
            collectiblesList = lvl1.CollectiblesList;
            obstaclesList = lvl1.ObstacleList;
            enemyList = lvl1.EnemyList;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kbstate = Keyboard.GetState();

            switch (currentState)
            {
                //all posibilities for the menu screen
                case GameState.Menu:

                    

                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
                        //resetting game
                        timer = 60;
                        //level.ResetGame();
                        currentState = GameState.Game;
                    }

                    if (SingleKeyPress(Keys.O, kbstate, previousKbState) ||
                        optionsButton.MouseClick() && optionsButton.MouseContains())
                    {
                        currentState = GameState.Options;
                    }

                    //to exit the game from menu
                    if (exitButton.MouseClick() && exitButton.MouseContains())
                    {
                        Exit();
                    }
                    break;

                //all posibilities for options
                case GameState.Options:

                    // PLACEHOLDER TO TEST TRANSITIONS

                    if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == false)
                    {
                        debug = true;
                    }

                    else if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == true)
                    {
                        debug = false;
                    }

                    //exit from the options
                    if (xMarkButton.MouseClick() && xMarkButton.MouseContains())
                    {
                        currentState = GameState.Menu;
                    }

                    //for when we do have the gode mode stuff implemented
                  /*  if (debug == false)
                    {
                        if (debugModeButtonOn.MouseClick() && debugModeButtonOn.MouseContains())
                        {
                            debug = true;
                        }
                    } else
                    {
                        if (debugModeButtonOff.MouseClick() && debugModeButtonOff.MouseContains())
                        {
                            debug = false;
                        }
                    }
                  */
                    break;

                //all options for the state of playing the game
                case GameState.Game:

                    if (player.Y + player.Rect.Height < 0 && levelCount < lvls.Count)
                    {
                        ResetGame();
                        NextLevel();
                    }

                    if (debug)
                    {
                        player.Update(gameTime);
                         
                        
                        //turn collisions off
                        //for each game object check if the player is colliding with it and return false every time
                    }

                    if (!debug)
                    {
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        player.Update(gameTime);

                        #region Collisions
                        //this method is to adjust the player's position with an non-overlappable object 
                        if (!player.IsHiding)
                        {
                            CheckObstacleCollision(player, obstaclesList);         //edge collision
                        }

                        // test if the player is able to hide in an obstacle
                        /*
                        foreach (Obstacle obs in obstaclesList)
                        {
                            bool collide = player.IndividualCollision(obs.Position);

                            if (collide)
                            {
                                player.Hide(kbstate, previousKbState, obs.Position);
                            }
                        }
                        */
                        //cleaner hide loop
                        foreach (Obstacle obstacle in obstaclesList)
                        {
                            Hide(previousKbState, kbstate, obstacle, player);
                        }

                        //collectible collision
                        CollectibleCollision();
                        #endregion

                        //win conditions for now -- moving later to game screen 3
                        if (timer <= 0 && collectiblesList.Count != 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameLose;
                        }
                        
                        else if (timer >= 0 && collectiblesList.Count == 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameWin;
                        }
                        
                    }

                    break;

                case GameState.GameLose:

                    //go back to menue
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState) ||
                        tryAgainButton.MouseClick() && tryAgainButton.MouseContains())
                    {
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameLose
                    if (quitButton.MouseClick() && quitButton.MouseContains())
                    {
                        Exit();
                    }
                    break;
                case GameState.GameWin:

                    if (SingleKeyPress(Keys.M, kbstate, previousKbState)
                        /*menuButton.MouseClick() && menuButton.MouseContains()*/)
                    {
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameWin
                    if (quitButton.MouseClick() && quitButton.MouseContains())
                    {
                        Exit();
                    }
                    break; 
            }

            // update the previous keyboard state
            previousKbState = kbstate;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.Menu:

                    _spriteBatch.Draw(menuScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    //drawing of the buttons
                    startButton.Draw(_spriteBatch);
                    optionsButton.Draw(_spriteBatch);
                    exitButton.Draw(_spriteBatch);


                    break;
                case GameState.Options:

                    _spriteBatch.Draw(optionScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    xMarkButton.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("Press 'U' to toggle debug mode!\nDebug mode: " + debug),
                        new Vector2(90, 700),
                        Color.Pink);

                    /*
                    if (debug == true)
                    {
                        debugModeButtonOn.Draw(_spriteBatch);
                    } 
                    else
                    {
                        debugModeButtonOff.Draw(_spriteBatch);
                    }
                    */
                    break;

                case GameState.Game:

                    if (debug)
                    {
                        // DRAW ORDER: obstacle, collectibles, enemy, player

                        foreach (IGameObject obstacle in obstaclesList)
                        {
                            obstacle.Draw(_spriteBatch, Color.White);
                        }

                        foreach (IGameObject collectible in collectiblesList)
                        {
                            collectible.Draw(_spriteBatch, Color.White);
                        }

                        foreach (IGameObject enemy in enemyList)
                        {
                            enemy.Draw(_spriteBatch, Color.White);
                        }

                        player.Draw(_spriteBatch, Color.White);

                        //drawing the timer to the screen
                        _spriteBatch.DrawString(
                            comicsans30,
                            string.Format("Time left: {0:0}", timer),
                            new Vector2(0, 5),
                            Color.White);
                    }
                    else
                    {
                        // DRAW ORDER: player, collectibles, obstacle, enemy

                        _spriteBatch.Draw(environmentalArt, new Rectangle(0, 0, 900, 900), Color.White);

                        player.Draw(_spriteBatch, Color.White);

                        foreach (IGameObject collectible in collectiblesList)
                        {
                            collectible.Draw(_spriteBatch, Color.White);
                        }

                        foreach (IGameObject obstacle in obstaclesList)
                        {
                            obstacle.Draw(_spriteBatch, Color.White);
                        }

                        foreach (IGameObject enemy in enemyList)
                        {
                            enemy.Draw(_spriteBatch, Color.White);
                        }

                        //drawing the timer to the screen
                        _spriteBatch.DrawString(
                            comicsans30,
                            string.Format("Time left: {0:0}", timer),
                            new Vector2(0, 5),
                            Color.White
                            );

                        // LEVEL ------------------------------------------
                        
                        // draw obstacles based on player collision
                        for (int i = 0; i < obstaclesList.Count; i++)
                        {
                            //test code for hideable objects
                            
                            if (IsInRange(obstaclesList[i].Rect, player) 
                                && obstaclesList[i].IsHideable)
                            {
                                obstaclesList[i].Draw(_spriteBatch, Color.Green);
                            }
                            else
                            {
                                obstaclesList[i].Draw(_spriteBatch, Color.White);
                            }
                            
                            /*
                            if (player.IndividualCollision(obstaclesList[i].Position))
                            {
                                obstaclesList[i].Draw(_spriteBatch, Color.Green);
                            }
                            else
                            {
                                obstaclesList[i].Draw(_spriteBatch, Color.White);
                            }
                            */
                            
                        }

                        // draw each enemy
                        //for (int i = 0; i < enemyList.Count; i++)
                        //{
                        //    enemyList[i].Draw(_spriteBatch, Color.White);
                        //}

                        // draw the player based on collisions with light
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


                        //drawing the timer to the screen
                        _spriteBatch.DrawString(
                            comicsans30,
                            string.Format("Time left: {0:0}", timer),
                            new Vector2(0, 5),
                            Color.White
                            );
                        // LEVEL ------------------------------------------
                    }



                    break;

                case GameState.GameLose:

                    _spriteBatch.Draw(
                        loseScreen,
                        new Vector2(0, 0),
                        Color.White
                        );

                    quitButton.Draw(_spriteBatch);
                    tryAgainButton.Draw(_spriteBatch);
                    break;
                case GameState.GameWin:

                    //_spriteBatch.Draw(winScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    //TEMP
                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("GAME WIN SCREEN"),
                        new Vector2(10, 100),
                        Color.White);

                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("PRESS 'M' FOR MAIN MENU"),
                        new Vector2(10, 200),
                        Color.White);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // HELPER METHODS ---------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentState"></param>
        /// <param name="previousState"></param>
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
        /// TODO: Clean up
        /// checks for collectible collision
        /// </summary>
        public void CollectibleCollision()
        {
            //collectible collision
            for (int i = 0; i < collectiblesList.Count; i++)
            {
                bool collide = player.IndividualCollision(collectiblesList[i].Rect);

                if (collide && 
                    (SingleKeyPress(Keys.LeftShift, kbstate, previousKbState) 
                    || SingleKeyPress(Keys.RightShift, kbstate, previousKbState)))
                {
                    collectiblesList.Remove(collectiblesList[i]);
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
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns>A bool on is the object is in range. In draw 
        /// there should be a visual indication on applications</returns>
        bool IsInRange(Rectangle otherObject, Player player)
        {
            //These numbers can be adjusted when visuals are implemented and do what looks good
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
        /// Press and release space bar. Hide and UnHide are the same. 
        /// If unhiding, then currently by edge collision logic, 
        /// it will set you outside automatically
        /// </summary>
        void Hide(KeyboardState prevState,
            KeyboardState curState, Obstacle otherObstacle, Player player)
        {
            //get mid points lined up
            if (IsInRange(otherObstacle.Rect, player)
                && otherObstacle.IsHideable 
                //&& !player.IsHiding
                )
            {
                if (SingleKeyPress(Keys.Space, curState, prevState))
                {
                    if (!player.IsHiding)
                    {
                        //Centers the player with the obstacle
                        player.X = (otherObstacle.Rect.X + (otherObstacle.Rect.Width / 2))
                            - (player.Rect.Width / 2);

                        player.Y = (otherObstacle.Rect.Y + (otherObstacle.Rect.Height / 2))
                            - (player.Rect.Height / 2);

                        player.IsHiding = true;
                    }

                    //if the space bar is pressed again then unhide
                    //Also check if the direction the player wants to unhide from is valid
                    else if (player.IsHiding)
                    {
                        //W; Up direction
                        if(SingleKeyPress(Keys.W, curState, prevState))
                        player.IsHiding = false;
                        
                        //position changing logic

                        //A; Left Direction

                        //S; Down Direction

                        //D; Right Direction

                        //check for intersection. If intersection is false, then do no move in that direction and go back to hiding
                        
                    }
                    
                }

                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextLevel()
        {
            levelCount++;

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
        /// 
        /// </summary>
        public void ResetGame()
        {
            collectiblesList.Clear();
            obstaclesList.Clear();
            enemyList.Clear();
        }

    }
}