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
    /// </summary>
    internal abstract class InteractibleObject
    {
        //Fields
        protected Texture2D objectTexture;
        protected Rectangle objectDimensions;

        //Properties
        public Rectangle ObjectDimensions
        {
            get { return objectDimensions; }
            set { objectDimensions = value; }
        }

        public Texture2D ObjectTexture
        {
            get { return objectTexture; }
            set { objectTexture = value; }
        }

        //Constructor
        ///<summary>
        ///Creates individual obstacle objects.
        ///</summary>
        ///<param name="objectTexture" the texture displayed for the created object
        ///<param name="objectDimensions" the dimensions of the object, used for drawing and checking collision
        public InteractibleObject (Texture2D objectTexture, Rectangle objectDimensions)
        {
            this.objectTexture = objectTexture;
            this.objectDimensions = objectDimensions;
        } 

        //Draw method
        public virtual void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(objectTexture, ObjectDimensions, color);
        }
    }
}
