﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    public class PlayerBullet : GameObject
    {
        public override void Initialize()
        {
            //initialize all the variables
            speed = 300.0f;
            name = "playerBullet";
            position = Game1.player.position;
            texture = Game1.assets["playerBullet"];
            orientation = 0f;
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(position.X > Game1.screenWidth || position.X < 0 || position.Y > Game1.screenHeight
                || position.Y < 0)
            {
                Game1.playerBulletList.Remove(this);
            }                         
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, orientation, origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}