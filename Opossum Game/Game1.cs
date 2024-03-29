﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.Runtime.CompilerServices;

namespace Opossum_Game
{
    /// <summary>
    /// McKenzie: added enums and did all level texture loading, assisted in level design,
    ///     player freeze mechanics, level switching, instructions, and code optimization
    /// Hui Lin: worked on enums and current state game state stuff
    /// Ariel: finalized UI fsm and implimented level switching / design, collectible mechanics,
    ///     player freeze mechanics, win/lose conditions, gameplay testing, and code optimization
    /// Jamie: edge collision so the player does not go over obstacles that the player
    ///     cannot go through and Hide()
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
        Debug,
        Instructions,
        Credits
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
        
        #region fields

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

        //exit button
        private Texture2D exitBase;
        private Texture2D exitRollOver;
        private Button exitButton;

        //tiny button for exit
        private Texture2D xMarkTexture;
        private Button xMarkButton;
        private Button xMarkButton2;

        //next and back buttons
        private Texture2D nextBase;
        private Button nextButton;
        private Texture2D backBase;
        private Button backButton;

        // play again
        private Texture2D playAgainBase;
        private Texture2D playAgainRollOver;
        private Button playAgainButton;
        private Button exitWinButton;

        // credits
        private Texture2D creditsBase;
        private Texture2D creditsRollOver;
        private Button creditsButton;

        #endregion

        #region Collectibles
        private Texture2D collectibleBurger;
        private Texture2D collectibleCandy;
        private Texture2D collectibleChips;
        private List<Texture2D> collectibleTextures;
        private int foodLeft;
        #endregion

        #region Player
        private Texture2D pSprite;
        private Player player;
        #endregion

        #region keyboard and mouse states
        private KeyboardState kbstate;
        private KeyboardState previousKbState;
        private MouseState mState;
        private MouseState prevMState;
        #endregion

        // font fields
        private SpriteFont comicsans25;

        //literal window
        private int windowWidth;
        private int windowHeight;

        //all general window screens
        private Texture2D menuScreen;
        private Texture2D optionScreen;
        private Texture2D winScreen;
        private Texture2D loseScreen;
        private Texture2D creditsScreen;

        private GameState currentState;

        // obstacle
        private Texture2D obstacleTexture;

        // enemy
        private Texture2D enemyTexture;

        // instructions pages
        private List<Texture2D> instructionsPage;
        private Texture2D instructionPage1;
        private Texture2D instructionPage2;
        private Texture2D instructionPage3;
        private Texture2D instructionPage4;
        private Texture2D instructionPage5;
        private int currentPage;

        #region Level
        // level lists
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;

        // level strings
        private string level1;
        private string level2;
        private string level3;
        private List<string> levelStrings;

        // level objects
        private Level lvl1;
        private Level lvl2;
        private Level lvl3;
        private List<Level> lvls;

        // level conditions
        private int levelCount;
        private double timer;
        Stopwatch frozenTimer;
        #endregion

        // DEBUG MODE
        private bool debug;
        private Texture2D debugX;
        private Texture2D debugBox;
        private Button debugButtonTrue;
        private Button debugButtonFalse;

        private Song gameMusic;

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
            // store the dimensions of the game window into a variable
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            // start the game state in menu
            currentState = GameState.Menu;

            //Initializing timer
            timer = 0;

            // debug mode
            debug = false;

            // instructions
            instructionsPage = new List<Texture2D>();

            // initialize the collectible texture list
            collectibleTextures = new List<Texture2D>();

            // level
            levelCount = -1;
            level1 = "levelScreen1";
            level2 = "levelScreen2";
            level3 = "levelScreen3";
            levelStrings = new List<string>();
            levelStrings.Add(level1);
            levelStrings.Add(level2);
            levelStrings.Add(level3);

            frozenTimer = new Stopwatch();

