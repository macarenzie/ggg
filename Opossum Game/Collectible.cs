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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        /// <param name="objectTexture"></param>
        /// <param name="objectDimensions"></param>
        public Collectible (Texture2D objectTexture, Rectangle objectDimensions) :
            base (objectTexture, objectDimensions) 
        {
            texture = objectTexture;
            position = objectDimensions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="color"></param>
        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
    }
}
