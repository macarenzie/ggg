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
        private string fileName;
        private string lineOfData;
        private StreamReader reader;

        // game object lists
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;
        private Player player;

        private Texture2D filler;

        // properties ---------------------------------------------------------
        public List<Collectible> CollectiblesList
        {
            get
            {
                return collectiblesList;
            }
        }

        public List<Obstacle> ObstacleList
        {
            get
            {
                return obstaclesList;
            }
        }

        public List<Enemy> EnemyList
        {
            get
            {
                return enemyList;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }
        }

        // constructor --------------------------------------------------------
        public Level()
        {
            collectiblesList = new List<Collectible>();
            obstaclesList = new List<Obstacle>();
            enemyList = new List<Enemy>();
        }

        // methods ------------------------------------------------------------
        public void LoadLevel(string levelFile)
        {
            // read in the file
            reader = new StreamReader("../../../" + levelFile);

            // iterate through each line in the file
            while ((lineOfData = reader.ReadLine()) != null)
            {
                // split the data and sort it
                string[] objectData = lineOfData.Split(",");

                if (objectData[2] == "obstacle")
                {
                    Obstacle obstacle = new Obstacle(
                        filler,
                        new Rectangle(
                            int.Parse(objectData[0]),
                            int.Parse(objectData[1]),
                            0, 0));

                    obstaclesList.Add(obstacle);
                }
                else if (objectData[2] == "collectible")
                {
                    Collectible collectible = new Collectible(
                        filler,
                        new Rectangle(
                            int.Parse(objectData[0]),
                            int.Parse(objectData[1]),
                            0, 0));

                    collectiblesList.Add(collectible);
                }
                else if (objectData[2] == "enemy")
                {
                    Enemy enemy = new Enemy(
                        filler,
                        new Rectangle(
                            int.Parse(objectData[0]),
                            int.Parse(objectData[1]),
                            0, 0));

                    EnemyList.Add(enemy);
                }
                else if (objectData[2] == "player")
                {
                    player = new Player(
                        filler, 
                        new Rectangle(
                            int.Parse(objectData[0]),
                            int.Parse(objectData[1]),
                            0, 0));
                }
            }
        }
    }
}
