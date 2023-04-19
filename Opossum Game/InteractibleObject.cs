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
    /// Abstract class for objects the player is able to interact with in some form. 
    /// Utilized as the parent class for obstacles and collectibles.
    /// Worked on by: Julia Rissberger, McKenzie Lam
    /// </summary>
    internal abstract class InteractibleObject : IGameObject
    {
        //Fields
        protected Texture2D objectTexture;
        protected Rectangle objectDimensions;

        /// <summary>
        /// Get/set for obstacle texture
        /// </summary>
        public Texture2D Sprite
        {
            get
            {
                return objectTexture;
            }
            set
            {
                objectTexture = value;
            }
        }

        /// <summary>
        /// Get/set for obstacle position
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return objectDimensions;
            }
            set
            {
                objectDimensions = value;
            }
        }

        ///<summary>
        ///Creates individual obstacle objects.
        ///</summary>
        ///<param name="objectTexture">the texture displayed for the created object</param>
        ///<param name="objectDimensions">the dimensions of the object, used for drawing and checking collision</param>
        public InteractibleObject (Texture2D objectTexture, Rectangle objectDimensions)
        {
            this.objectTexture = objectTexture;
            this.objectDimensions = objectDimensions;
        }

        /// <summary>
        /// Abstract method for drawing objects
        /// </summary>
        /// <param name="sb">spritebatch</param>
        /// <param name="color">color of object</param>
        public abstract void Draw(SpriteBatch sb, Color color);
        
    }
}
