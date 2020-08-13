﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Dictionary<string, Texture2D> assets = new Dictionary<string, Texture2D>();
        public static Player player = new Player();
        public static List<GameObject> playerBulletList = new List<GameObject>();
        public static List<GameObject> enemyList = new List<GameObject>();
        public static List<GameObject> enemyBulletList = new List<GameObject>();
        public static List<Missile> missileList = new List<Missile>();
        public static GameWindow window;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            window = this.Window;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load content here
            assets.Add("player", Content.Load<Texture2D>("Player"));
            assets.Add("playerBullet", Content.Load<Texture2D>("bullet"));

            player.Initialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //update player
            player.Update(gameTime);
            for (int i = 0; i < playerBulletList.Count; i++)
                playerBulletList[i].Update(gameTime);
            //update player bullet
            for (int i = 0; i < playerBulletList.Count; i++)
                playerBulletList[i].Update(gameTime);
            //update enemy bullet
            for (int i = 0; i < enemyBulletList.Count; i++)
                enemyBulletList[i].Update(gameTime);
            //update enemy 
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Update(gameTime);
            //update missile
            for (int i = 0; i < missileList.Count; i++)
                missileList[i].Update(gameTime);

            //detect collision
            DetectCollision();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //draw player
            player.Draw(spriteBatch, gameTime);
            //draw player bullet
            for (int i = 0; i < playerBulletList.Count; i++)
                playerBulletList[i].Draw(spriteBatch, gameTime);
            //draw enemy bullet
            for (int i = 0; i < enemyBulletList.Count; i++)
                enemyBulletList[i].Draw(spriteBatch, gameTime);
            //draw enemy 
            for (int i = 0; i < enemyList.Count; i++)
                enemyList[i].Draw(spriteBatch, gameTime);
            //draw missile
            for (int i = 0; i < missileList.Count; i++)
                missileList[i].Draw(spriteBatch, gameTime);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DetectCollision()
        {
            //detect collision between player and enemy
            for(int i = 0; i < enemyList.Count; i++)
            {
                if (player.position.X < enemyList[i].position.X + enemyList[i].texture.Width &&
                    player.position.X + player.texture.Width > enemyList[i].position.X &&
                    player.position.Y < enemyList[i].position.Y + enemyList[i].texture.Height &&
                    player.position.Y + player.texture.Height > enemyList[i].position.Y)
                {
                    player.health--;
                    enemyList[i].health--;

                    if(player.health <= 0)
                    {
                        //player lose
                        //proceed to lose menu
                    }

                    if(enemyList[i].health <= 0)
                        enemyList.Remove(enemyList[i]);

                    break;
                }
            }

            //detect collision between player bullet and enemy
            for(int i = 0; i < enemyList.Count; i++)
            {
                for(int j = 0; j < playerBulletList.Count; j++)
                {
                    if (enemyList[i].position.X < playerBulletList[i].position.X + playerBulletList[i].texture.Width &&
                    enemyList[i].position.X + enemyList[i].texture.Width > playerBulletList[i].position.X &&
                    enemyList[i].position.Y < playerBulletList[i].position.Y + playerBulletList[i].texture.Height &&
                    enemyList[i].position.Y + enemyList[i].texture.Height > playerBulletList[i].position.Y)
                    {
                        enemyList[i].health--;

                        if (enemyList[i].health <= 0)
                            enemyList.Remove(enemyList[i]);

                        playerBulletList.Remove(playerBulletList[i]);

                        break;
                    }
                }
            }

            //detect collision between enemy bullet and player
            for (int i = 0; i < enemyBulletList.Count; i++)
            {
                if (player.position.X < enemyBulletList[i].position.X + enemyBulletList[i].texture.Width &&
                    player.position.X + player.texture.Width > enemyBulletList[i].position.X &&
                    player.position.Y < enemyBulletList[i].position.Y + enemyBulletList[i].texture.Height &&
                    player.position.Y + player.texture.Height > enemyBulletList[i].position.Y)
                {
                    player.health--;

                    if (player.health <= 0)
                    {
                        //player lose
                        //proceed to lose menu
                    }

                    enemyBulletList.Remove(enemyBulletList[i]);

                    break;
                }
            }

            //detect collision between missile and target
            for (int i = 0; i < missileList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    if (object.ReferenceEquals(missileList[i].target, enemyList[j]))
                    {
                        if (missileList[i].position.X < enemyList[j].position.X + enemyList[j].texture.Width &&
                            missileList[i].position.X + missileList[i].texture.Width > enemyList[j].position.X &&
                            missileList[i].position.Y < enemyList[j].position.Y + enemyList[j].texture.Height &&
                            missileList[i].position.Y + missileList[i].texture.Height > enemyList[j].position.Y)
                        {
                            enemyList[j].health -= 3;
                            if (enemyList[j].health <= 0)
                                enemyList.Remove(enemyList[j]);

                            missileList[i].target = null;
                            missileList.Remove(missileList[i]);

                            break;
                        }
                    }
                }
            }
        }
    }
}
