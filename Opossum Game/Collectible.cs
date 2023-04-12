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
    /// Class for collectible objects that can be picked up by the player when in range.
    /// Can be walked over by the player.
    /// Inherits from InteractibleObject.
    /// Worked on by: McKenzie Lam, Julia Rissberger
    /// </summary>
    internal class Collectible : InteractibleObject
    {
        Texture2D texture;
        Rectangle position;

        /// <summary>
        /// Get/set for collectible texture
        /// </summary>
        public Texture2D Texture
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
        /// Get/set for collectible position
        /// </summary>
        public Rectangle Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Constructor for individual collectible objects
        /// </summary>
        /// <param name="objectTexture">texture of collectible</param>
        /// <param name="objectDimensions">position of collectible</param>
        public Collectible (Texture2D objectTexture, Rectangle objectDimensions) :
            base (objectTexture, objectDimensions) 
        {
            texture = objectTexture;
            position = objectDimensions;
        }

        /// <summary>
        /// Override for draw, draws collectible with specified color
        /// </summary>
        /// <param name="sb">spritebatch</param>
        /// <param name="color">current color of the collectible</param>
        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
    }
}
