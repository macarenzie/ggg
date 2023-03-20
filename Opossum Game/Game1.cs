using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Opossum_Game
{
    /// <summary>
    /// Worked on by: McKenzie Lam
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
        // start button
        private Texture2D startButtonBase;
        private Texture2D startButtonRollOver;

        // options button
        private Texture2D optionsButtonBase;
        private Texture2D optionsButtonRollOver;

        // collectible fields

        // player fields
        private GameState currentState;

        // level fields

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // button sprites
            // start button
            startButtonBase = 
                Content.Load<Texture2D>("startButtonBase");
            startButtonRollOver = 
                Content.Load<Texture2D>("startButtonRollOver");

            // option button
            optionsButtonBase = 
                Content.Load<Texture2D>("optionButtonBase");
            optionsButtonRollOver = 
                Content.Load<Texture2D>("optionButtonRollOver");

            // player sprite

            // collectible sprites
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
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

            base.Draw(gameTime);
        }
    }
}