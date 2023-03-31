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
    /// Worked on by: McKenzie Lam
    /// </summary>
    internal class Enemy
    {
        private Texture2D texture;
        private Rectangle position;

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public Rectangle Position
        {
            get
            {
                return position;
            }
        }


        public Enemy(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }
    }
}
