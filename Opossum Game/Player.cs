using System;
using System.Collections.Generic;
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
    /// Code optimization done by Ariel and McKenzie
    /// </summary>
    internal class Player : IGameObject
    {
        //fields
        private Rectangle playerRectangle; //dimensions pSprite dimensions
        private Texture2D pSprite;

        //player movement stuff
        private KeyboardState currKB;
        private KeyboardState prevKB;
        private PlayerState playerState;

        //hiding stuff
        private bool isHiding;

        //properties
        /// <summary>
        /// gets and sets the player texture
        /// </summary>
        public Texture2D Texture
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
        /// gets and sets player's x coordinate
        /// </summary>
        public int X
        {
            get { return playerRectangle.X; }
            set { playerRectangle.X = value; }
        }

        /// <summary>
        /// gets and sets player's y coordinate
        /// </summary>
        public int Y
        {
            get { return playerRectangle.Y; }
            set { playerRectangle.Y = value; }
        }

        /// <summary>
        /// Returns the Rectangle associated with the Player.
        /// Get only, although the X and Y properties allow for set
        /// No reason to change dimensions
        /// </summary>
        public Rectangle Rectangle
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

        //constructor
        /// <summary>
        /// Creates what the player will control.
        /// </summary>
        /// <param name="pSprite">The image to represent the player</param>
        /// <param name="pLocation">Dimensions are dependent on pSprite Texture2D</param>
        public Player(Texture2D pSprite, Rectangle pLocation)
        {
            //foodCollected = 0;
            this.pSprite = pSprite;
            this.playerRectangle = pLocation;
            isHiding = false;
        }


        /// <summary>
        /// processes EVERYTHING that affects player movement
        /// </summary>
        //TODO: Implement edge collision--block the player from moving off
        //    screen (left/right edges), or change panel (top/bottom edges) -Julia
        //DONE^^ See CheckObstacleCollision in Game1 - Jamie
        //TODO: Diagonal movement doesn't work right now. Could potentially
        //   only use enum switch for draw purposes. -Julia
        private void ProcessInput()
        {
            //get current kbState
            currKB = Keyboard.GetState();

            //---------------------------------------------------------------
            //player can only move if they are not hiding in a box
            if (!isHiding)
            {
                switch (playerState)
                {
                    //===================================================================
                    case PlayerState.Left:
                        //if A is pressed
                        if (currKB.IsKeyDown(Keys.A))
                        {
                            playerRectangle.X -= 5;
                        }

                        //TRANSITIONS
                        if (currKB.IsKeyDown(Keys.D) && prevKB.IsKeyUp(Keys.D))
                            playerState = PlayerState.Right;

                        if (currKB.IsKeyDown(Keys.W) && prevKB.IsKeyUp(Keys.W))
                            playerState = PlayerState.Front;

                        if (currKB.IsKeyDown(Keys.S) && prevKB.IsKeyUp(Keys.S))
                            playerState = PlayerState.Back;

                        break;
                    //===================================================================
                    case PlayerState.Right:
                        //if D is pressed
                        if (currKB.IsKeyDown(Keys.D))
                        {
                            playerRectangle.X += 5;
                        }

                        //TRANSITIONS
                        if (currKB.IsKeyDown(Keys.A) && prevKB.IsKeyUp(Keys.A))
                            playerState = PlayerState.Left;

                        if (currKB.IsKeyDown(Keys.W) && prevKB.IsKeyUp(Keys.W))
                            playerState = PlayerState.Front;

                        if (currKB.IsKeyDown(Keys.S) && prevKB.IsKeyUp(Keys.S))
                            playerState = PlayerState.Back;

                        break;

                    case PlayerState.Front:
                        //if W is pressed
                        if (currKB.IsKeyDown(Keys.W))
                        {
                            playerRectangle.Y -= 5;
                        }

                        //TRANSITIONS
                        if (currKB.IsKeyDown(Keys.A) && prevKB.IsKeyUp(Keys.A))
                            playerState = PlayerState.Left;

                        if (currKB.IsKeyDown(Keys.D) && prevKB.IsKeyUp(Keys.D))
                            playerState = PlayerState.Right;

                        if (currKB.IsKeyDown(Keys.S) && prevKB.IsKeyUp(Keys.S))
                            playerState = PlayerState.Back;

                        break;
                    //===================================================================
                    case PlayerState.Back:
                        //if S is pressed
                        if (currKB.IsKeyDown(Keys.S))
                        {
                            playerRectangle.Y += 5;
                        }

                        //TRANSITIONS
                        if (currKB.IsKeyDown(Keys.A) && prevKB.IsKeyUp(Keys.A))
                            playerState = PlayerState.Left;

                        if (currKB.IsKeyDown(Keys.D) && prevKB.IsKeyUp(Keys.D))
                            playerState = PlayerState.Right;

                        if (currKB.IsKeyDown(Keys.W) && prevKB.IsKeyUp(Keys.W))
                            playerState = PlayerState.Front;

                        break;
                        //===================================================================
                        //will update this when enemy obstacles have been made
                        //case PlayerState.PlayDead:
                        //    if (Collision() == true)
                        //    {
                        //        pLocation.X += 0;
                        //        pLocation.Y += 0;
                        //    }
                        //    break;
                }
            }
            else
            {
                //logically, the player can't move in this else
                //Do not move
                //playerRectangle.X += 0;
                //playerRectangle.Y += 0;
            }
            
            //---------------------------------------------------------------

            //update prevKB
            prevKB = currKB;
        }

        /// <summary>
        /// all update stuff wtihin player
        /// </summary>
        /// <param name="prevState"></param>
        /// <param name="curState"></param>
        public void Update(GameTime gameTime)
        {
            ProcessInput();
        }

        /// <summary>
        /// Draw the player to the screen, highlight if collision with light is true 
        /// maybe have the light collision be handled in level manager. just a thought --Jamie
        /// </summary>
        //Color specifier is a TEMP until playdead is implemented
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(
                pSprite,
                playerRectangle,
                color
                );

        }

        /// <summary>
        /// detects individual collisions with different game objects
        /// </summary>
        /// <param name="obstacle">The rectangle associated w. the object in question</param>
        /// <returns>whether the player is colliding with another object</returns>
        public bool IndividualCollision(Rectangle obstacle)
        {
            if (playerRectangle.Intersects(obstacle)) //And !isHidden
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
