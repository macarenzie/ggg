using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Opossum_Game
{
    /// <summary>
    /// For an easier time drawing game objects and dealing with collisions
    /// and debug mode
    /// Worked on by Ariel Cthwe
    /// </summary>
    internal interface IGameObject
    {
        //necessary accessors for every class
        Rectangle Rect { get; }
        Texture2D Sprite { get; }

        void Draw(SpriteBatch sb, Color color);

    }
}
