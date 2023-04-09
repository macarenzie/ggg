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
    /// Class for obstacle objects that have collision interactions with the player.
    /// The player is able to hide under these objects when in range.
    /// Inherits from InteractibleObject.
    /// Worked on by: McKenzie Lam, Julia Rissberger
    /// </summary>
    internal class Obstacle : InteractibleObject
    {
        Texture2D texture;
        Rectangle position;
        bool isHidable;

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
        /// makes the bool for an object acessible
        /// </summary>
        public bool IsHidable
        {
            get
            {
                return isHidable;
            }
        }

        //Parameterized constructor
        //Only utilizes fields from the parent class at present
        public Obstacle(Texture2D objectTexture, Rectangle objectDimensions /*,bool isHidable*/) 
            : base (objectTexture, objectDimensions) 
        {
            texture = objectTexture;
            position = objectDimensions;

            //put this here because i would assume its also being read in --ariel
            //this.isHidable = isHidable;
        }
        
        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
    }
}
