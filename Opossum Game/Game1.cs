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
        #endregion

        #region Collectibles
        private Texture2D collectibleBurger;
        private Texture2D collectibleCandy;
        private Texture2D collectibleChips;
        private List<Texture2D> collectibleTextures;
        #endregion

        #region Player
        private Texture2D pSprite;
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
        private List<Obstacle> obstacleList;
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
        private string levelFile;
        private Level level;
        string levelName;
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
            obstacleList = new List<Obstacle>();

            //Initializing timer
            timer = 20; //15 seconds

            //testing for reseting level
            levelName = "newTestL1";

            // debug mode
            debug = false;

            // initialize the collectible texture list
            collectibleTextures = new List<Texture2D>();

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
                    (windowHeight / 2) + (optionsButtonBase.Height / 4),
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                optionsButtonRollOver
                );
            #endregion

            // player sprite
            pSprite = Content.Load<Texture2D>("playerSprite");

            // player initialization
            player = new Player(
                pSprite,
                new Rectangle(10, 10, pSprite.Width / 4, pSprite.Height / 4));

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
            optionScreen = Content.Load<Texture2D>("optionsScreen");

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
            level = new Level(
                collectibleTextures,       // collectible texture
                obstacleTexture,         // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture
            level.LoadLevel(levelName);

            // pass in the fields from the level class to the game1 class
            player = level.Player;
            collectiblesList = level.CollectiblesList;
            obstaclesList = level.ObstacleList;
            enemyList = level.EnemyList;
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

                    if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == false)
                    {
                        debug = true;
                    }

                    else if (SingleKeyPress(Keys.U, kbstate, previousKbState) && debug == true)
                    {
                        debug = false;
                    }

                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
                        //resetting game
                        ResetGame();
                        currentState = GameState.Game;
                    }

                    if (SingleKeyPress(Keys.O, kbstate, previousKbState) ||
                        optionsButton.MouseClick() && optionsButton.MouseContains())
                    {
                        currentState = GameState.Options;
                    }

                    //to exit the game from menu
                    if (quitButton.MouseClick() && quitButton.MouseContains())
                    {
                        Exit();
                    }
                    break;

                //all posibilities for options
                case GameState.Options:

                    // PLACEHOLDER TO TEST TRANSITIONS
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState)
                        /*menuButton.MouseClick() && menuButton.MouseContains()*/)
                    {
                        currentState = GameState.Menu;
                    }

                    //for when we do have the gode mode stuff implemented
                    //if (/*stealthModeButton.MouseClick() && stealthModeButton.MouseContains()*/)
                    //{
                    //    god mode on and off
                    //}
                    break;

                //all options for the state of playing the game
                case GameState.Game:

                    if (debug)
                    {
                        player.Update(gameTime);
                    }

                    if (!debug)
                    {
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        player.Update(gameTime);

                        #region Game Level Screen
                        switch (currentScreen)
                        {
                            case GameScreen.One:

                                if (player.Y == windowHeight)
                                {
                                    currentScreen = GameScreen.Two;
                                    player = level.Player;
                                }

                                if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                                {
                                    currentState = GameState.Menu;
                                }
                                break;
                                #region GameScreenTwo
                                //case GameScreen.Two:
                                //    if (player.Y == windowHeight)
                                //    {
                                //        currentScreen = GameScreen.Three;
                                //        player.ResetPosition();
                                //    }

                                //    if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                                //    {
                                //        currentState = GameState.Menu;
                                //    }
                                //    break;
                                //case GameScreen.Three:

                                //    if (player.Y == windowHeight)
                                //    {
                                //        currentScreen = GameScreen.Three;
                                //        player.ResetPosition();
                                //    }

                                //    if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                                //    {
                                //        currentState = GameState.Menu;
                                //    }

                                //    if (SingleKeyPress(Keys.Z, kbstate, previousKbState))
                                //    {
                                //        currentState = GameState.GameWin;
                                //    }
                                //    else if (SingleKeyPress(Keys.L, kbstate, previousKbState))
                                //    {
                                //        currentState = GameState.GameLose;
                                //    }

                                //    //to go back to main menu from game screen
                                //    else if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                                //    {
                                //        currentState = GameState.Menu;
                                //    }
                                //    break;
                                #endregion

                        }
                        #endregion

                        #region Collisions
                        //this method is to adjust the player's position with an non-overlappable object 
                        if (!player.IsHiding)
                        {
                            CheckObstacleCollision(player, level.ObstacleList);         //edge collision
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
                        if (timer <= 0 && collectiblesList.Count != 0)
                        {
                            currentState = GameState.GameLose;
                        }
                        else if (timer >= 0 && collectiblesList.Count == 0)
                        {
                            currentState = GameState.GameWin;
                        }
                    }


                    //PSEUDOCODE FOR THE FREEZING AND SLOWING
                    /*
                     ********ENEMY DOES NOT KEEP MOVING**************
                    IF ENEMY.LIGHTINTERSECTS == TRUE
                    Put in a freeze method that would tick down for a certain amount of time.
                        - Must be in a method to pause everything else within the game.
                    Once the timer runs out, the enemy TURNS
                        - Because the enemy has turned, the player will no longer intersect with the enemy light
                    Freeze method is done, and all player movement and enemy movement is in tact
                    */


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

                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("Press 'U' to toggle debug mode!\nDebug mode: " + debug),
                        new Vector2(0, 5),
                        Color.Pink);
                    break;
                case GameState.Options:

                    _spriteBatch.Draw(optionScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    // TEMP
                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("OPTIONS SCREEN"),
                        new Vector2(10, 100),
                        Color.White);
                    _spriteBatch.DrawString(
                        comicsans30,
                        string.Format("PRESS 'M' FOR MAIN MENU"),
                        new Vector2(10, 200),
                        Color.White);
                    break;

                case GameState.Game:

                    if (debug)
                    {
                        // DRAW ORDER: obstacle, collectibles, enemy, player

                        player.Draw(_spriteBatch, Color.White);

                        //drawing the timer to the screen
                        _spriteBatch.DrawString(
                            comicsans30,
                            string.Format("Time left: {0:0}", timer),
                            new Vector2(0, 5),
                            Color.White);
                    }

                    if (!debug)
                    {
                        // DRAW ORDER: player, collectibles, obstacle, enemy

                        player.Draw(_spriteBatch, Color.White);

                        //drawing the timer to the screen
                        _spriteBatch.DrawString(
                            comicsans30,
                            string.Format("Time left: {0:0}", timer),
                            new Vector2(0, 5),
                            Color.White
                            );

                        //draw all the collectibles
                        CollectibleDraw();

                        // LEVEL ------------------------------------------
                        
                        // draw obstacles based on player collision
                        for (int i = 0; i < obstaclesList.Count; i++)
                        {
                            //test code for hideable objects
                            
                            if (IsInRange(obstaclesList[i].Position, player) 
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
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            enemyList[i].Draw(_spriteBatch);
                        }

                        // draw the player based on collisions with light
                        foreach (Enemy enemy in enemyList)
                        {
                            bool collide = player.IndividualCollision(enemy.Position);

                            if (collide)
                            {
                                player.Draw(_spriteBatch, Color.Red);
                                break;
                            }

                            player.Draw(_spriteBatch, Color.White);
                        }

                        
                        
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

        // helper methods ------------------------------------------------------------
        private bool SingleKeyPress(
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
                bool collide = player.IndividualCollision(collectiblesList[i].Position);

                if (collide && SingleKeyPress(Keys.E, kbstate, previousKbState))
                {
                    collectiblesList.Remove(collectiblesList[i]);
                    i--;
                }
            }
        }

        /// <summary>
        /// draws the collectibles to the screen
        /// </summary>
        public void CollectibleDraw()
        {
            for (int i = 0; i < collectiblesList.Count; i++)
            {
                collectiblesList[i].Draw(_spriteBatch, Color.White);
            }
        }

        /// <summary>
        /// resets all the game conditions
        /// </summary>
        public void ResetGame()
        {
            collectiblesList.Clear();
            obstaclesList.Clear();
            enemyList.Clear();
            level.LoadLevel(levelName);

            collectiblesList = level.CollectiblesList;

            timer = 15;

            player = level.Player;
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
                    if (player.PRectangle.Intersects(obstacle.Position))
                    {
                        //Get the intersection area
                        Rectangle intersectionArea = Rectangle.Intersect(
                            player.PRectangle, obstacle.Position);

                        //If the width is less than the height, adjust the X position
                        if (intersectionArea.Width < intersectionArea.Height)
                        {
                            //LEFT side of obstacle
                            //player.X is less than the midpoint of the obstacle's width
                            if (player.X < (obstacle.Position.X + obstacle.Position.Width / 2))
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
                            if (player.Y < (obstacle.Position.Y + obstacle.Position.Height / 2))
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

            System.Diagnostics.Debug.WriteLine(player.IsHiding);

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
            float dx = Math.Abs((player.PRectangle.Width / 2) - (otherObject.Width / 2));
            float dy = Math.Abs((player.PRectangle.Height / 2) - (otherObject.Height / 2));

            if (
                //TODO: Check distance between objects
                //distance is based on midpoint of each object??
                (dx + 20) >= (player.PRectangle.X + player.PRectangle.Width) - (otherObject.X + otherObject.Width)
                && (dy + 20) >= (player.PRectangle.Y + player.PRectangle.Width) - (otherObject.Y + otherObject.Height)
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
        /// Press and release space bar
        /// </summary>
        /// 
        //TODO: Implement a way to exit hide state, otherwise player will be stuck within object. -Julia
        //^^ bool to allow for exit
        //DONE^^ -Jamie
        void Hide(KeyboardState prevState,
            KeyboardState curState, Obstacle otherObstacle, Player player)
        {
            //get mid points lined up
            if (IsInRange(otherObstacle.Position, player)
                && otherObstacle.IsHideable)
            {
                if (curState.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space) && !player.IsHiding)
                {
                    //These obstacles have to be the same size or larger than the player 
                    //Centers the player with the obstacle
                    player.X = (otherObstacle.Position.X + (otherObstacle.Position.Width / 2))
                        - (player.PRectangle.Width / 2);

                    player.Y = (otherObstacle.Position.Y + (otherObstacle.Position.Height / 2))
                        - (player.PRectangle.Height / 2);

                    player.IsHiding = true;
                }

                //if the space bar is pressed again then unhide
                else if (player.IsHiding)
                {
                    player.IsHiding = false;
                    //Add position changing stuff
                }
            }
        }


    }
}