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
    /// Inherits from obstacle, uses collision interface(?)
    /// Food objects for the player. Collected via button press when in range. 
    /// Range can be determined through collision if we expand the image dimensions beyond the actual image. 
    /// Use collision to highlight(?) if player is in range, sets bool to true for flag to check if object can be collected.
    /// Actual collection command is in player class.
    /// </summary>
    internal class Collectible : ObstacleObject
    {
    }
}
