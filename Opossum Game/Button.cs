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
    /// Representing a button
    /// Has roll-over built in 
    /// </summary>
    internal class Button
    {
        private Texture2D button;
        private Texture2D rolloverButton;
        private Rectangle rectangle;

        /// <summary>
        /// instantiating the button
        /// </summary>
        /// <param name="image"> button image </param>
        /// <param name="rectangle"> rectangle of button </param>
        /// <param name="graphics"> size of the screen </param>
        public Button(Texture2D image, Rectangle rectangle, Texture2D rollOverVersion)
        {
            this.rolloverButton = rollOverVersion;
            this.button = image;
            this.rectangle = rectangle;

        }

        /// <summary>
        /// draws the button to the screen
        /// Changes texture if mouse is over the button
        /// </summary>
        /// <param name="sb"> sprite batch parameter </param>
        public void Draw(SpriteBatch sb)
        {
            //Rollover texture
            if (MouseContains())
            {
                sb.Draw(rolloverButton, rectangle, Color.White);
            }

            //Regular texture
            else
            {
                sb.Draw(button, rectangle, Color.White);
            }
        }

        /// <summary>
        /// checks if the position of the mouse is within button bounds
        /// </summary>
        /// <returns> bool based on conditions </returns>
        public bool MouseContains()
        {
            //might change this to contains for easier reading - ariel
            MouseState mouse = Mouse.GetState();
            if (mouse.X > rectangle.X && mouse.X < rectangle.X + rectangle.Width &&
                mouse.Y > rectangle.Y && mouse.Y < rectangle.Y + rectangle.Height)
            {
                //return true
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks if the mouse button is clicked
        /// </summary>
        /// <returns> bool based on coditions </returns>
        public bool MouseClick()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }
    }
}
