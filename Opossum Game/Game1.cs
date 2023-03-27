﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        private Texture2D quitRollover;
        private Button quitButton;

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

        private Player player;
        // window fields
        private int windowWidth;
        private int windowHeight;

        //List of interactible objects
        private List<InteractibleObject> objects;

        //String to hold collision direction
        string obstacleCollision;

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

            // button sprites
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

            // option button
            optionsButtonBase = 
                Content.Load<Texture2D>("optionButtonBase");
            optionsButtonRollOver = 
                Content.Load<Texture2D>("optionButtonRollOver");

            // player sprite
            pSprite = Content.Load<Texture2D>("pSprite");
            // here bc pSprite is loaded here
            player = new Player(pSprite, new Rectangle(10, 10, pSprite.Width / 4, pSprite.Height / 4)); 

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
            
            //UI FINITE STATE MACHINES --------------------------------------------------------------------------
            kbstate = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.Menu:
                    // if player presses start button 
                    if (startButton.MouseClick() && startButton.MouseContains())
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
                    player.Update(gameTime);

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
            //---------------------------------------------------------------------------------------------

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
                    startButton.Draw(_spriteBatch);

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
                    player.Draw(_spriteBatch, new Rectangle()); //rectangle temp
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