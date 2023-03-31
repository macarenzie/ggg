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

        // game object textures
        private Texture2D collectibleTexture;
        private Texture2D obstacleTexture;
        private Texture2D playerTexture;
        private Texture2D enemyTexture;

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
        public Level(
            Texture2D collectibleTexture, 
            Texture2D obstacleTexture, 
            Texture2D playerTexture, 
            Texture2D enemyTexture)
        {
            collectiblesList = new List<Collectible>();
            obstaclesList = new List<Obstacle>();
            enemyList = new List<Enemy>();

            this.collectibleTexture = collectibleTexture;
            this.obstacleTexture = obstacleTexture;
            this.playerTexture = playerTexture;
            this.enemyTexture = enemyTexture;
        }

        // methods ------------------------------------------------------------
        public void LoadLevel(string levelFile)
        {
            // read in the file
            reader = new StreamReader("../../../" + levelFile + ".txt");

            // iterate through each line in the file
            while ((lineOfData = reader.ReadLine()) != null)
            {
                // split the data and sort it
                string[] objectData = lineOfData.Split(",");

                if (objectData[2] == "obstacle")
                {
                    Obstacle obstacle = new Obstacle(
                        obstacleTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) * 100 - 4,
                            obstacleTexture.Width / 5, 
                            obstacleTexture.Height / 5));

                    obstaclesList.Add(obstacle);
                }
                else if (objectData[2] == "collectible")
                {
                    Collectible collectible = new Collectible(
                        collectibleTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) * 100,
                            collectibleTexture.Width / 2, 
                            collectibleTexture.Height / 2));

                    collectiblesList.Add(collectible);
                }
                else if (objectData[2] == "enemy")
                {
                    Enemy enemy = new Enemy(
                        enemyTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) * 100,
                            enemyTexture.Width / 2, 
                            enemyTexture.Height / 2));

                    EnemyList.Add(enemy);
                }
                else if (objectData[2] == "player")
                {
                    player = new Player(
                        playerTexture, 
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) *100 ,
                            playerTexture.Width / 4, 
                            playerTexture.Height / 4));
                }
            }
        }
    }
}
