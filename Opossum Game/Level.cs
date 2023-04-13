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
        private string lineOfData;
        private StreamReader reader;

        // game object lists
        private List<Collectible> collectiblesList;
        private List<Obstacle> obstaclesList;
        private List<Enemy> enemyList;
        private Player player;

        // game object textures
        private Texture2D collectibleTexture;
        private List<Texture2D> collectibleTextures;
        private Texture2D obstacleTexture;
        private Texture2D playerTexture;
        private Texture2D enemyTexture;

        /// <summary>
        /// 
        /// </summary>
        public List<Collectible> CollectiblesList
        {
            get
            {
                return collectiblesList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Obstacle> ObstacleList
        {
            get
            {
                return obstaclesList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Enemy> EnemyList
        {
            get
            {
                return enemyList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Player Player
        {
            get
            {
                return player;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectibleTexture"></param>
        /// <param name="obstacleTexture"></param>
        /// <param name="playerTexture"></param>
        /// <param name="enemyTexture"></param>
        public Level(
            List<Texture2D> collectibleTextures, 
            Texture2D obstacleTexture, 
            Texture2D playerTexture, 
            Texture2D enemyTexture)
        {
            // initialize each game object list
            collectiblesList = new List<Collectible>();
            obstaclesList = new List<Obstacle>();
            enemyList = new List<Enemy>();

            // initialize each game object texture
            this.collectibleTextures = collectibleTextures;
            this.obstacleTexture = obstacleTexture;
            this.playerTexture = playerTexture;
            this.enemyTexture = enemyTexture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelFile"></param>
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
                            obstacleTexture.Width / 2, 
                            obstacleTexture.Height / 2));

                    obstaclesList.Add(obstacle);
                }
                else if (objectData[2] == "collectible")
                {
                    Random rng = new Random();
                    collectibleTexture = collectibleTextures[rng.Next(0, 3)];
                    Collectible collectible = new Collectible(
                        collectibleTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) * 100,
                            collectibleTexture.Width / 4, 
                            collectibleTexture.Height / 4));

                    collectiblesList.Add(collectible);
                }
                else if (objectData[2] == "enemy")
                {
                   /* Enemy enemy = new Enemy(
                        enemyTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 100,
                            int.Parse(objectData[0]) * 100,
                            enemyTexture.Width / 20, 
                            enemyTexture.Height / 10));

                    EnemyList.Add(enemy); */
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
