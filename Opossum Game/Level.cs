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
        //private Texture2D playerTextureSide;
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
        /// <param name="collectibleTextures"></param>
        /// <param name="obstacleTexture"></param>
        /// <param name="playerTexture"></param>
        /// <param name="enemyTexture"></param>
        public Level(
            List<Texture2D> collectibleTextures, 
            Texture2D obstacleTexture, 
            Texture2D playerTexture, 
            Texture2D enemyTexture)
            //Texture2D playerTextureSide)
        {
            // initialize each game object list
            collectiblesList = new List<Collectible>();
            obstaclesList = new List<Obstacle>();
            enemyList = new List<Enemy>();

            // initialize each game object texture
            this.collectibleTextures = collectibleTextures;
            this.obstacleTexture = obstacleTexture;
            this.playerTexture = playerTexture;
            //this.playerTextureSide = playerTextureSide;
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
                    Obstacle obstacle = null;

                    // determine if it is a hideable object
                    if (objectData[3] == "t")
                    {
                        obstacle = new Obstacle(
                            obstacleTexture, 
                            new Rectangle(
                                int.Parse(objectData[1]) * 90, 
                                int.Parse(objectData[0]) * 90,
                                obstacleTexture.Width / 3, 
                                obstacleTexture.Height / 3), 
                            true);
                    }
                    else
                    {
                        obstacle = new Obstacle(
                            obstacleTexture,
                            new Rectangle(
                                int.Parse(objectData[1]) * 90,
                                int.Parse(objectData[0]) * 90,
                                obstacleTexture.Width / 3,
                                obstacleTexture.Height / 3),
                            false);
                    }
                    
                    obstaclesList.Add(obstacle);
                }
                else if (objectData[2] == "collectible")
                {
                    // randomize the collectible texture
                    Random rng = new Random();
                    collectibleTexture = collectibleTextures[rng.Next(0, 3)];
                    
                    // create and add the collectible object to the list
                    Collectible collectible = new Collectible(
                        collectibleTexture,
                        new Rectangle(
                            int.Parse(objectData[1]) * 90 + 10,
                            int.Parse(objectData[0]) * 90 + 10,
                            collectibleTexture.Width / 3, 
                            collectibleTexture.Height / 3));

                    collectiblesList.Add(collectible);
                }
                else if (objectData[2] == "enemy")
                {
                    Enemy enemy = null;
                    
                    // determine the movement direction of the enemy
                    if (objectData[3] == "l")
                    {
                        enemy = new Enemy(
                        enemyTexture,       // texture
                        new Rectangle(      // position and dimensions
                            int.Parse(objectData[1]) * 90,
                            int.Parse(objectData[0]) * 90,
                            enemyTexture.Width / 20,
                            enemyTexture.Height / 10), 
                        new Rectangle(),    // light
                        MovementDirection.Left);    // direction
                    }
                    else
                    {
                        enemy = new Enemy(
                        enemyTexture,       // texture
                        new Rectangle(      // position and dimensions
                            int.Parse(objectData[1]) * 90,
                            int.Parse(objectData[0]) * 90,
                            enemyTexture.Width / 20,
                            enemyTexture.Height / 10),
                        new Rectangle(),    // light
                        MovementDirection.Left);    // direction
                    }
                    
                    EnemyList.Add(enemy);
                }
                else if (objectData[2] == "player")
                {
                    player = new Player(
                        playerTexture, 
                        new Rectangle(
                            int.Parse(objectData[1]) * 90,
                            int.Parse(objectData[0]) * 90,
                            playerTexture.Width / 5, 
                            playerTexture.Height / 5),
                        playerTextureSide,
                        new Rectangle(
                            int.Parse(objectData[1]) * 90,
                            int.Parse(objectData[0]) * 90,
                            playerTextureSide.Width / 5,
                            playerTextureSide.Height / 5));
                }
            }
        }
    }
}
