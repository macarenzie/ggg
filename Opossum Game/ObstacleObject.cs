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
    }
}
