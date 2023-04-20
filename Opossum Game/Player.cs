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
        //private Texture2D pSpriteSide;
        //private Rectangle sideRectangle;

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

        //constructor
        /// <summary>
        /// Creates what the player will control.
        /// </summary>
        /// <param name="pSprite">The image to represent the player</param>
        /// <param name="pLocation">Dimensions are dependent on pSprite Texture2D</param>
        public Player(Texture2D pSprite, Rectangle pLocation)//Texture2D pSpriteSide, Rectangle sideRectangle)
        {
            //foodCollected = 0;
            this.pSprite = pSprite;
            this.playerRectangle = pLocation;
            isHiding = false;
            //this.pSpriteSide = pSpriteSide;
            //this.sideRectangle = sideRectangle;
        }


        /// <summary>
        /// processes EVERYTHING that affects player movement
        /// </summary>
        //TODO: Implement edge collision--block the player from moving off
        //    screen (left/right edges), or change panel (top/bottom edges) -Julia
        //DONE^^ See CheckObstacleCollision in Game1 - Jamie
        //TODO: is playerstate enum going to be used to change the direction the player is drawn in?
        //Otherwise the only use is for playdead, which could become a bool if that's the only use for the enum. -Julia
        private void ProcessInput()
        {
            //get current kbState
            currKB = Keyboard.GetState();

            //---------------------------------------------------------------
            //player can only move if they are not hiding in a box
            //TODO: tentatively adding PlayDead state as a check here.
            //This means that PlayDead resolution needs to set the player state to movement. -Julia
            if (!isHiding && playerState != PlayerState.PlayDead)
            {
                //Player movement. Gives priority of current state to front/back for potential drawing purposes.

                //W pressed
                if (currKB.IsKeyDown(Keys.W))
                {
                    playerState = PlayerState.Front;
                    playerRectangle.Y -= 5;
                    //sideRectangle.Y -= 5;
                }

                //S pressed 
                if (currKB.IsKeyDown(Keys.S))
                {
                    playerState = PlayerState.Back;
                    playerRectangle.Y += 5;
                    //sideRectangle.Y += 5;
                }

                //A pressed
                if (currKB.IsKeyDown(Keys.A))
                {
                    playerState = PlayerState.Left;
                    playerRectangle.X -= 5;
                    //sideRectangle.X -= 5;
                }

                //D pressed 
                if (currKB.IsKeyDown(Keys.D))
                {
                    playerState = PlayerState.Right;
                    playerRectangle.X += 5;
                   // sideRectangle.X += 5;
                }

                //Adjusting for diagonal movement. Decreases speed in the Y direction
                //Diagonally up
                if (currKB.IsKeyDown(Keys.W) && (currKB.IsKeyDown(Keys.A) || currKB.IsKeyDown(Keys.D)))
                {
                    playerRectangle.Y += 2;
                    //sideRectangle.Y += 2;
                }

                //Diagonally down
                if (currKB.IsKeyDown(Keys.S) && (currKB.IsKeyDown(Keys.A) || currKB.IsKeyDown(Keys.D)))
                {
                    playerRectangle.Y -= 2;
                    //sideRectangle.Y -= 2;
                }

                
            }

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
            //Only draws the player if they aren't hiding
            if (!isHiding)
            {
                //Rotates the player sprite based on the direction they are facing.
                //TODO: this is buggy. Commented out for now--figure it out during milestone 4, or cut.
                switch (playerState)
                {
                    //Facing right
                    /*case PlayerState.Right:
                        sb.Draw(
                            pSpriteSide,
                            sideRectangle,
                            null,
                            color,
                            (float)Math.PI/2,
                            new Vector2(pSpriteSide.Width/2, pSpriteSide.Height/2),
                            SpriteEffects.None,
                            0);
                        break;

                    //Facing left
                    case PlayerState.Left:
                        sb.Draw(
                            pSpriteSide,
                            sideRectangle,
                            null,
                            color,
                            (float)-Math.PI / 2,
                            new Vector2(pSpriteSide.Width/2, pSpriteSide.Height/2),
                            SpriteEffects.None,
                            0);
                        break; 
                    
                    //Facing back
                    case PlayerState.Back:
                        sb.Draw(
                            pSprite,
                            playerRectangle,
                            null,
                            color,
                            (float)Math.PI,
                            new Vector2(pSprite.Width / 2, pSprite.Height / 2),
                            SpriteEffects.None,
                            0);
                        break;

                    //Facing front--default version of sprite
                    case PlayerState.Front:
                        sb.Draw(
                            pSprite,
                            playerRectangle,
                            color);
                        break; */

                    default:
                        sb.Draw(
                            pSprite,
                            playerRectangle,
                            color);
                        break; 
                }
            }

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
