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
    /// Worked on by: McKenzie Lam, Julia Rissberger
    /// </summary>

    
    ///<summary>
    /// Enum used to indicate the current direction the enemy is moving/facing.
    ///</summary>
    enum MovementDirection
    {
        Left,
        Right
    }

    internal class Enemy
    {
        //Fields
        private Texture2D texture;
        private Rectangle position;
        private MovementDirection currentDirection;

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


        public Enemy(Texture2D texture, Rectangle position, MovementDirection currentDirection)
        {
            this.texture = texture;
            this.position = position;
            this.currentDirection = currentDirection;
        }

        public void Update(GameTime gameTime)
        {
            //Moves the enemy in the indicated direction until they hit the edge of the screen.
            switch (currentDirection)
            {
                //Moves to the left until the left edge is hit by the left edge
                case MovementDirection.Left:
                    position.X = position.X - 3;
                    if (position.X < 0)
                    {
                        currentDirection = MovementDirection.Right;
                    }
                    break;

                //Moves to the right until the right edge is hit by the right edge
                case MovementDirection.Right:
                    position.X = position.X + 3;
                    if (position.X + position.Width > 900)
                    {
                        currentDirection = MovementDirection.Left;
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
    }
}
