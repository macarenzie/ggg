using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Opossum_Game
{
    /// <summary>
    /// Worked on by: McKenzie Lam, Julia Rissberger, Hui Lin Khouw
    /// </summary>

    
    ///<summary>
    /// Enum used to indicate the current direction the enemy is moving/facing. 
    ///</summary>
    enum MovementDirection
    {
        Left,
        Right,
        Pause
    }

    internal class Enemy
    {
        //Fields
        private Texture2D texture;
        private Rectangle position;
        private MovementDirection currentDirection;
        private MovementDirection previousDirection;
        private Rectangle lightRectangle;
        private Stopwatch freezeTimer;

        //Properties
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public Rectangle Position
        {
            get
            {
                return position;
            }
        }
        
        /// <summary>
        /// Parameterized constructor for enemy objects
        /// </summary>
        /// <param name="texture">enemy texture</param>
        /// <param name="position">position and size of enemy</param>
        /// <param name="currentDirection">current direction the enemy is moving in</param>
        /// <param name="lightRectangle">dimensions of the enemy light</param>
        public Enemy(Texture2D texture, Rectangle position, Rectangle lightRectangle, MovementDirection currentDirection)
        {
            this.texture = texture;
            this.position = position;
            this.currentDirection = currentDirection;
            this.lightRectangle = lightRectangle;
            freezeTimer = new Stopwatch();  
        }

        public void Update(GameTime gameTime)
        {
            //Moves the enemy and light in the indicated direction until they hit the edge of the screen.
            switch (currentDirection)
            {
                //Moves to the left until the left edge is hit by the left edge
                case MovementDirection.Left:
                    position.X -= 3;
                    lightRectangle.X -= 3;
                    if (position.X < 0)
                    {
                        currentDirection = MovementDirection.Right;
                    }
                    break;

                //Moves to the right until the right edge is hit by the right edge
                case MovementDirection.Right:
                    position.X += 3;
                    lightRectangle.X += 3; 
                    if (position.X + position.Width > 900)
                    {
                        currentDirection = MovementDirection.Left;
                    }
                    break;

                //Occurs when an enemy collides with the player. Freezes for a short time and then flips direction.
                //TODO: adjust the freezeTimer value once we have a set time for player freeze
                case MovementDirection.Pause:

                    //Timer lasts for 5 seconds
                    freezeTimer.Start();

                    //Changes the movement direction to be opposite what the last movement direction was.
                    //Also resets stopwatch for next use.
                    if (freezeTimer.ElapsedMilliseconds > 5000)
                    {
                        freezeTimer.Stop();
                        freezeTimer.Reset();
                        
                        //Checks what last movement direction was
                        if (previousDirection == MovementDirection.Left) 
                        {
                            currentDirection = MovementDirection.Right;
                        }
                        else if (previousDirection == MovementDirection.Right)
                        {
                            currentDirection = MovementDirection.Left;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws enemy in the direction they are currently facing based on the direction enum.
        /// </summary>
        //TODO: Implement texture and texture flipping. Currently draws the same each direction.
        public void Draw(SpriteBatch sb)
        {
            switch(currentDirection)
            {
                case MovementDirection.Left:
                    sb.Draw(texture, position, Color.White);
                    break;

                case MovementDirection.Right:
                    sb.Draw(texture, position, Color.White);
                    break;
            }
                
        }

        /// <summary>
        /// Checks if an enemy is colliding with an obstacle.
        /// Changes their direction if so. Doesn't need to check collision side due to straight path of enemy.
        /// Needs to be called in game1 due to utilizing the obstacle list.
        /// </summary>
        /// <param name="obstacles"> List of obstacles currently on screen. </param>
        public void enemyObstacleCollision (List<Obstacle> obstacles)
        {
            //Indicates if collision is occurring. Set to false by default.
            bool isColliding = false;

            //Iterates through the entire list and checks intersect status of each obstacle. Stops early if collision is found.
            for (int i = 0; i < obstacles.Count || isColliding; i++)
            {
                if (position.Intersects(obstacles[i].Position))
                {
                    isColliding = true;
                }
            }

            //If collision is true, changes the current direction of the enemy. 
            if (isColliding)
            {
                if (currentDirection == MovementDirection.Left)
                {
                    currentDirection = MovementDirection.Right;
                }
                else if (currentDirection == MovementDirection.Right)
                {
                    currentDirection = MovementDirection.Left;
                }
            }
        }

        /// <summary>
        /// Checks if the player hits the rectangle that is the light.
        /// Also puts the enemy into the pause state
        /// </summary>
        /// <param name="player">Player or other object that would interact with the light</param>
        /// <returns>True if light intersects, otherwise false.</returns>
        public bool LightIntersects(Rectangle player)
        {
            if (lightRectangle.Intersects(player)) 
            {
                //Records last direction the enemy was moving and sets to pause state
                previousDirection = currentDirection;
                currentDirection = MovementDirection.Pause;
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
