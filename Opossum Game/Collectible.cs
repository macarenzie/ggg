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
    /// </summary>
    internal class Collectible : InteractibleObject
    {
        Texture2D texture;
        Rectangle position;

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

        //Parameterized constructor
        //Only utilizes fields from parent class at present
        public Collectible (Texture2D objectTexture, Rectangle objectDimensions) :
            base (objectTexture, objectDimensions) { }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }
    }
}