            base.Initialize(); 
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Buttons
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
            debugBox =
                Content.Load<Texture2D>("debugBox");
            debugX =
                Content.Load<Texture2D>("debugX");
            debugButtonTrue = new Button(
                debugBox,
                new Rectangle(
                     190,
                     680,
                     debugBox.Width,
                     debugBox.Height),
                debugBox
                );
            debugButtonFalse = new Button(
                debugBox,
                new Rectangle(
                     550,
                     675,
                     debugBox.Width,
                     debugBox.Height),
                debugBox
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

            xMarkButton2 = new Button(
                xMarkTexture,
                new Rectangle(
                    5,
                    0,
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

            //next button
            nextBase =
                Content.Load<Texture2D>("nextBase");
            backBase =
                Content.Load<Texture2D>("backBase");
            nextButton = new Button(
                nextBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) + 150,
                    770,
                    nextBase.Width / 3,
                    nextBase.Height / 3
                    ),
                nextBase
                );

            // back button
            backButton = new Button(
                backBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4) - 50,
                    770,
                    backBase.Width / 3,
                    backBase.Height / 3
                    ),
                backBase
                );

            //credits button
            creditsBase =
                Content.Load<Texture2D>("creditsBase");
            creditsRollOver =
                Content.Load<Texture2D>("creditsRollOver");
            creditsButton = new Button(
                creditsBase,
                new Rectangle(
                    (windowWidth / 2) - (optionsButtonBase.Width / 4),
                    (windowHeight / 2) + (optionsButtonBase.Height / 4) + 300,
                    optionsButtonBase.Width / 2,
                    optionsButtonBase.Height / 2
                    ),
                creditsRollOver
                );
               
            

            #endregion

            //background art
            environmentalArt = Content.Load<Texture2D>("environmentalArt");

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
            optionScreen = Content.Load<Texture2D>("optionsScreenAgain");

            //winScreen
            winScreen = Content.Load<Texture2D>("winScreen");

            //loseScreen
            loseScreen = Content.Load<Texture2D>("gameOverScreen");

            //credits screen
            creditsScreen = Content.Load<Texture2D>("creditsScreen");
            #endregion

            // instruction pages
            instructionPage1 = Content.Load<Texture2D>("instructionPage1");
            instructionPage2 = Content.Load<Texture2D>("instructionPage2");
            instructionPage3 = Content.Load<Texture2D>("instructionPage3");
            instructionPage4 = Content.Load<Texture2D>("instructionPage4");
            instructionPage5 = Content.Load<Texture2D>("instructionPage5");

            instructionsPage.Add(instructionPage1);
            instructionsPage.Add(instructionPage2);
            instructionsPage.Add(instructionPage3);
            instructionsPage.Add(instructionPage4);
            instructionsPage.Add(instructionPage5);

            // temporary font
            comicsans25 = Content.Load<SpriteFont>("comicsans30");

            // obstacle
            obstacleTexture = Content.Load<Texture2D>("obstacleTexture");

            // enemy
            enemyTexture = Content.Load<Texture2D>("newEnemyTexture");

            // level loading
            lvl1 = new Level(
                level1,
                collectibleTextures,    // collectible texture
                obstacleTexture,        // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture
            lvl2 = new Level(
                level2,
                collectibleTextures,    // collectible texture
                obstacleTexture,        // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture
            lvl3 = new Level(
                level3,
                collectibleTextures,    // collectible texture
                obstacleTexture,        // obstacle texture
                pSprite,                // player texture
                enemyTexture);          // enemy texture

            lvls = new List<Level>();
            lvls.Add(lvl1);
            lvls.Add(lvl2);
            lvls.Add(lvl3);

            // figure out how much food the player has to collect
            foreach (Level l in lvls)
            {
                foodLeft += l.CollectiblesList.Count;
            }

            //game music
            gameMusic = Content.Load<Song>("ringing_at_midnight-144085");
            MediaPlayer.Play(gameMusic);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // update the current keyboard and mouse states
            kbstate = Keyboard.GetState();
            mState = Mouse.GetState();

            switch (currentState)
            {
                #region MENU SCREEN ---------------------------------------------------------------
                case GameState.Menu:

                    // start the game
                    if (startButton.MouseClick() && startButton.MouseContains())
                    {
                        // reset instructions
                        currentPage = 0;
                        currentState = GameState.Instructions;
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

                    // go to the credits page
                    if (creditsButton.MouseClick() && creditsButton.MouseContains())
                    {
                        currentState = GameState.Credits;
                    }

                    //
                    if (debugButtonTrue.MouseClick() && debugButtonTrue.MouseContains() && !debug)
                    {
                        debug = true;
                    }
                    if (debugButtonFalse.MouseClick() && debugButtonFalse.MouseContains() && debug)
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
                #region CREDITS SCREEN ------------------------------------------------------------
                case GameState.Credits:
                    
                    // go back to the options
                    if (xMarkButton2.MouseClick() && xMarkButton2.MouseContains())
                    {
                        currentState = GameState.Options;
                    }
                    break;

                #endregion
                #region INSTRUCTIONS SCREEN -------------------------------------------------------
                case GameState.Instructions:
                   if (nextButton.MouseClick() && nextButton.MouseContains())
                   {
                        // increase the current page only by one
                        if (mState.LeftButton == ButtonState.Pressed
                            && prevMState.LeftButton != ButtonState.Pressed)
                        {
                            currentPage++;
                        }

                        // take the player to the game
                        if (currentPage >= 4)
                        {
                            //resetting the game
                            timer = 100;
                            NextLevel();
                            currentState = GameState.Game;
                        }
                    
                   }

                   // go back a page in the instructions
                   if (backButton.MouseClick() && backButton.MouseContains())
                   {
                       // decrease the current page only by one
                       if (mState.LeftButton == ButtonState.Pressed
                           && prevMState.LeftButton != ButtonState.Pressed)
                       {
                           currentPage--;
                       }

                       // take the player back to the main menu
                       if (currentPage < 0)
                       {
                           currentState = GameState.Menu;
                       }
                   }
                    break;

                #endregion
                #region GAMEPLAY SCREEN -----------------------------------------------------------
                case GameState.Game:
                    // advance to the next level
                    if (player.Y + player.Rect.Height < 0   // player goes to the exit
                        && levelCount < lvls.Count          // there are more levels left
                        && collectiblesList.Count == 0)     // all collectibles on the
                                                            //     screen are collected
                    {
                        ResetLevel();
                        NextLevel();
                    }

                    // stop the player from leaving the screen if all
                    //     collectibles haven't been collected
                    if (collectiblesList.Count != 0 && player.Y <= 0)
                    {
                        player.Y = 0;
                    }

                    // general game functions

                    // update player
                    player.Update(gameTime);

                    // update collectibles
                    CollectibleCollision();

                    // update enemy
                    foreach (Enemy e in enemyList)
                    {
                        e.Update(gameTime);
                    }

                    // everything that happens in debug mode
                    if (debug)
                    {
                        //if the game is still going w/o timer
                        if ((foodLeft >= 0) && levelCount != lvls.Count)
                        {
                            // determine if the player wants to hide
                            foreach (Obstacle obstacle in obstaclesList)
                            {

                                if (!player.IsHiding)
                                {
                                    Hide(previousKbState, kbstate, obstacle, player);
                                }
                            }

                            //different unhiding implementation
                            if (player.IsHiding)
                            {
                                UnHide(previousKbState, kbstate, obstaclesList, player);
                            }

                            //enemy obstacle collision
                            foreach (Enemy e in enemyList)
                            {
                                e.EnemyObstacleCollision(obstaclesList);
                            }
                        }
                        else if (foodLeft == 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameWin;
                        }
                    }
                    
                    //everything that happens in regular mode
                    if (!debug)
                    {
                        // decrease the timer
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        // win/lose conditions
                        if (timer <= 0 && foodLeft >= 0 && levelCount <= lvls.Count)
                        {
                            currentState = GameState.GameLose;
                        }
                        else if (timer >= 0 && foodLeft == 0 && levelCount == lvls.Count)
                        {
                            currentState = GameState.GameWin;
                        }
                        else
                        {
                            // determine enemy behavior
                            foreach (Enemy e in enemyList)
                            {
                                e.EnemyObstacleCollision(obstaclesList);

                                // create immunity buff after player escapes play dead state
                                if (player.IsImmune)
                                {
                                    //start timer to have player imunity
                                    frozenTimer.Start();
                                    if (frozenTimer.Elapsed.TotalSeconds > 3)
                                    {
                                        player.IsImmune = false;
                                        frozenTimer.Stop();
                                        frozenTimer.Reset();
                                    }
                                }
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

                            //collision checking
                            if (!player.IsHiding)
                            {
                                CheckObstacleCollision(player, obstaclesList);    //edge collision
                            }

                            // determine if the player is hiding
                            foreach (Obstacle obstacle in obstaclesList)
                            {
                                //check for hide attempts
                                if (!player.IsHiding)
                                {
                                    Hide(previousKbState, kbstate, obstacle, player);
                                }
                                
                            }

                            //check for unhiding attempts
                            if (player.IsHiding)
                            {
                                UnHide(previousKbState, kbstate, obstaclesList, player);
                            }
                        }
                    }

                    //x mark button
                    if (xMarkButton2.MouseClick() && xMarkButton2.MouseContains())
                    {
                        
                        ResetLevel();
                        ResetGame();
                        currentState = GameState.Menu;
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
                        timer = 100;
                        NextLevel();
                        currentState = GameState.Game;
                    }

                    //to exit the game from gameLose
                    if (quitButton.MouseClick() && quitButton.MouseContains())
                    {
                        ResetLevel();
                        ResetGame();
                        currentState = GameState.Menu;
                    }
                    break;
                #endregion
                #region GAME WIN SCREEN -----------------------------------------------------------
                case GameState.GameWin:
                    
                    if (playAgainButton.MouseClick() && playAgainButton.MouseContains())
                    {
                        ResetLevel();
                        ResetGame();
                        timer = 100;
                        NextLevel();
                        currentState = GameState.Game;
                    }

                    //to exit the game from gameWin
                    if (exitWinButton.MouseClick() && exitWinButton.MouseContains())
                    {
                        ResetLevel();
                        ResetGame();
                        currentState = GameState.Menu;
                    }
                    break;
                    #endregion
            }

            // update the previous keyboard and mouse state
            previousKbState = kbstate;
            prevMState = mState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

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

                    //Draw the debug buttons
                    debugButtonTrue.Draw(_spriteBatch);
                    debugButtonFalse.Draw(_spriteBatch);

                    //Indication checkmarks
                    if (debug)
                    {
                        _spriteBatch.Draw(debugX,
                            new Rectangle(
                                190,
                                680,
                                debugX.Width,
                                debugX.Height
                                ),
                            Color.White);
                    } else
                    {
                        _spriteBatch.Draw(debugX,
                            new Rectangle(
                                550,
                                675,
                                debugX.Width,
                                debugX.Height
                                ),
                            Color.White);
                    }

                    creditsButton.Draw(_spriteBatch);

                    break;
                #endregion
                #region CREDITS SCREEN ------------------------------------------------------------
                case GameState.Credits:

                    // go back to the options
                    
                    _spriteBatch.Draw(creditsScreen, new Rectangle(0, 0, 900, 900), Color.White);
                    xMarkButton2.Draw(_spriteBatch);

                    break;

                #endregion
                #region INSTRUCTIONS SCREEN -------------------------------------------------------
                case GameState.Instructions:

                    // draw the correct instructions page
                    if (currentPage >= 0)
                    {
                        _spriteBatch.Draw(
                            instructionsPage[currentPage], 
                            new Rectangle(0, 0, 900, 900), 
                            Color.White);
                    }
                   
                    // buttons
                    nextButton.Draw(_spriteBatch);
                    backButton.Draw(_spriteBatch);

                    break;

                #endregion
                #region GAMEPLAY SCREEN -----------------------------------------------------------
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
                                obstacle.Draw(_spriteBatch, Color.DarkGoldenrod);
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
                                obstacle.Draw(_spriteBatch, Color.DarkGoldenrod);
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
                        comicsans25,
                        string.Format("Time left: {0:0}", timer),
                        new Vector2(80, 5),
                        Color.White);

                    // draw the exit button
                    xMarkButton2.Draw(_spriteBatch);

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
        /// checks for singlekey press
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

        #region Player Methods

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
                //&& !player.IsHiding
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
            KeyboardState curState, List<Obstacle> otherObstacles, Player player)
        {
            //need to create a new rectangle. otherwise if
            //potentialPlayer = player.Rect then we have a case of polymorphism and
            //it's just the same rectangle rather than a copy of its values
            Rectangle potentialPlayer = new Rectangle(
                player.X, 
                player.Y,
                player.Rect.Width,
                player.Rect.Height
                );
            int numberOfIntersecting = 0;

            //if WASD is pressed again then unhide
            //Also check if the direction the player wants to unhide from is valid
            if (player.IsHiding)
            {
                //W; Up direction
                if (SingleKeyPress(Keys.W, curState, prevState))
                {
                    //adjust to potential coordinates
                    potentialPlayer.Y -= potentialPlayer.Height;

                    //loop through every object in the otherObstacles list
                    //Then check after interating through all of the items
                    //If there was a single collision
                    for (int i = 0; i < otherObstacles.Count; i++)
                    {
                        //check for intersection. If intersection is true, then increment
                        if (potentialPlayer.Intersects(otherObstacles[i].Rect))
                        {
                            numberOfIntersecting++;
                        }

                        //always checking against the potential player. as such do not
                        //have to worry about intersecting with the box you are hiding in
                        //Although with the Height and Width of the player, there is a potential
                        //to check against the box the player is hiding in in the X-axis
                        //Therefore the <= 1 is insurance instead of <= 0
                        if (numberOfIntersecting <= 1 && i >= otherObstacles.Count - 1)
                        {
                            player.IsHiding = false; //change bool

                            //position changing logic
                            player.Rect = potentialPlayer;
                        }
                    }

                }

                //A; Left Direction
                else if (SingleKeyPress(Keys.A, curState, prevState))
                {
                    potentialPlayer.X -= potentialPlayer.Width;

                    for (int i = 0; i < otherObstacles.Count; i++)
                    {                        
                        if (potentialPlayer.Intersects(otherObstacles[i].Rect))
                        {
                            numberOfIntersecting++;
                        }

                        if (numberOfIntersecting <= 1 && i >= otherObstacles.Count - 1)
                        {
                            player.IsHiding = false; 
                            player.Rect = potentialPlayer;
                        }
                    }
                        
                }

                //S; Down Direction
                else if (SingleKeyPress(Keys.S, curState, prevState))
                {
                    potentialPlayer.Y += potentialPlayer.Height;

                    for (int i = 0; i < otherObstacles.Count; i++)
                    {
                        if (potentialPlayer.Intersects(otherObstacles[i].Rect))
                        {
                            numberOfIntersecting++;
                        }

                        if (numberOfIntersecting <= 1 && i >= otherObstacles.Count - 1)
                        {
                            player.IsHiding = false; 
                            player.Rect = potentialPlayer;
                        }
                    }

                    
                }

                //D; Right Direction
                else if (SingleKeyPress(Keys.D, curState, prevState))
                {
                    potentialPlayer.X += potentialPlayer.Width;

                    for (int i = 0; i < otherObstacles.Count; i++)
                    {
                        if (potentialPlayer.Intersects(otherObstacles[i].Rect))
                        {
                            numberOfIntersecting++;
                        }

                        if (numberOfIntersecting <= 1 && i >= otherObstacles.Count - 1)
                        {
                            player.IsHiding = false; 
                            player.Rect = potentialPlayer;
                        }
                    }

                    
                }
            }
        }

        #endregion

        #region level and game advancement/reset methods
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
            // reload the level content into the corresponding level objects
            for (int i = 0; i < lvls.Count; i++)
            {
                // only fill the game object lists if there is objects in them
                if (lvls[i].CollectiblesList.Count == 0)
                {
                    lvls[i].LoadLevel(levelStrings[i]);
                }
            }

            // reset the level counter
            levelCount = -1;

            // reload the food counter
            foodLeft = 0;
            foreach (Level l in lvls)
            {
                foodLeft += l.CollectiblesList.Count;
            }

            // reset the player condition
            player.PlayerState = PlayerState.Front;
            player.IsHiding = false;
            player.IsImmune = false;
            player.FreezeTimer.Stop();
            player.FreezeTimer.Reset();
            frozenTimer.Stop();
            frozenTimer.Reset();
        }

        #endregion
    }
}