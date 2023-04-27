using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Opossum_Game
{
    /// <summary>
    /// Will represent the object the player is controlling
    /// Majority Written by: Jamie Zheng
    /// Code optimization / freeze mechanics done by Ariel and McKenzie
    /// Some edits and optimization done by Julia
    /// </summary>
    internal class Player : IGameObject
    {
        //Player dimensions and sprite
        private Rectangle playerRectangle; 
        private Texture2D pSprite;

        //States for keyboard input and corresponding player movement
        private KeyboardState currKB;
        private KeyboardState prevKB;
        private PlayerState playerState;
        private PlayerState prevState;

        //Tracks if the player is currently hiding
        private bool isHiding;

        //Immmunity after playdead
        private bool isImmune;

        //Freeze timer
        private Stopwatch freezeTimer;

        /// <summary>
        /// Gets and sets the player texture
        /// </summary>
        public Texture2D Sprite
        {
            get
            {
                return pSprite;
            }
            set
            {
                pSprite = value;
            }
        }


        /// <summary>
        /// Gets and sets player's x coordinate
        /// </summary>
        public int X
        {
            get { return playerRectangle.X; }
            set { playerRectangle.X = value; }
        }

        /// <summary>
        /// Gets and sets player's y coordinate
        /// </summary>
        public int Y
        {
            get { return playerRectangle.Y; }
            set { playerRectangle.Y = value; }
        }

        /// <summary>
        /// Returns and sets the Rectangle associated with the Player.
        /// </summary>
        public Rectangle Rect
        {
            get { return playerRectangle; }
            set { playerRectangle = value; }
        }

        /// <summary>
        /// Get and Set, returns whether or not a player is hiding within another obstacle
        /// See Game1 for Hide()
        /// </summary>
        public bool IsHiding
        {
            get { return isHiding; }
            set { isHiding = value; }
        }

        public bool IsImmune
        {
            get { return isImmune; }
            set { isImmune = value; }
        }

        /// <summary>
        /// Returns the current state of the player
        /// </summary>
        public PlayerState PlayerState
        {
            get { return playerState; }
        }

        /// <summary>
        /// Creates the user-controlled player object
        /// </summary>
        /// <param name="pSprite">The image to represent the player</param>
        /// <param name="pLocation">Dimensions are dependent on pSprite Texture2D</param>
        public Player(Texture2D pSprite, Rectangle pLocation)
        {
            this.pSprite = pSprite;
            this.playerRectangle = pLocation;
            isHiding = false;
            freezeTimer = new Stopwatch();
            isImmune = false;
        }


        /// <summary>
        /// Processes keyboard input for player movement
        /// Also freezes the player when in playdead state
        /// </summary>
        private void ProcessInput()
        {
            //get current kbState
            currKB = Keyboard.GetState();

            //Moves player in specified direction if not frozen or hiding
            if (!isHiding && playerState != PlayerState.PlayDead)
            {
                //W pressed
                if (currKB.IsKeyDown(Keys.W))
                {
                    playerState = PlayerState.Front;
                    playerRectangle.Y -= 5;
                }

                //S pressed 
                if (currKB.IsKeyDown(Keys.S))
                {
                    playerState = PlayerState.Back;
                    playerRectangle.Y += 5;

                    //Prevents player from moving down off the screen
                    if (playerRectangle.Y + playerRectangle.Height >= 900)
                    {
                        playerRectangle.Y = 900 - playerRectangle.Height;
                    }
                }

                //A pressed
                if (currKB.IsKeyDown(Keys.A))
                {
                    playerState = PlayerState.Left;
                    playerRectangle.X -= 5;

                    //Prevents the player from moving off screen to the left
                    if (playerRectangle.X <= 0)
                    {
                        playerRectangle.X = 0;
                    }
                }

                //D pressed 
                if (currKB.IsKeyDown(Keys.D))
                {
                    playerState = PlayerState.Right;
                    playerRectangle.X += 5;

                    //Prevents the player from moving off screen to the right
                    if (playerRectangle.X + playerRectangle.Width >= 900)
                    {
                        playerRectangle.X = 900 - playerRectangle.Width;
                    }
                }

                //Adjusting for diagonal movement. Decreases speed in the Y direction
                //Diagonally up
                if (currKB.IsKeyDown(Keys.W) && (currKB.IsKeyDown(Keys.A) || currKB.IsKeyDown(Keys.D)))
                {
                    playerRectangle.Y += 2;
                }

                //Diagonally down
                if (currKB.IsKeyDown(Keys.S) && (currKB.IsKeyDown(Keys.A) || currKB.IsKeyDown(Keys.D)))
                {
                    playerRectangle.Y -= 2;
                }

                
            }

            //Temporarily freezes the player when playing dead
            if (!isHiding && playerState == PlayerState.PlayDead)
            {
                //Timer lasts for 3 seconds
                freezeTimer.Start();

                //Changes the movement direction to be opposite what the last movement direction was.
                //Also resets stopwatch for next use.
                if (freezeTimer.Elapsed.TotalSeconds > 3)
                {
                    //Stops the timer
                    freezeTimer.Stop();

                    //Player becomes immune to being frozen for a short time
                    isImmune = true;

                    //Sets player state to the recorded state before play dead
                    switch (prevState)
                    {
                        case PlayerState.Front:
                            playerState = PlayerState.Front;
                            break;

                        case PlayerState.Back:
                            playerState = PlayerState.Back;
                            break;

                        case PlayerState.Left:
                            playerState = PlayerState.Left;
                            break;

                        case PlayerState.Right:
                            playerState = PlayerState.Right;
                            break;
                    }

                    //Resets the timer used for freezing the player
                    freezeTimer.Reset();
                }
            }

            //update prevKB
            prevKB = currKB;
        }

        /// <summary>
        /// Runs the process input method
        /// </summary>
        public void Update(GameTime gameTime)
        {
            ProcessInput();         
        }

        /// <summary>
        /// Draws the player with a different tint depending on current state
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public void Draw(SpriteBatch sb, Color color)
        {
            //Only draws the player if they aren't hiding
            if (!isHiding)
            {
                //Draws the player with a blue tint when frozen
                if (playerState == PlayerState.PlayDead)
                {
                    sb.Draw
                        (
                        pSprite,
                        playerRectangle,
                        Color.SteelBlue);
                }

                //Draws the player with a yellow tint if immune to being stopped.
                else if (isImmune)
                {
                    sb.Draw(
                        pSprite, 
                        playerRectangle,
                        Color.Yellow);
                }

                //Draws the player normally
                else
                {
                    sb.Draw(
                        pSprite,
                        playerRectangle,
                        color);
                }
            }

        }

        /// <summary>
        /// Detects collision with individual obstacles
        /// </summary>
        /// <param name="obstacle">The rectangle associated with the object in question</param>
        /// <returns>Collision status--if the player is currently colliding with an object</returns>
        public bool IndividualCollision(Rectangle obstacle)
        {
            //Checks if the rectangles intersect, returns true if so
            if (playerRectangle.Intersects(obstacle))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Checks if the player has collided with an enemy
        /// Sets the player to the playdead state if so
        /// </summary>
        /// <param name="enemy">Enemy rectangle</param>
        public void intersectsEnemy(Rectangle enemy)
        {
            //Checks if the player rectangle intersects the enemy
            if (playerRectangle.Intersects(enemy))
            {
                //Updates previous player state 
                if (playerState != PlayerState.PlayDead)
                {
                    prevState = playerState;
                }
                
                //Sets current state to play dead
                playerState = PlayerState.PlayDead;
            }
        }
    }
}
