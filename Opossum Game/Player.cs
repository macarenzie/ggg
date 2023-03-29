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

        //Player location on X axis
        public int X
        {
            get { return pLocation.X; }
            set { pLocation.X = value; }
        }

        //Player location on Y axis
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

        /// <summary>
        /// Checking if the edge's of the player and another object are touching
        /// Exists to keep FSM if statements clean
        /// </summary>
        /// <param name="otherObject">The enemy or obstacle's Rectangle field (property)</param>
        /// <returns>If the player's edge is in contact with another obstacle's edge</returns>
        //TODO: We may not need this method. Commented out for now in case we need to pull any code from it. -Julia
        /*public bool EdgeCollision(Rectangle otherObject)
        {
            //TODO: 3.18.2023: Maybe have overlap, but in the draw logic keep the edges clipped
            //This logic is not implemented yet -Jamie
            if (
                //LEFT OR RIGHT EDGE
                ((((pLocation.X + pLocation.Width) == otherObject.X) ||      //left
                (pLocation.X == (otherObject.X + otherObject.Width))) &&     //right
                (pLocation.Y <= (otherObject.Y + otherObject.Height)) &&     //between length edges
                (pLocation.Y >= otherObject.Y)) ||

                //TOP OR BOTTOM EDGE
                ((((pLocation.Y + pLocation.Height) == otherObject.Y) ||     //Top
                (pLocation.Y == (otherObject.Y + otherObject.Height))) &&    //Bottom
                (pLocation.X <= (otherObject.X + otherObject.Width)) &&      //Between width edges
                (pLocation.X >= otherObject.X))
                )
            {
                return true;
            }

            else
            {
                return false;
            }
        } */

        ///<summary>
        ///Checks edge collision with obstacles.
        ///Edits a string to represent which edge is in contact. 
        ///The string is used in game1 to adjust player movement.
        ///Assumes that the list provided is a list of interactibleObjects. This can be adjusted later if necessary.
        ///</summary>
        
        //public string ObstacleCollisionList(List<Obstacle> objects)
        //{
            //Default representation for no collision
            /*string collisionDirection = "none";

            /*TODO: I pulled these height/width specifications from game1 on 3/27/23. 
             * Please let me know if this changes, or collision won't work properly. -Julia*/
            //int playerWidth = pSprite.Width / 4;
            //int playerHeight = pSprite.Height / 4;

            //Loops to check each object within the list
           /*for (int i = 0; i < objects.Count; i++)
            {
                //TODO: placeholder height/width specifications, based on player specifications--see above note. -Julia
                int objectWidth = 200;
                int objectHeight = 200;

                //Only checks collision if the object is an obstacle. Skips any collectibles.

                
                    //Checks each individual edge
                    //Player blocked in down direction
                    //Player width factored into X direction to prevent clipping. 
                    if(pLocation.Y + playerHeight == objects[i].ObjectDimensions.Y && 
                        pLocation.X >= objects[i].ObjectDimensions.X + playerWidth &&
                        pLocation.X <= objects[i].ObjectDimensions.X + objectWidth)
                    {
                        collisionDirection = "down";
                    }

                    //Player blocked in up direction
                    else if (pLocation.Y == objects[i].ObjectDimensions.Y + objectHeight &&
                         pLocation.X >= objects[i].ObjectDimensions.X + playerWidth &&
                         pLocation.X <= objects[i].ObjectDimensions.X + objectWidth)
                    {
                        collisionDirection = "up";
                    }

                    //Player blocked in left direction
                    else if (pLocation.X == objects[i].ObjectDimensions.X + objectWidth &&
                        pLocation.Y >= objects[i].ObjectDimensions.Y + playerHeight &&
                        pLocation.Y <= objects[i].ObjectDimensions.Y + objectHeight)
                    {
                        collisionDirection = "left";
                    }

                    //Player blocked in right direction
                    else if (pLocation.X + playerWidth == objectWidth &&
                        pLocation.Y >= objects[i].ObjectDimensions.Y + playerHeight &&
                        pLocation.Y <= objects[i].ObjectDimensions.Y + objectHeight)
                    {
                        collisionDirection = "right";
                    }
                
            }
            //Later objects in list shouldn't override an earlier detected collision.
            //May add individual return commands to if blocks depending on how many obstacles, for efficiency's sake.
            return collisionDirection;
        } */

        //Method for detecting individual collision with obstacles
        public bool IndividualCollision (Obstacle obstacle)
        {
            if (pLocation.Intersects(obstacle.ObjectDimensions)) //And !isHidden
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Press space to collect food if other object is in range
        /// </summary>
        /// <param name="key"></param>
        public void Collect(KeyboardState prevState, KeyboardState curState, InteractibleObject otherObject)
        {
            //TODO: Check for press and release of space bar
            //Only collect if collectible is in range, check if collectible is collectible
            //Complete IsInRange() method before this one
            if(prevState.IsKeyDown(Keys.Space) &&               //key release check
               curState.IsKeyUp(Keys.Space) &&
                IsInRange(otherObject.ObjectDimensions) &&      //Check if in range
                otherObject is Collectible                      //Check object is collectible
                )
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
        /// Will change the player's position to be overlapping with the hidable obstacle
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
        //TODO: Implement edge collision--block the player from moving off screen (left/right edges), or change panel (top/bottom edges) -Julia
        //TODO: Diagonal movement doesn't work right now. Could potentially only use enum switch for draw purposes. -Julia
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

                        //no moving in negative x direction if A is released
                        if(currKB.IsKeyUp(Keys.A) && prevKB.IsKeyDown(Keys.A))
                        {
                            pLocation.X += 0;
                        }
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

                        //No moving in positive x direction if D is released
                        if(currKB.IsKeyUp(Keys.D) && prevKB.IsKeyDown(Keys.D))
                        {
                            pLocation.X += 0;
                        }
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

                        //No moving in the negative y direction if W is released
                        if(currKB.IsKeyUp(Keys.W) && prevKB.IsKeyDown(Keys.W))
                        {
                            pLocation.Y += 0;
                        }
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

                        //No moving in the positive Y direction if S is released
                        if(currKB.IsKeyUp(Keys.S) && prevKB.IsKeyDown(Keys.S))
                        {
                            pLocation.Y += 0;
                        }
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
        public void Draw(SpriteBatch sb) 
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
                Color.White
                );

        }
    }
}
