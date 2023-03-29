using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and started loading in content, worked on temp fsm for update and draw
    /// Hui Lin: worked on enums and current state game state stuff
    /// Ariel: finalized UI fsm and implimented camera movement fsm
    /// </summary>
    #region Enums
    public enum GameState
    {
        Menu,
        Options,
        Game,
        GameLose,
        GameWin
    }
    public enum PlayerState
    {
        Front,
        Back,
        Left,
        Right,
        PlayDead
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
        private GameState currentState;
        private Texture2D pSprite;
        private Player player;
        #endregion

        #region Level
        // "camera" textures
        private Texture2D gameScreen1;
        private Texture2D gameScreen2;
        private Texture2D gameScreen3;
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

            //Initializing list and default collision state
            objects = new List<InteractibleObject>();
            obstacleCollision = "none";

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
                quitBase,
                new Rectangle(
                    ((windowWidth / 2) - 200) - (tryAgainBase.Width / 4),                   //x value
                    ((windowHeight / 2) - 150) - (tryAgainBase.Height / 4),                 //y value
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
                case GameState.Menu:
                    // if player presses start button 
                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
                        currentState = GameState.Game;
                    }

                    if (SingleKeyPress(Keys.O, kbstate, previousKbState)
                        /*optionsButton.MouseClick() && optionsButton.MouseContains()*/)
                    {
                        currentState = GameState.Options;
                    }

                    //to exit the game from menu
                    //if (exitButton.MouseClick() && exitButton.MouseContains())
                    //{
                    //  Exit();
                    //}
                    break;
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
                case GameState.Game:
                    player.Update(gameTime);

                    // game win conditions
                    if (SingleKeyPress(Keys.Z, kbstate, previousKbState))
                    {
                        currentState = GameState.GameWin;
                    }
                    // game lose conditions
                    if (SingleKeyPress(Keys.L, kbstate, previousKbState))
                    {
                        currentState = GameState.GameLose;
                    }

                    //to go back to main menu from game screen
                    if (SingleKeyPress(Keys.Escape, kbstate, previousKbState))
                    {
                        currentState = GameState.Menu;
                    }

                    break;
                case GameState.GameLose:
                    //go back to menue
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState)
                        /*menuButton.MouseClick() && menuButton.MouseContains()*/)
                    {
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameLose
                    //if (exitButton.MouseClick() && exitButton.MouseContains())
                    //{
                    //  Exit();
                    //}
                    break;
                case GameState.GameWin:
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState)
                        /*menuButton.MouseClick() && menuButton.MouseContains()*/)
                    {
                        currentState = GameState.Menu;
                    }

                    //to exit the game from gameWin
                    //if (exitButton.MouseClick() && exitButton.MouseContains())
                    //{
                    //  Exit();
                    //}
                    break;
            }

            //Collision detection -- utilizing keyboard input to ensure player is attempting to move.
            //Adjusts player direction in the opposite direction of their movement--should result in player staying still.
            //If player movement speed is adjusted, this needs to be adjusted as well!
            //updates collision string
            obstacleCollision = player.ObstacleCollision(objects);

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
            }
            
            // update the previous keyboard state
            previousKbState = kbstate;
            #endregion

            #region Camera Finite State Machine

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            //theoretically the game screens should load, however! idk how were gonna consider obstacles +
            //i think thats mckenzies job - ariel
            #region UI FSM
            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.Menu:
                    _spriteBatch.Draw(menuScreen, new Rectangle(0, 0, 900, 900), Color.White);
                    startButton.Draw(_spriteBatch);
                    break;
                case GameState.Options:
                    //_spriteBatch.Draw(optionScreen, new Rectangle(0, 0, 900, 900), Color.White);

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
                    player.Draw(_spriteBatch, new Rectangle()); //rectangle temp
                    break;
                case GameState.GameLose:
                    //_spriteBatch.Draw(loseScreen, new Rectangle(0, 0, 900, 900), Color.White);

                    //TEMP
                    _spriteBatch.Draw(
                        loseScreen,
                        new Vector2(0,0),
                        Color.White
                        );
                    quitButton.Draw(_spriteBatch);
                    tryAgainButton.Draw(_spriteBatch);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("GAME LOSE SCREEN"), 
                        new Vector2(10, 100), 
                        Color.White);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'M' FOR MAIN MENU"), 
                        new Vector2(10, 200), 
                        Color.White);
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

        // methods ------------------------------------------------------------
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