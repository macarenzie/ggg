﻿using System;
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
        //Fields
        Texture2D texture;
        Rectangle position;
        bool isHideable;

        /// <summary>
        /// Get/set for obstacle texture
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
        /// Get/set for obstacle position
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
        /// Get-only property for isHideable status
        /// </summary>
        public bool IsHideable
        {
            get
            {
                return isHideable;
            }
        }

        /// <summary>
        /// Constructor for obstacle objects
        /// </summary>
        /// <param name="objectTexture">Texture of obstacle</param>
        /// <param name="objectDimensions">Size and position of obstacle</param>
        /// <param name="isHideable">If obstacle can be hidden in by the player</param>
        public Obstacle(Texture2D objectTexture, Rectangle objectDimensions /*bool isHideable*/) 
            : base (objectTexture, objectDimensions) 
        {
            texture = objectTexture;
            position = objectDimensions;
            //this.isHideable = isHideable;
        }
        
        /// <summary>
        /// Override for draw method to draw the obstacle to the screen
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        /// <param name="color">Color the obstacle should be tinted</param>
        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
    }
}
