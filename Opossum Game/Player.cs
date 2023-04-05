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
    /// COde optimization done by Ariel and McKenzie
    /// </summary>
    internal class Player
    {
        //fields
        private Rectangle pLocation; //dimensions pSprite dimensions
        private Texture2D pSprite;
        private int health;
        private int foodCollected;

        //player movement stuff
        private KeyboardState currKB;
        private KeyboardState prevKB;
        private PlayerState playerState;

        //properties
        /// <summary>
        /// gets and sets the player texture
        /// </summary>
        public Texture2D PSprite
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
        /// The amount of food the player has collected
        /// </summary>
        public int FoodCollected
        {
            get { return foodCollected; }
        }

        /// <summary>
        /// The amount of health the player has
        /// </summary>
        public int Health
        {
            get { return health; }  
        }

        /// <summary>
        /// gets and sets player's x coordinate
        /// </summary>
        public int X
        {
            get { return pLocation.X; }
            set { pLocation.X = value; }
        }

        /// <summary>
        /// gets and sets player's y coordinate
        /// </summary>
        public int Y
        {
            get { return pLocation.Y; }
            set { pLocation.Y = value; }
        }

        //constructor
        /// <summary>
        /// Creates what the player will control.
        /// </summary>
        /// <param name="pSprite">The image to represent the player</param>
        /// <param name="pLocation">Dimensions are dependent on pSprite Texture2D</param>
        public Player(Texture2D pSprite, Rectangle pLocation)
        {
            health = 3;
            foodCollected = 0;
            this.pSprite = pSprite;
            this.pLocation = pLocation;
        }

        //Method for detecting individual collision with obstacles
        /// <summary>
        /// detects individual collisions with different game objects
        /// </summary>
        /// <param name="obstacle"></param>
        /// <returns></returns>
        public bool IndividualCollision(Rectangle obstacle)
        {
            if (pLocation.Intersects(obstacle)) //And !isHidden
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// player's collecting food method based on user input
        /// </summary>
        /// <param name="prevState">keyboard's previous state</param>
        /// <param name="curState">keyboard's current state</param>
        /// <param name="otherObject"></param>
        public void Collect(KeyboardState prevState, KeyboardState curState, Collectible food)
        {
            //TODO: Check for press and release of space bar
            //Only collect if collectible is in range, check if collectible is collectible
            //Complete IsInRange() method before this one
            if(prevState.IsKeyDown(Keys.Space) &&               //key release check
               curState.IsKeyUp(Keys.Space) &&
                IsInRange(food.ObjectDimensions))      //Check if in range 
            {
                foodCollected++;
            }
        }

        /// <summary>
        /// Checking if another object is in range
        /// Used to determine if a food collectible is in range
        /// or a hiding spot is in range to collect or use
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public bool IsInRange(Rectangle otherObject)
        {
            //These numbers can be adjusted when visuals are implemented and do what looks good
            float dx = Math.Abs((this.pLocation.Width / 2) - (otherObject.Width / 2));
            float dy = Math.Abs((this.pLocation.Height / 2) - (otherObject.Height / 2));

            if (
                //TODO: Check distance between objects
                //distance is based on midpoint of each object??
                (dx + 20) >= (pLocation.X + pLocation.Width) - (otherObject.X + otherObject.Width)
                && (dy + 20) >= (pLocation.Y + pLocation.Width) - (otherObject.Y + otherObject.Height)
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
        /// Press and release space bar
        /// </summary>
        /// 
        //TODO: Implement a way to exit hide state, otherwise player will be stuck within object. -Julia
        //^^ bool to allow for exit
        public void Hide(KeyboardState prevState, 
            KeyboardState curState, Rectangle otherObstacle)
        {
            //get mid points lined up
            if (IsInRange(otherObstacle)
                // && otherObstacle is Hideable
                )
            {
                if(prevState.IsKeyDown(Keys.Space) && curState.IsKeyUp(Keys.Space))
                {
                    //These obstacles have to be the same size or larger than the player 
                    //Centers the player with the obstacle
                    pLocation.X = (otherObstacle.X + (otherObstacle.Width / 2)) 
                        - (pLocation.Width/2);

                    pLocation.Y = (otherObstacle.Y + (otherObstacle.Height / 2)) 
                        - (pLocation.Height / 2);

                }
            }
        }

        /// <summary>
        /// processes EVERYTHING that affects player movement
        /// </summary>
        //TODO: Implement edge collision--block the player from moving off
        //    screen (left/right edges), or change panel (top/bottom edges) -Julia
        //TODO: Diagonal movement doesn't work right now. Could potentially
        //   only use enum switch for draw purposes. -Julia
        private void ProcessInput()
        {
            //get current kbState
            currKB = Keyboard.GetState();

            //---------------------------------------------------------------
            switch (playerState)
            {
                //===================================================================
                case PlayerState.Left:
                    //if A is pressed
                    if (currKB.IsKeyDown(Keys.A))
                    {
                        pLocation.X -= 5;
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
                        pLocation.X += 5;
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
                        pLocation.Y -= 5;
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
                        pLocation.Y += 5;
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
            //IF COLLISION IS TRUE
            /*
            if (pLocation.Intersects(lightSource))
            {
                sb.Draw(
                pSprite,
                pLocation,
                Color.Red
                );
            }
            */

            //IF COLLSION IS FALSE
            sb.Draw(
                pSprite,
                pLocation,
                color
                );

        }
    }
}
