using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and started loading in content, worked on temp fsm for update and draw
    /// Hui Lin: worked on enums and current state game state stuff
    /// Ariel: finalized UI fsm and implimented camera movement fsm
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
        #endregion

        #region KBState
        private KeyboardState kbstate;
        private KeyboardState previousKbState;
        #endregion

        // font fields
        private SpriteFont comicsans30;

        #region Window
        //literal window
        private int windowWidth;
        private int windowHeight;

        //List of interactible objects
        private List<InteractibleObject> objects;

        //String to hold collision direction
        string obstacleCollision;

        //all general window screens
        private Texture2D menuScreen;
        private Texture2D optionScreen;
        private Texture2D winScreen;
        private Texture2D loseScreen;

        private GameState currentState;
        #endregion

        //Obstacle test. Texture and rectangle and obstacle list
        private List<Obstacle> obstacleList;
        private Texture2D obstacleTexture;
        private Rectangle obstacleDimensions;
        private Obstacle testObstacle;
        private bool isColliding;

        //Placeholder light
        private Texture2D lightTexture;
        private Rectangle lightDimensions;
        private bool isCollidingLight;

        #region LevelLoading
        private StreamReader reader;
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;
        private string levelFile;
        private Level level;
        #endregion

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

            //Initializing list and default collision state
            objects = new List<InteractibleObject>();
            obstacleCollision = "none";

            //Test for list collision
            obstacleList = new List<Obstacle>();

            

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
                    startButtonBase2D.Width / 2,    // width of button
                    startButtonBase2D.Height / 2),  // height of button
                startButtonRollOver);   // rollover button texture

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
                ) ;
            

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
            pSprite = Content.Load<Texture2D>("pSprite");
            // player initialization
            player = new Player(pSprite, new Rectangle(10, 10, pSprite.Width / 4, pSprite.Height / 4));

            #region Collectibles
            collectibleBurger = Content.Load<Texture2D>("collectibleBurger");
            collectibleCandy = Content.Load<Texture2D>("collectibleCandy");
            collectibleChips = Content.Load<Texture2D>("collectibleChips");
            #endregion

            #region Game Screens
            menuScreen = Content.Load<Texture2D>("startScreen");
            //menuScreen
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

            //Temporary collision obstacle
            obstacleTexture = Content.Load<Texture2D>("collectibleBurger");
            obstacleDimensions = new Rectangle(400, 400, 200, 200);
            testObstacle = new Obstacle(obstacleTexture, obstacleDimensions);
            isColliding = false;

            //Temp light
            lightTexture = Content.Load<Texture2D>("light");
            lightDimensions = new Rectangle(200, 700, 200, 200);
            isCollidingLight = false;

            // level loading
            level = new Level(
                collectibleChips,   // collectible texture
                collectibleBurger,  // obstacle texture
                pSprite,            // player texture
                collectibleCandy);  // enemy texture
            level.LoadLevel("newTesterLevel");
            
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

            #region UI FSM
            kbstate = Keyboard.GetState();

            switch (currentState)
            {
                //all posibilities for the menu screen
                case GameState.Menu:

                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
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

                ////all options for the state of playing the game
                case GameState.Game:

                    player.Update(gameTime);

                    //collision stuff
                    isColliding = player.IndividualCollision(testObstacle);
                    if (isColliding)
                    {
                        player.Hide(kbstate, previousKbState, testObstacle.ObjectDimensions);
                    }
                    isCollidingLight = player.IndividualCollision(lightDimensions);


                    #region Game Level Screen
                    switch (currentScreen)
                    {
                        case GameScreen.One:

                            if (SingleKeyPress(Keys.Z, kbstate, previousKbState))
                            {
                                currentState = GameState.GameWin;
                            }
                            else if (SingleKeyPress(Keys.L, kbstate, previousKbState))
                            {
                                currentState = GameState.GameLose;
                            }

                            if (player.Y == windowHeight)
                            {
                                currentScreen = GameScreen.Two;
                                player.ResetPosition();
                            }

                            if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                            {
                                currentState = GameState.Menu;
                            }
                            break;
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


                    //Collision detection -- utilizing keyboard input to ensure player is attempting to move.
                    //Adjusts player direction in the opposite direction of their movement--should result in player staying still.
                    //If player movement speed is adjusted, this needs to be adjusted as well!
                    //updates collision string
                    /*obstacleCollision = player.ObstacleCollision(obstacleList);

                    //Only runs if collision isn't "none"
                    if (obstacleCollision != "none")
                    {
                        //Player moving down
                        if (obstacleCollision == "down" && kbstate.IsKeyDown(Keys.S))
                        {
                            player.Y -= 5;
                        }

                        //Player moving up
                        if (obstacleCollision == "up" && kbstate.IsKeyDown(Keys.W))
                        {
                            player.Y += 5;
                        }

                        //Player moving left
                        if (obstacleCollision == "left" && kbstate.IsKeyDown(Keys.A))
                        {
                            player.X += 5;
                        }

                        //Player moving right
                        if (obstacleCollision == "right" && kbstate.IsKeyDown(Keys.D))
                        {
                            player.X -= 5;
                        }
                    } */
            }

            // update the previous keyboard state
            previousKbState = kbstate;
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            #region UI FSM
            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.Menu:

                    _spriteBatch.Draw(menuScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    //drawing of the buttons
                    startButton.Draw(_spriteBatch);
                    optionsButton.Draw(_spriteBatch);
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

                    player.Draw(_spriteBatch, Color.White);

                    #region Game Level Screen
                    switch (currentScreen)
                    {
                        case GameScreen.One:
                            //_spriteBatch.Draw(gameScreen1, new Rectangle(0, 0, 900, 900), Color.White);

                            // TEMP
                            _spriteBatch.DrawString(
                                comicsans30,
                                string.Format("GAMEPLAY SCREEN"),
                                new Vector2(10, 100),
                                Color.White);

                            _spriteBatch.DrawString(
                                comicsans30,
                                string.Format("PRESS 'Z' FOR WIN OR 'L' FOR LOSE"),
                                new Vector2(10, 200),
                                Color.White);

                            break;

                        //case GameScreen.Two:

                        //    _spriteBatch.DrawString(
                        //        comicsans30,
                        //        string.Format("GAMEPLAY SCREEN"),
                        //        new Vector2(10, 100),
                        //        Color.White);
                        //    break;

                        //case GameScreen.Three:

                        //    _spriteBatch.DrawString(
                        //        comicsans30,
                        //        string.Format("GAMEPLAY SCREEN"),
                        //        new Vector2(10, 100),
                        //        Color.White);

                        //    _spriteBatch.DrawString(
                        //        comicsans30,
                        //        string.Format("PRESS 'Z' FOR WIN OR 'L' FOR LOSE"),
                        //        new Vector2(10, 200),
                        //        Color.White);
                        //    break;
                    }
                    #endregion

                    //test obstacle
                    if (isColliding)
                    {
                        testObstacle.Draw(_spriteBatch, Color.Green);
                    }
                    else
                    {
                        testObstacle.Draw(_spriteBatch, Color.White);
                    }

                    //test light
                    _spriteBatch.Draw(lightTexture, lightDimensions, Color.White);

                    if (isCollidingLight)
                    {
                        player.Draw(_spriteBatch, Color.Red);
                    }

                    // LEVEL TESTING ------------------------------------------
                    /*
                    player.Draw(_spriteBatch);
                    for (int i = 0; i < collectiblesList.Count; i++)
                    {
                        if (isColliding)
                        {
                            collectiblesList[i].Draw(_spriteBatch, Color.Blue);
                            break;
                        }
                        else
                        {
                            collectiblesList[i].Draw(_spriteBatch, Color.White);
                        }
                    }

                    for (int i = 0; i < obstaclesList.Count; i++)
                    {
                        if (isColliding)
                        {
                            obstaclesList[i].Draw(_spriteBatch, Color.Green);
                            break;
                        }
                        else
                        {
                            obstaclesList[i].Draw(_spriteBatch, Color.White);
                        }
                    }
                    */
                    
                    

                    break;

                case GameState.GameLose:

                    _spriteBatch.Draw(
                        loseScreen,
                        new Vector2(0,0),
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
            #endregion

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // helper methods ------------------------------------------------------------
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
    }
}