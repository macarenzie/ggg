using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and started loading in content, worked on temp fsm for update and draw
    /// Hui Lin: worked on enums and current state game state stuff
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

        // button fields


        // start menu and buttons
        private Texture2D startButtonBase2D;
        private Texture2D startButtonRollOver;
        private Texture2D startScreen;
        private Button startButtonBase;

        // options button
        private Texture2D optionsButtonBase;
        private Texture2D optionsButtonRollOver;

        // collectible fields
        private Texture2D collectibleBurger;
        private Texture2D collectibleCandy;
        private Texture2D collectibleChips;

        // player fields
        private GameState currentState;
        private Texture2D pSprite;

        // level fields

        // keyboard state tracking
        private KeyboardState kbstate;
        private KeyboardState previousKbState;

        // font fields
        private SpriteFont comicsans30;

        // window fields
        private int windowWidth;
        private int windowHeight;


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
            // TODO: Add your initialization logic here

            // store the dimesnions of the game window into a variable
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            // start the game state in menu
            currentState = GameState.Menu;

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // button sprites
            // start button
            startButtonBase2D = 
                Content.Load<Texture2D>("startButtonBase");
            startButtonRollOver = 
                Content.Load<Texture2D>("startButtonRollOver");
            startButtonBase = new Button(
                startButtonBase2D,      // initial button texture
                new Rectangle(
                    (windowWidth / 2) - (startButtonBase2D.Width / 4),          // x value
                    (windowHeight / 2) - (startButtonBase2D.Height / 4),        // y value
                    startButtonBase2D.Width / 2,    // width of button
                    startButtonBase2D.Height / 2),  // height of button
                startButtonRollOver);   // rollover button texture

            // option button
            optionsButtonBase = 
                Content.Load<Texture2D>("optionButtonBase");
            optionsButtonRollOver = 
                Content.Load<Texture2D>("optionButtonRollOver");

            // player sprite
            pSprite = Content.Load<Texture2D>("pSprite");

            // collectible sprites
            collectibleBurger = Content.Load<Texture2D>("collectibleBurger");
            collectibleCandy = Content.Load<Texture2D>("collectibleCandy");
            collectibleChips = Content.Load<Texture2D>("collectibleChips");

            //start screen
            startScreen =
                Content.Load<Texture2D>("startScreen");

            // temporary font
            comicsans30 = Content.Load<SpriteFont>("comicsans30");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            // get the current keyboard state
            kbstate = Keyboard.GetState();
            

            switch (currentState)
            {
                case GameState.Menu:
                    // if player presses start button

                    // PLACEHOLDER CODE TO TEST GAME STATE TRANSITIONS
                    if (SingleKeyPress(Keys.G, kbstate, previousKbState))
                    {
                        currentState = GameState.Game;
                    }

                    if (SingleKeyPress(Keys.O, kbstate, previousKbState))
                    {
                        currentState = GameState.Options;
                    }

                    // if player presses options button
                    break;
                case GameState.Options:

                    // PLACEHOLDER TO TEST TRANSITIONS
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState))
                    {
                        currentState = GameState.Menu;
                    }

                    break;
                case GameState.Game:

                    // PLACEHOLDER TO TEST TRANSITIONS
                    if (SingleKeyPress(Keys.Z, kbstate, previousKbState))
                    {
                        currentState = GameState.GameWin;
                    }
                    // PLACEHOLDER TO TEST TRANSITIONS
                    if (SingleKeyPress(Keys.L, kbstate, previousKbState))
                    {
                        currentState = GameState.GameLose;
                    }
                    break;
                case GameState.GameLose:
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState))
                    {
                        currentState = GameState.Menu;
                    }
                    break;
                case GameState.GameWin:
                    if (SingleKeyPress(Keys.M, kbstate, previousKbState))
                    {
                        currentState = GameState.Menu;
                    }
                    break;
            }

            // update the previous keyboard state
            previousKbState = kbstate;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //switch statement for diff screens and buttons that need to be drawn. 
            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.Menu:
                    _spriteBatch.Draw(startScreen, new Rectangle(0, 0, 900, 900), Color.White);
                    startButtonBase.Draw(_spriteBatch);

                    // TEMP
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'G' FOR GAME OR 'O' FOR OPTIONS"), 
                        new Vector2 (10, 100), 
                        Color.Red);
                    break;
                case GameState.Options:
                    // TEMP
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("OPTIONS SCREEN"), 
                        new Vector2(10, 100), 
                        Color.Red);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'M' FOR MAIN MENU"), 
                        new Vector2(10, 200), 
                        Color.Red);
                    break;
                case GameState.Game:
                    // TEMP
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("GAMEPLAY SCREEN"), 
                        new Vector2(10, 100), 
                        Color.Red);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'Z' FOR GAME WIN OR 'L' FOR GAME LOSE"), 
                        new Vector2(10, 200), 
                        Color.Red);
                    break;
                case GameState.GameLose:
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("GAME LOSE SCREEN"), 
                        new Vector2(10, 100), 
                        Color.Red);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'M' FOR MAIN MENU"), 
                        new Vector2(10, 200), 
                        Color.Red);
                    break;
                case GameState.GameWin:
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("GAME WIN SCREEN"), 
                        new Vector2(10, 100), 
                        Color.Red);
                    _spriteBatch.DrawString(
                        comicsans30, 
                        string.Format("PRESS 'M' FOR MAIN MENU"), 
                        new Vector2(10, 200), 
                        Color.Red);
                    break;
            }
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