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
    /// Class for objects that the player can interact with. 
    /// Includes hiding spots and collectibles.
    /// </summary>
    internal class InteractibleObject
    {
        //Fields
        private Texture2D obstacleTexture;
        private Rectangle obstacleDimensions;
        private bool isCollectible;

        //Constructor
        ///<summary>
        ///Creates individual obstacle objects.
        ///</summary>
        ///<param name="obstacleTexture" the texture displayed for the created object
        ///<param name="obstacleDimensions" the dimensions of the object, used for drawing and checking collision
        ///<param name="isCollectible" shows if an object can be collected by the player</param>
        public InteractibleObject (Texture2D obstacleTexture, Rectangle obstacleDimensions, bool isCollectible)
        {
            this.obstacleTexture = obstacleTexture;
            this.obstacleDimensions = obstacleDimensions;
            this.isCollectible = isCollectible;
        }

        //May not need any more methods. Depends on what's being put in player class.
    }
}
