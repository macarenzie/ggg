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
    /// </summary>
    internal class Obstacle : InteractibleObject
    {
        //Parameterized constructor
        //Only utilizes fields from the parent class at present
        public Obstacle(Texture2D objectTexture, Rectangle objectDimensions) 
            : base (objectTexture, objectDimensions) { }
        
    }
}
