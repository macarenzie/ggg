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
    /// Enum used to indicate the current status of the enemy (movement). 
    ///</summary>
    enum Status
    {
        Left,
        Right,
        Pause
    }

    internal class Enemy : IGameObject
    {
        //Fields
        private Texture2D texture;
        private Rectangle position;
        private Status currentDirection;
        private Status previousDirection;
        private Stopwatch freezeTimer;

        /// <summary>
        /// Get-only property for enemy texture
        /// </summary>
        public Texture2D Sprite
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        /// <summary>
        /// Get-only property for enemy position
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// get and set of enemy's Y coordinate
        /// </summary>
        public int Y
        {
            get
            {
                return position.Y; 
            }
            set
            {
                position.Y = value;
            }
        }

        /// <summary>
        /// get and set of enemy's X coordinate
        /// </summary>
        public int X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }
        
        /// <summary>
        /// Parameterized constructor for enemy objects
        /// </summary>
        /// <param name="texture">enemy texture</param>
        /// <param name="position">position and size of enemy</param>
        /// <param name="currentDirection">current direction the enemy is moving in</param>
        /// <param name="lightRectangle">dimensions of the enemy light</param>
        public Enemy(Texture2D texture, Rectangle position, Status currentDirection)
        {
            this.texture = texture;
            this.position = position;
            this.currentDirection = currentDirection;
            freezeTimer = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            //Moves the enemy and light in the indicated direction until they hit the edge of the screen.
            switch (currentDirection)
            {
                //Moves to the left until the left edge is hit by the left edge
                case Status.Right:
                    position.X -= 3;
                    
                    if (position.X < 0)
                    {
                        currentDirection = Status.Left;
                    }
                    break;

                //Moves to the right until the right edge is hit by the right edge
                case Status.Left:
                    position.X += 3;
                    
                    if (position.X + position.Width > 900)
                    {
                        currentDirection = Status.Right;
                    }
                    break;

                //Occurs when an enemy collides with the player. Freezes for a short time and then flips direction.
                //TODO: adjust the freezeTimer value once we have a set time for player freeze
                case Status.Pause:

                   //Timer lasts for 3 seconds
                   freezeTimer.Start();

                   //Changes the movement direction to be opposite what the last movement direction was.
                   //Also resets stopwatch for next use.
                   if (freezeTimer.Elapsed.TotalSeconds > 3)
                   {
                      freezeTimer.Stop();

                      //Checks what last movement direction was
                      if (previousDirection == Status.Left)
                      {
                          currentDirection = Status.Right;
                      }
                      else if (previousDirection == Status.Right)
                      {
                          currentDirection = Status.Left;
                      }
                      freezeTimer.Reset();
                   }
                   break; 
            }
            
        }

        /// <summary>
        /// Draws enemy in the direction they are currently facing based on the direction enum.
        /// </summary>
        //TODO: Implement texture and texture flipping. Currently draws the same each direction.
        public void Draw(SpriteBatch sb, Color color)
        {
            switch(currentDirection)
            {
                case Status.Left:
                    sb.Draw(
                        texture, 
                        position, 
                        color);
                    break;

                case Status.Right:
                    sb.Draw(
                        texture, 
                        position, 
                        null, 
                        color, 
                        0.0f, 
                        new Vector2(), 
                        SpriteEffects.FlipHorizontally, 
                        1.0f);
                    break;

                //Draws enemy in the direction of movement before pause
                case Status.Pause:

                    //Checks for recorded previous direction
                    if (previousDirection == Status.Left)
                    {
                        sb.Draw(
                            texture, 
                            position, 
                            color);
                    }
                    else if (previousDirection == Status.Right)
                    {
                        sb.Draw(
                        texture,
                        position,
                        null,
                        color,
                        0.0f,
                        new Vector2(),
                        SpriteEffects.FlipHorizontally,
                        1.0f);
                    }
                    break;
            }
                
        }

        /// <summary>
        /// Checks if an enemy is colliding with an obstacle.
        /// Changes their direction if so. Doesn't need to check collision side due to straight path of enemy.
        /// Needs to be called in game1 due to utilizing the obstacle list.
        /// </summary>
        /// <param name="obstacles"> List of obstacles currently on screen. </param>
        public void EnemyObstacleCollision (List<Obstacle> obstacles)
        {
            bool isColliding = false;

            //Going through each obstacle in the list of obstacles to check for intersection
            foreach (Obstacle obstacle in obstacles)
            {
                //If the intersection returns true
                if (position.Intersects(obstacle.Rect))
                {
                    isColliding = true; //adjust return bool

                    //Get the intersection area
                    Rectangle intersectionArea = Rectangle.Intersect(
                        position, obstacle.Rect);

                    //If the width is less than the height, adjust the X position
                    if (intersectionArea.Width < intersectionArea.Height)
                    {
                        //LEFT side of obstacle
                        //player.X is less than the midpoint of the obstacle's width
                        if (position.X < (obstacle.Rect.X + obstacle.Rect.Width / 2))
                        {
                            position.X -= intersectionArea.Width;

                        }
                        //RIGHT
                        else
                        {
                            position.X += intersectionArea.Width;

                        }

                    }

                    //If the height it less than the width, adjust the Y position
                    else if (intersectionArea.Height < intersectionArea.Width)
                    {
                        //TOP side of the obstacle;
                        //If player.Y is less than the obstacle's Height midpoint
                        if (position.Y < (obstacle.Rect.Y + obstacle.Rect.Height / 2))
                        {
                            position.Y -= intersectionArea.Height;

                        }
                        //BOTTOM
                        else
                        {
                            position.Y += intersectionArea.Height;

                        }
                    }

                }
            }



            //If collision is true, changes the current direction of the enemy. 
            if (isColliding)
            {
                if (currentDirection == Status.Left)
                {
                    currentDirection = Status.Right;
                }
                else if (currentDirection == Status.Right)
                {
                    currentDirection = Status.Left;
                }
            }

            
        }

        /// <summary>
        /// Checks if the player hits the rectangle that is the light.
        /// Also puts the enemy into the pause state
        /// </summary>
        /// <param name="player">Player or other object that would interact with the light</param>
        /// <returns>True if light intersects, otherwise false.</returns>
        public void LightIntersects(Rectangle player)
        {
            if (Rect.Intersects(player)) 
            {
                //Records last direction the enemy was moving (if not paused)
                if (currentDirection != Status.Pause)
                {
                    previousDirection = currentDirection;
                }

                //Sets current direction to pause
                currentDirection = Status.Pause;
            } 
        }
    }
}
