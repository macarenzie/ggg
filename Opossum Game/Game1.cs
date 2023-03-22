using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and started loading in content
    /// Hui Lin: worked on enums and current state game state stuff
    /// </summary>
    #region Enums
    public enum GameState
    {
        Menu,
        Load,
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
            //900 / 2 = 450
            //450 - 250 X 
            //450 - 100  Y
            

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
            startButtonBase = new Button(startButtonBase2D, new Rectangle(200, 350, 250, 100));

            // option button
            optionsButtonBase = 
                Content.Load<Texture2D>("optionButtonBase");
            optionsButtonRollOver = 
                Content.Load<Texture2D>("optionButtonRollOver");

            // player sprite

            // collectible sprites

            //start screen
            startScreen =
                Content.Load<Texture2D>("startScreen");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentState = GameState.Menu;

            switch (currentState)
            {
                case GameState.Menu:
                    break;
                case GameState.Load:
                    break;
                case GameState.GameLose:
                    break;
                case GameState.GameWin:
                    break;
            }

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
                    break;
                case GameState.Load:
                    break;
                case GameState.GameLose:
                    break;
                case GameState.GameWin:
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}