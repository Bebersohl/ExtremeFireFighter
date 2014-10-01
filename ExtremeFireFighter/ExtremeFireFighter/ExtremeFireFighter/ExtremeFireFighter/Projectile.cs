using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace ExtremeFireFighter
{
    class Projectile: Sprite
    {
        //true is player false is enemy
        Boolean playerProjectile;
        int projectileDamage;

        public int Projectile_Damage
        {
            set { projectileDamage = value; }
            get { return projectileDamage; }
        }

        public Boolean Player_Projectile
        {
            get { return playerProjectile; }
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public Projectile(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, Boolean attackingSprite, int damage)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            playerProjectile = attackingSprite;
            this.projectileDamage = damage;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            // Move sprite based on direction
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
