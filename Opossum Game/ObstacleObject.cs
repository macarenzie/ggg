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
    /// Objects for the player to hide under, can't walk over normally.
    /// Range detection--can press key to snap under object if close enough.
    /// Collision method will mess with player position--instead of stopping player movement, 
    /// will just change position opposite to the current player movement direction. Keeps it separate from the player class.
    /// </summary>
    internal class ObstacleObject
    {
        //Fields
        private Texture2D obstacleTexture;
        private Rectangle obstacleDimensions;

        //Constructor
        ///<summary>
        ///Creates individual obstacle objects.
        ///</summary>
        ///<param name="obstacleTexture" the texture displayed for the created object
        ///<param name="obstacleDimensions" the dimensions of the object, used for drawing and checking collision
        public ObstacleObject (Texture2D obstacleTexture, Rectangle obstacleDimensions)
        {
            this.obstacleTexture = obstacleTexture;
            this.obstacleDimensions = obstacleDimensions;
        }

        //May not need any more methods. Depends on what's being put in player class.
    }
}
