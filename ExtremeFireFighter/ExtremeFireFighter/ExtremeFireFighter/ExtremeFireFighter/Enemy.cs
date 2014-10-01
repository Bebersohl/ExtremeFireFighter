using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ExtremeFireFighter
{
    class Enemy: Sprite
    {
        private int health;

        public int health_
        {
            get { return health; }
            set { health = value; }
        }

        public void applyDamage(int damage)
        {
            health -= damage;
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public Enemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            //base health for testing out projectile damage
            health_ = 1;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move sprite based on direction
            position += direction;

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 47)
                position.Y = 47;
            if ((position.X >= 230 && position.X <= 275) || (position.X >= 780 && position.X <= 825))
            {
                if (position.Y < 57)
                {
                    position.Y = 57;
                }
            }
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            

            base.Update(gameTime, clientBounds);
        }

        public virtual void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {

        }

    }
}
