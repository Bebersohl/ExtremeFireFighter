using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExtremeFireFighter
{
    class ShootingEnemy : Enemy
    {
        int projectileDamage;
        int movementCount = 0;
        Random numberGenerator = new Random();
        protected Vector2[] movementDirections = new Vector2[4] { new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1), new Vector2(0,-1) };

        public int Projectile_Damage
        {
            set { projectileDamage = value; }
            get { return projectileDamage; }
        }



        public ShootingEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            this.setDirection(movementDirections[numberGenerator.Next(0, 4)]);
        }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {
            
            if (movementCount == 30)
            {
                int direction = numberGenerator.Next(0, 4);
                System.Diagnostics.Debug.WriteLine(direction);
                
                this.setDirection(movementDirections[direction]);
                movementCount = 0;
            }
            else
                movementCount++;
        }


        public virtual Boolean canFireProjectile(GameTime gameTime)
        {
                return false;
        }

    }
}
