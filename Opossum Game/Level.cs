using System;
using System.IO;
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
    /// generates level objects based on a passed in data file
    /// Worked on by: McKenzie Lam
    /// </summary>
    internal class Level
    {
        // fields -------------------------------------------------------------
        string lineOfData;
        StreamReader reader;

        // collectible list
        // obstacle list
        // enemy list
        // player object

        // properties ---------------------------------------------------------

        // ********************************************************************
        /* ALL LISTS NEED (GET ONLY) PROPERTIES TO BE ACCESSED IN GAME1
         * IN GAME1: 
         *      - create instance of level object which takes in file
         *      - then utilize properties to draw each respective list
         */
        // ********************************************************************

        // collectible list (get only)

        // obstacle list (get only)

        // enemy list (get only)

        // player object (get only)

        // constructor --------------------------------------------------------
        public Level()
        {
            lineOfData = "";
        }

        // methods ------------------------------------------------------------
        public void LoadLevel(string levelFile)
        {
            // read in the file
            reader = new StreamReader("Content/Levels/" + levelFile);

            while ((lineOfData = reader.ReadLine()) != null)
            {
                // WHILE LOOP
                /*      split the data
                 *      determine what kind of data it is
                 *      
                 *      if ( interactable object )
                 *        if ( collectible )
                 *            create collectible object
                 *            add to collectible list
                 *         if ( obstacle )
                 *           create obstace object
                 *              add to obstacle list
                 *          
                 *      if ( enemy )
                 *          create enemy object
                 *          add to enemy list
                 *          
                 *      if ( player )
                 *          create player object
                 */
            }
        }
    }
}
