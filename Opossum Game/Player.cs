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
    /// Interfaces: ICollide
    /// Majority Written by: Jamie Zheng
    /// </summary>
    internal class Player
    {
        //fields
        private Rectangle pLocation; //dimensions pSprite dimensions
        private Texture2D pSprite;
        private int health;
        private int foodCollected;

        //properties
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

        //methods
        /// <summary>
        /// Will call in the Game1 Update block
        /// WASD controls, Player does not move if edges are touching
        /// </summary>
        /// <param name="prevState"></param>
        /// <param name="curState"></param>
        public void Update(KeyState prevState, KeyState curState)
        {
            //if collision true, do not change position
            if (true)           //TEMP; change to Collision();
            {
                pLocation.X += 0;
                pLocation.Y += 0;
            }

            //else
            else
            {
                //W

                //A

                //S

                //D
            }
        }

        /// <summary>
        /// Draw the player to the screen, highlight if collision with light is true
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //TODO: IF COLLISION IS TRUE
            sb.Draw(
                pSprite,
                pLocation,
                Color.Red
                );

            //TODO: IF COLLSION IS FALSE
            sb.Draw(
                pSprite,
                pLocation,
                Color.White
                );

            sb.End();
        }

        /// <summary>
        /// Checking if the edge's of the player and another object are touching
        /// Exists to keep FSM if statements clean
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns>If the player's edge is in contact with another obstacle's edge</returns>
        public bool Collision(Rectangle otherObject)
        {
            //TODO: CHECKING FOR COLLISION OF EACH EDGE, NOT A FULL ON OVERLAP. 
            if (pLocation.Intersects(otherObject)           //CURRENT CONDITION A PLACEHOLDER
                //Left edge
                //Right edge
                //Top edge
                //Bottom edge
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
        /// Press space to collect food if other object is in range
        /// </summary>
        /// <param name="key"></param>
        public void Collect(Keys space, KeyState prevState, KeyState curState)
        {
            //TODO: Check for press and release of space bar
            //Only collect if collectible is in range, check if collectible is collectible
            //Complete IsInRange() method before this one
            foodCollected++;
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
            float dx = Math.Abs((this.pLocation.Width / 2) - (otherObject.Width / 2));
            float dy = Math.Abs((this.pLocation.Height / 2) - (otherObject.Height / 2));
            if (
                //TODO: Check distance between objects
                //distance is based on midpoint of each object
                (dx + 20) >= (this.pLocation.X + this.pLocation.Width) - (otherObject.X + otherObject.Width) //incomplete
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
        public void Hide(Keys space, KeyState prevState, 
            KeyState curState, Rectangle otherObstacle)
        {

        }
    }
}
